using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public abstract class PluginBase : Plugin, ISchedulablePlugin
    {
        #region Properties

        /// <summary>
        /// Flag that indicates if the plugin is initialized. Need to be set to true in childrens
        /// </summary>
        protected bool isInitialized;

        #endregion

        #region Plugin Implementation        

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);


        public PluginBase(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            isInitialized = false;
        }

        #endregion

        #region ISchedulablePlugin Implementation

        public virtual void Init(object[] args)
        {
            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;
            ClientManager.ClientDisconnected += OnClientDisconnected;

        }

        #endregion

        #region Message events

        /// <summary>
        /// Called when a client is connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnClientMessageReceived;
        }

        /// <summary>
        /// Called when a client is disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            
        }

        /// <summary>
        /// Called when a message is received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {

        }

        #endregion

        #region Implementation

        /// <summary>
        /// To be overriden
        /// </summary>
        /// <returns></returns>
        public virtual bool IsInitialized()
        {
            return isInitialized;
        }

        #endregion
    }
}
