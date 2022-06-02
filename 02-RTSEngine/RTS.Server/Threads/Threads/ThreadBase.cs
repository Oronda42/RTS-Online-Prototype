using RTS.Database;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace RTS.Server
{
    /// <summary>
    /// Role of the thread
    /// </summary>
    public enum RoleOfThreadConsumerProducer
    {
        BOTH = 0,
        CONSUMER = 1,
        PRODUCER = 2
    }

    public class ThreadBase
    {
        #region Properties

        /// <summary>
        /// instance of the Thread.
        /// </summary>
        public Thread ThreadReference { get; set; }

        /// <summary>
        /// Consummers that consume the EventQueue
        /// </summary>
        public List<ThreadBase> ConsumerList;

        /// <summary>
        /// Role of the thread
        /// </summary>
        public RoleOfThreadConsumerProducer Role { get; protected set; }

        /// <summary>
        /// Reference to the concurrent queue
        /// </summary>
        protected ConcurrentQueue<ThreadEvent> EventQueue { get; set; }

        /// <summary>
        /// Locker for the event queue
        /// </summary>
        public static readonly object EventQueueLocker = new object();

        /// <summary>
        /// Current event performing
        /// </summary>
        public ThreadEvent CurrentEvent { get; protected set; }

        /// <summary>
        /// Database connection (if there is one=
        /// </summary>
        public DatabaseConnection DBConnection { get; protected set; } = null;

        #endregion

        #region Constructor

        public ThreadBase()
        {
            EventQueue = new ConcurrentQueue<ThreadEvent>();
            ConsumerList = new List<ThreadBase>();
            ThreadReference = new Thread(MainThreadLoop);
        }


        #endregion

        #region Implementation

        /// <summary>
        /// To be overriden
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        /// Start the thread function
        /// </summary>
        public virtual void Start()
        {
            ThreadReference.Start();
        }

        protected virtual void MainThreadLoop()
        {

        }

        /// <summary>
        /// Add a consumer who can consume our event queue. If the current thread is consumer only, it doesn't produce event to his queue
        /// </summary>
        /// <param name="pConsumer"></param>
        public virtual void AddConsumer(ThreadBase pConsumer)
        {
            if (Role == RoleOfThreadConsumerProducer.CONSUMER)
                return;

            ConsumerList.Add(pConsumer);
        }

        /// <summary>
        /// To be overriden
        /// </summary>
        public virtual void Stop()
        {
            if (DBConnection != null)
                DBConnection.Connection.Close();
        }

        /// <summary>
        /// Add and event to the queue
        /// </summary>
        /// <param name="pAction"></param>
        public virtual void EnqueueEvent(ThreadEvent pAction)
        {
            //If the thread is a producer only, it can't add on it's own queue
            if (Role == RoleOfThreadConsumerProducer.PRODUCER)
                return;

            //Lock and add into the queue
            lock (EventQueueLocker)
            {
                EventQueue.Enqueue(pAction);
                //pulse waiters
                Monitor.Pulse(EventQueueLocker);
            }

            
        }

        /// <summary>
        /// To be overriden
        /// </summary>
        protected virtual void PerformEvent()
        {

        }


        #endregion
    }
}
