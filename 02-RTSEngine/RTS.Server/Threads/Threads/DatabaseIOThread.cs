using System;
using System.Threading;
using MySql.Data.MySqlClient;
using RTS.Database;


namespace RTS.Server
{

    public class DatabaseIOThread : ThreadBase
    {
        #region Properties

        public static DatabaseIOThread Instance { get; private set; }

        public MySqlConnection connection;

        #endregion

        #region Constructor

        public DatabaseIOThread() : base()
        {
            Role = RoleOfThreadConsumerProducer.CONSUMER;
            ThreadReference.Name = "DispatcherThread";
            Instance = this;
        }

        #endregion

        #region Implementation

        public void Init(MySqlConnectionStringBuilder pConnectionString)
        {
            base.Init();

            //Initialize data
            DBConnection = new DatabaseConnection(pConnectionString);
            DBConnection.Connect();

            ColoredConsole.WriteLine("[DatabaseIO Thread] Init ", ConsoleColor.Green);
        }

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
        /// Perform the action
        /// </summary>
        protected override void PerformEvent()
        {
            DatabaseEvent dbEvent = (DatabaseEvent)CurrentEvent;
            var result = dbEvent.DatabaseCallback.DynamicInvoke(dbEvent.Arguments);

            Console.WriteLine(result);

            CurrentEvent = null;
        }
        
        #endregion
    }
}
