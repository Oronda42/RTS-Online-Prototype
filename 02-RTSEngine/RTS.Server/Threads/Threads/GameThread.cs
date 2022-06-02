using MySql.Data.MySqlClient;
using RTS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class GameThread : ThreadBase
    {
        public static GameThread Instance { get; private set; }
                
        #region Constructor

        public GameThread() : base()
        {
            Role = RoleOfThreadConsumerProducer.CONSUMER;

            ThreadReference.Name = "GameThread";
            Instance = this;
        }
        
        #endregion

        /// <summary>
        /// Initialize the thread
        /// </summary>
        public void Init(MySqlConnectionStringBuilder pConnectionString)
        {
            base.Init();

            //Initialize data
            DBConnection = new DatabaseConnection(pConnectionString);
            DBConnection.Connect();

            ColoredConsole.WriteLine("[Game Thread] Init ", ConsoleColor.Green);

        }

        /// <summary>
        /// Main function executed when the thread start
        /// </summary>
        protected override void MainThreadLoop()
        {            
            try
            {
                while (true)
                {
                    lock (EventQueueLocker)
                    {
                        //If there is no action
                        while (EventQueue.Count == 0)
                            //Release the lock
                            Monitor.Wait(EventQueueLocker, 200);

                        if (EventQueue.TryDequeue(out ThreadEvent pickedAction))
                        {
                            CurrentEvent = pickedAction;
                        }
                    }

                    PerformEvent();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Perform event on the queue
        /// </summary>
        protected override void PerformEvent()
        {
            //Cast to a game event
            GameEvent gameEvent = (GameEvent)CurrentEvent;

            //Execute the callback in the specified event
            var result = gameEvent.CallBack.DynamicInvoke(gameEvent.Arguments);

            //Set current event to null
            CurrentEvent = null;
        }

    }
}
