using DarkRift;
using DarkRift.Server;
using RTS.Server.Messages;
using RTS.Database;
using RTS.Models;
using System;

namespace RTS.Plugins
{
    public class PlayerCenterManager : Plugin
    {
        #region Plugin implementation
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public PlayerCenterManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        public void Init()
        {
            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;
        }

        #endregion

        #region Connection 

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnClientMessageReceived;
        }

        #endregion

        #region Message handle

        private void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case CommunicationTag.PlayerCenter.PLAYER_CENTER_REQUEST:
                    SendPlayerCenter(e);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Send the player center 
        /// </summary>
        /// <param name="e"></param>
        private void SendPlayerCenter(MessageReceivedEventArgs e)
        {
            using (DarkRiftReader reader = e.GetMessage().GetReader())
            {
                //Get credential
                CredentialMessage request = e.GetMessage().Deserialize<CredentialMessage>();

                //If player is authorized
                if (ConnectionManager.instance.CheckClientToken(request.playerId, request.token))
                {
                    //Get the player center
                    //Console.WriteLine("[RTS] INFO : Sending player center for player N° " + request.playerId );
                    //PlayerBuildingCenterModel playerCenter = PlayerBuildingCenterFactory.Get(request.playerId);

                    //Create the serialized message
                    //PlayerCenterMessage playerCenterMessage = new PlayerCenterMessage { Model = playerCenter };

                    //Send the message
                    //using (Message response = Message.Create(CommunicationTag.PlayerCenter.PLAYER_CENTER_RESPONSE, playerCenterMessage))
                    //{
                    //    e.Client.SendMessage(response, SendMode.Reliable);
                    //}
                }

            }
        }

        #endregion

    }
}
