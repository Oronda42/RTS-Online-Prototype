using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;
using RTS.Server.Messages;
using RTS.Models.Server;
using RTS.Server;


namespace RTS.Server
{
    public abstract class ServerInitializationPluginBase : PluginBase
    {
        #region Properties

        /// <summary>
        /// Data used to start the DarkRift server from the console
        /// </summary>
        public ServerSpawnData SpawnData { get; set; } = null;

        /// <summary>
        /// Information about this server
        /// </summary>
        public ServerInstanceInformation InstanceInformation { get; set; }

        /// <summary>
        /// A ref to the Mysql Connection
        /// </summary>
        public DatabaseConnection DBConnection { get; protected set; }

        /// <summary>
        /// Scheduler of plugins
        /// </summary>
        protected PluginSchedulerManager PluginScheduler { get; set; }
             

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public ServerInitializationPluginBase(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region Implementation

        /// <summary>
        /// Initialize the plugin
        /// </summary>
        /// <param name="pSpawnData"></param>
        /// <param name="pEnvironment"></param>
        /// <param name="pName"></param>
        public virtual void Init(ServerSpawnData pSpawnData, string pEnvironment, string pName, ServerCustomConfig pConfigFile)
        {
            //Build connection string
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = pConfigFile.Database.Host,
                Database = pConfigFile.Database.DatabaseName,
                UserID = pConfigFile.Database.User,
                Password = pConfigFile.Database.Password,
                Port = pConfigFile.Database.Port,
                AllowZeroDateTime = false,
                ConvertZeroDateTime = false
            };

            //Initialize data
            DBConnection = new DatabaseConnection(connectionStringBuilder);
            DBConnection.Connect();

            SpawnData = pSpawnData;
            PluginScheduler = new PluginSchedulerManager();

            //Create information
            InstanceInformation = new ServerInstanceInformation
            {
                ClientConnection = null,    //No connection to itself
                ServerInstanceModel = new ServerInstanceModel
                {
                    Name = pName,
                    Environment = pEnvironment,
                    Host = SpawnData.Listeners.NetworkListeners[0].Address.ToString(),
                    Port = SpawnData.Listeners.NetworkListeners[0].Port,
                    Token = TokenService.GenerateNewToken()
                }
            };

            //Update database
            ServerFactory.CreateOrUpdateServerInstance(DBConnection.Connection, InstanceInformation.ServerInstanceModel);

            //INIT THE DISPATCHER THREAD
            DispatcherThread dispatcherThread = new DispatcherThread();
            dispatcherThread.Init();

            //INIT THE LOGGING THREAD
            LoggingThread loggingThread = new LoggingThread(pConfigFile.Log);
            loggingThread.Init();

            ///INIT THE DATABASEIO THREAD
            DatabaseIOThread dbIOThread = new DatabaseIOThread();
            dbIOThread.Init(connectionStringBuilder);

            ///INIT THE GAME THREAD
            GameThread gameThread = new GameThread();
            gameThread.Init(connectionStringBuilder);            
            
            //Start threads           
            dispatcherThread.Start();
            loggingThread.Start();
            dbIOThread.Start();
            gameThread.Start();

        }

        /// <summary>
        /// The function is called when the server shut down
        /// </summary>
        public virtual void Stop()
        {
            Console.WriteLine("Stoping Server");
            try
            {
                //Delete server instance from database
                ServerFactory.DeleteServerInstance(DBConnection.Connection, InstanceInformation.ServerInstanceModel);

                GameThread.Instance.Stop();
                DatabaseIOThread.Instance.Stop();
                LoggingThread.Instance.Stop();
                DispatcherThread.Instance.Stop();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                ColoredConsole.WriteLine("Press key to exit", ConsoleColor.Red);
                Console.ReadKey();
            }
            finally
            {
                ColoredConsole.WriteLine("Connexion to database closed", ConsoleColor.Green);
                DBConnection.Connection.Close();
            }

            
        }

        #endregion

        #region Command

        public override Command[] Commands => new Command[]
        {
            new Command("GetLogLevel", "Current LogLevel of the Logging Thread", "GetLogLevel", GetLogLevelCommand),
            new Command("SetLogLevel", "Set the LogLevel of the Logging Thread (DEBUG = 1, INFO = 2,WARNING = 3,ERROR = 4) ", "SetLogLevel -level value", SetLogLevelCommand),
        };

        protected void GetLogLevelCommand(object sender, CommandEventArgs e)
        {
            Console.WriteLine(LoggingThread.Instance.currentLogLevel);
        }

        protected void SetLogLevelCommand(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.Arguments.Length == 0 || e.Flags.Count == 0)
                {
                    ColoredConsole.WriteLine("Wrong arguments or -flags, Type 'help SetLogLevel' for more infos", ConsoleColor.Red);
                    return;
                }

                if (e.Flags.Keys[0] == "level")
                {
                    int arg = int.Parse(e.Arguments[0]);
                    if (arg >= 1 && arg <= 4)
                    {
                        LogLevel tempLevel = (LogLevel)Enum.ToObject(typeof(LogLevel), int.Parse(e.Arguments[0]));
                        LoggingThread.Instance.currentLogLevel = tempLevel;
                        ColoredConsole.WriteLine("Log Level Set To : " + LoggingThread.Instance.currentLogLevel, ConsoleColor.Green);
                    }
                    else                    
                        ColoredConsole.WriteLine("Wrong level, Type 'help SetLogLevel' for more Infos", ConsoleColor.Red);
                    

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetLogLevel Command Error :"+ ex.Message);
            }

        }
        #endregion
    }
}
