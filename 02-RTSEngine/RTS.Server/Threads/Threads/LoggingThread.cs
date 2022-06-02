using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTS.Server
{
    #region Enums
    /// <summary>
    /// The Level of information you want in the logging message
    /// </summary>
    public enum LogLevel
    {
        DEBUG = 1,
        INFO = 2,
        WARNING = 3,
        ERROR = 4,
    }

    /// <summary>
    /// The target destination of Logging
    /// </summary>
    public enum LogDestination
    {
        CONSOLE = 1,
        FILE = 2,
        FILE_AND_CONSOLE = 3
    }
    #endregion

    public class LoggingThread : ThreadBase
    {
        #region Properties

        /// <summary>
        /// The path where the log is written
        /// </summary>
        public string outputDirectory;

        /// <summary>
        /// the Name of the Logging File
        /// </summary>
        public string outputFileName;

        /// <summary>
        /// The complet Path to the Logging file
        /// </summary>
        public string loggingFilePath;
        /// <summary>
        /// the max size of the file
        /// </summary>
        public int logSizeLimit = 100;

        /// <summary>
        /// Current Level Of Logging
        /// </summary>
        public LogLevel currentLogLevel;

        /// <summary>
        /// Current target destination of Logging
        /// </summary>
        public LogDestination currentLogDestination;

        /// <summary>
        /// Use for I/O opérations in a file
        /// </summary>
        FileStream fileStream;

        public static LoggingThread Instance { get; private set; }

        #endregion

        #region Constructor

        public LoggingThread(LogConfig pConfig) : base()
        {
            Role = RoleOfThreadConsumerProducer.CONSUMER;

            if (!Enum.TryParse(pConfig.Level, out currentLogLevel))
                throw new Exception("Log Level not supported : " + pConfig.Level);

            if (!Enum.TryParse(pConfig.Destination, out currentLogDestination))
                throw new Exception("Destination not supported : " + pConfig.Destination);

            outputDirectory = pConfig.OutputDirectory;
            outputFileName = pConfig.OutputFileName;

            if (outputDirectory == null || outputFileName == null)
            {
                throw new Exception("[Logging Thread] Please Specify a Path and a name for the Logging to be written");
            }

            ThreadReference.Name = "LoggingThread";
            Instance = this;
        }

        #endregion

        #region Implementation

        public override void Init()
        {
            base.Init();

            if (currentLogDestination == LogDestination.FILE || currentLogDestination == LogDestination.FILE_AND_CONSOLE)
                CreateFile();

            ColoredConsole.WriteLine("[Logging Thread] Init ", ConsoleColor.Green);
        }

        /// <summary>
        /// Create A file
        /// </summary>
        private void CreateFile()
        {
            loggingFilePath = outputDirectory + "\\" + outputFileName + "-" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".log";
            if (!File.Exists(loggingFilePath)) //No File? Create
            {
                fileStream = File.Create(loggingFilePath);
                fileStream.Close();
            }

            WriteFileHeader();
        }

        /// <summary>
        /// Write header to file
        /// </summary>
        private void WriteFileHeader()
        {
            TryWriteToFile("Date | MessageId | LogLevel | Message | Source");
        }

        public override void EnqueueEvent(ThreadEvent pAction)
        {
            try
            {
                LoggingEvent log = (LoggingEvent)pAction;
                if ((int)log.logLevel >= (int)currentLogLevel)
                    base.EnqueueEvent(pAction);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Start the thread
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
        /// Perform the action
        /// </summary>
        protected override void PerformEvent()
        {
            CreateOrUpdateLog((LoggingEvent)CurrentEvent);
            CurrentEvent = null;
        }

        /// <summary>
        /// Create A file if not exist, or update a file if already exist
        /// </summary>
        /// <param name="threadEvent"></param>
        /// <returns></returns>
        public bool CreateOrUpdateLog(LoggingEvent threadEvent)
        {
            if (currentLogDestination == LogDestination.FILE || currentLogDestination == LogDestination.FILE_AND_CONSOLE)
            {
                /// Cheking if Thread si active since More than One day
                if ((File.GetCreationTime(loggingFilePath) - DateTime.Now) > TimeSpan.FromDays(1))
                {
                    CreateFile();
                }
                //// Cheking if File is not too big (100mB) else create new
                if (File.ReadAllBytes(loggingFilePath).Length >= logSizeLimit * 1024 * 1024)
                {
                    CreateFile();
                }
            }

            //TARGET THE RIGHT DESTINATION  
            switch (currentLogDestination)
            {
                case LogDestination.CONSOLE:
                    WriteToConsole(threadEvent);
                    break;

                case LogDestination.FILE:
                    return (TryWriteToFile(threadEvent));

                case LogDestination.FILE_AND_CONSOLE:
                    WriteToConsole(threadEvent);
                    return (TryWriteToFile(threadEvent));
            }

            return true;
        }


        /// <summary>
        /// Try to Write on the Logging file
        /// </summary>
        /// <param name="pLogMessage"></param>
        /// <returns></returns>
        public bool TryWriteToFile(LoggingEvent pLog)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(loggingFilePath))
                {
                    string threadSource = "";
                    threadSource = pLog.ThreadSource?.GetType().Name;

                    writer.WriteLine("{0} | {1} | {2} | {3} | {4} ",
                    DateTime.Now.ToString("ddMMyyyy_HHmmss"),
                    pLog.Id,
                    pLog.logLevel.ToString(),
                    pLog.message, threadSource);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Try to write to file
        /// </summary>
        /// <param name="pMessage"></param>
        /// <returns></returns>
        public bool TryWriteToFile(string pMessage)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(loggingFilePath))
                {
                    writer.WriteLine(pMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Write the LogEvent to the Console
        /// </summary>
        /// <param name="pLogMessage"></param>
        public void WriteToConsole(LoggingEvent pLog)
        {
            string threadSource = "";
            threadSource = pLog.ThreadSource?.GetType().Name;

            ConsoleColor color = ConsoleColor.White;

            if (pLog.logLevel == LogLevel.ERROR)
                color = ConsoleColor.Red;

            if (pLog.logLevel == LogLevel.INFO)
                color = ConsoleColor.Gray;

            if (pLog.logLevel == LogLevel.WARNING)
                color = ConsoleColor.Yellow;

            ColoredConsole.WriteLine(
                 string.Format("{0} | {1} | {2} | {3} | {4} ",
                     DateTime.Now.ToString("ddMMyyyy_HHmmss"),
                     pLog.Id,
                     pLog.logLevel.ToString(),
                     pLog.message, threadSource)
                , color);
        }

        #endregion

    }
}
