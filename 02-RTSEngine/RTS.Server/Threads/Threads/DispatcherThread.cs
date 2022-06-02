using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class DispatcherThread : ThreadBase
    {
        #region Properties

        public static DispatcherThread Instance { get; private set; }

        #endregion

        #region Constructor
        public DispatcherThread() : base()
        {
            Role = RoleOfThreadConsumerProducer.CONSUMER;
            ThreadReference.Name = "DispatcherThread";
            Instance = this;
        }
        #endregion

        #region Implementation

        /// <summary>
        /// Initialize the thread
        /// </summary>
        public override void Init()
        {
            base.Init();
            ColoredConsole.WriteLine("[Dispatcher Thread] Init ", ConsoleColor.Green);
        }

        /// <summary>
        /// Main loop of the thread
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
                        {
                            //Release the lock
                            Monitor.Wait(EventQueueLocker, 200);
                        }

                        if (EventQueue.TryDequeue(out ThreadEvent pickedAction))
                            CurrentEvent = pickedAction;
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
        /// Execute the task in the queue
        /// </summary>
        protected override void PerformEvent()
        {
            Dispatch(CurrentEvent);
            CurrentEvent = null;
        }

        /// <summary>
        /// Dispatch events to other thread
        /// </summary>
        /// <param name="pCurrentEvent"></param>
        private void Dispatch(ThreadEvent pCurrentEvent)
        {
            switch (pCurrentEvent)
            {
                case LoggingEvent logginEvent:
                    LoggingThread.Instance.EnqueueEvent(logginEvent);
                    break;

                case DatabaseEvent dbEvent:
                    DatabaseIOThread.Instance.EnqueueEvent(dbEvent);
                    break;

                case GameEvent gameEvent:
                    GameThread.Instance.EnqueueEvent(gameEvent);
                    break;
            }
        }

        #endregion
    }

}

