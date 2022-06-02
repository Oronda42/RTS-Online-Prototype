using DarkRift;
using DarkRift.Server;
using RTS.Server.Messages;
using RTS.Database;
using RTS.Models;
using System;

namespace RTS.Plugins
{
    class PlayerResourceManager : Plugin
    {
        #region Plugin implementation
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public PlayerResourceManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
        }

        public void Init()
        {
            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;
            Console.WriteLine("PlayerRessourceManager initialized successfully");
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
                case CommunicationTag.PlayerConsumableResources.ALL_RESOURCE_REQUEST:
                    SendAllResources(e);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Implementation

        private void SendAllResources(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerConsumableResources.ALL_RESOURCE_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    PlayerResourceRequestMessage request = e.GetMessage().Deserialize<PlayerResourceRequestMessage>();

                                       
                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(request.playerId, request.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(request.playerId);

                        PlayerResourcesResponseMessage resourceMessageData = new PlayerResourcesResponseMessage
                        {
                            ResourceBag = client.Simulation.Player.resourceBag,
                        };

                        //Create and send message
                        using (Message resourceMessage = Message.Create(CommunicationTag.PlayerConsumableResources.ALL_RESOURCE_RESPONSE, resourceMessageData))
                        {
                            Console.WriteLine(string.Format("[RTS] INFO : Ressource sent to player {0}", client.Simulation.Player.id));
                            e.Client.SendMessage(resourceMessage, SendMode.Reliable);
                        }
                    }
                }
            }
        }

        #endregion

    }
}