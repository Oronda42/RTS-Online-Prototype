using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using DarkRift.Server;

namespace RTS.Server.Console
{
    class Console
    {
        #region Delegates

        private delegate bool ConsoleEventDelegate(int eventType);

        #endregion

        #region Properties
        public static DarkRiftServer Server { get; set; }
        public static string ConfigFilePath { get; set; }
        public static string Environment { get; set; }
        public static string InitializationPluginName { get; set; }
        public static string ServerName { get; set; }
        public static string CustomConfig { get; set; }

        /// <summary>
        /// Use dynamic type because cast doesn't work
        /// </summary>
        public static dynamic InitializePlugin { get; set; }

        static DarkRiftServer darkRiftServer;


        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);
        static EventHandler handler;

        enum CtrlType : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            System.Console.WriteLine("Cleaning Before Exit");
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                    System.Console.WriteLine(sig + ": Exiting system due to external CTRL-C");
                    return true;                    
                                                    
                case CtrlType.CTRL_LOGOFF_EVENT:    
                    System.Console.WriteLine(sig + ": Exiting system due to external CTRL-C, or process kill, or shutdown");
                    return false;                   
                                                    
                case CtrlType.CTRL_SHUTDOWN_EVENT:  
                    System.Console.WriteLine(sig + ": Exiting system due to shutdown");
                    return false;

                case CtrlType.CTRL_CLOSE_EVENT:
                  
                    System.Console.WriteLine(sig + ": Exiting system due to User Command");
                    Thread.Sleep(500);
                    try
                    {
                        InitializePlugin.Stop();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    return true;

                case CtrlType.CTRL_BREAK_EVENT:
                    System.Console.WriteLine(sig + ": Exiting system due to external CTRL-C, or process kill, or shutdown");
                    return false;

                default:
                    System.Console.WriteLine(sig + ": Exiting system due to external CTRL-C, or process kill, or shutdown");
                    return false;
            }
        }

        #endregion

        /// <summary>
        /// Main server
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Handle Closing
            handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(handler, true);

            ////To debug
            //System.Console.WriteLine("Press a key to continue");
            //System.Console.ReadKey();


            System.Console.WriteLine(@"
##############################
            ");

            //Get parameters, if a parameters is missing, doesn't start the server
            if (args.Length != 5)
            {
                System.Console.WriteLine("Invalid Arguments : 0=ConfigFilePath, 1=Environment, 2=PluginInitializationName, 3=ServerName 4=ConfigFile. Press a key to exit");
                System.Console.ReadKey();
                return;
            }

            //-ConfigFilePath 
            ConfigFilePath = args[0];
            System.Console.WriteLine("arg[0] - Configuration : " + ConfigFilePath);

            //-Environment
            Environment = args[1];
            System.Console.WriteLine("arg[1] - Environment : " + Environment);

            //-PluginInitializationName
            InitializationPluginName = args[2];
            System.Console.WriteLine("arg[2] - Initialization plugin name : " + InitializationPluginName);

            //-ServerName
            ServerName = args[3];
            System.Console.WriteLine("arg[3] - Server name : " + ServerName);

            //-CustomConfig
            CustomConfig = args[4];
            System.Console.WriteLine("arg[4] - CustomConfig : " + CustomConfig);

            System.Console.WriteLine(@"
############################## 

            ");

            //Create configuration spawn data
            ServerSpawnData serverData = ServerSpawnData.CreateFromXml(ConfigFilePath, null);

            //Create a customConfig
            string xmlConfig = File.ReadAllText(CustomConfig);

            ServerCustomConfig configPath = new ServerCustomConfig();
            configPath = XmlConverterService.Deserialize<ServerCustomConfig>(xmlConfig);

            //Start the server
            darkRiftServer = new DarkRiftServer(serverData);
            darkRiftServer.Start();

            // Store A reference to Initiliazation Plugin
            try
            {
                InitializePlugin = darkRiftServer.PluginManager.GetPluginByName(InitializationPluginName);

                //Server Initialization
                if (InitializePlugin != null)
                {
                    InitializePlugin.Init(serverData, Environment, ServerName, configPath);
                    //InitializePlugin.Init(serverData, Environment, ServerName, "");
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Fail to load Initiliazation Plugin :" + e.Message);
                throw e;
            }

            //Récupération du plugin d'initialization
            while (!InitializePlugin.IsInitialized())
            {
                System.Console.WriteLine("Waiting for Initialization");
            }

            //Mettre a jour les informations grace au plugin CommonPlugin
            System.Console.Title = string.Format("{0}:{1}---{2}---{3}",
                InitializePlugin.InstanceInformation.ServerInstanceModel.Name, 
                InitializePlugin.InstanceInformation.ServerInstanceModel.Port,
                InitializePlugin.InstanceInformation.ServerInstanceModel.Environment,
                InitializePlugin.InstanceInformation.ServerInstanceModel.Token);


            /// create a new thread for Handling Commands
            Thread ConsoleThread = new Thread(new ThreadStart(CommandLoop));
            ConsoleThread.Name = "ConsoleInputThread";
            ConsoleThread.Start();
            
            for (; ; )
            {
                darkRiftServer.DispatcherWaitHandle.WaitOne();
                darkRiftServer.ExecuteDispatcherTasks();
            }

        }
        private static void CommandLoop()
        {
            while (!darkRiftServer.Disposed)
            {
                string input = System.Console.ReadLine();
                darkRiftServer.ExecuteCommand(input);
            }
        }
    }
}


