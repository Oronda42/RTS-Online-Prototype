using DarkRift.Server;
using RTS.Plugins;
using System;
using System.Threading.Tasks;

namespace RTS
{
    /// <summary>
    /// Plugin wich schedule initialization of darkrift plugins
    /// </summary>
    class SchedulerManager : Plugin
    {
        #region Plugin implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public SchedulerManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            //To debug
            //Console.ReadKey();

            //Initialize plugins 
            InitAsync(PluginManager);
        }

        /// <summary>
        /// Initialize plugins
        /// </summary>
        /// <param name="pPluginManager"></param>
        /// <returns></returns>
        private static async Task InitAsync(IPluginManager pPluginManager)
        {
            //Free execution
            await Task.Yield();

            bool arePluginsLoaded = false;
            while (!arePluginsLoaded)
            {
                try
                { 
                    pPluginManager.GetPluginByType<SchedulerManager>();
                    arePluginsLoaded = true;
                }
                catch (Exception)
                {
                    //Console.WriteLine("Darkrift hasn't finished to load plugins");
                }
            }

            Console.WriteLine("Scheduler initialization start");
            //////////////////////////
            /// Services
            PlayerMapService.Init();

            //////////////////////////
            /// Plugins
            ((ClockManager)pPluginManager.GetPluginByType<ClockManager>()).Init();
            ((ConnectionManager)pPluginManager.GetPluginByType<ConnectionManager>()).Init();
            ((PlayerBuildingManager)pPluginManager.GetPluginByType<PlayerBuildingManager>()).Init();
            ((PlayerCityManager)pPluginManager.GetPluginByType<PlayerCityManager>()).Init();
            ((PlayerMapManager)pPluginManager.GetPluginByType<PlayerMapManager>()).Init();
            ((PlayerResourceManager)pPluginManager.GetPluginByType<PlayerResourceManager>()).Init();
            ((PlayerMarketManager)pPluginManager.GetPluginByType<PlayerMarketManager>()).Init();
            ((PlayerCenterManager)pPluginManager.GetPluginByType<PlayerCenterManager>()).Init();

            Console.WriteLine("Scheduler initialization finished");
        }



        #endregion
    }
}
