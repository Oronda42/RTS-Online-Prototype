using DarkRift;
using DarkRift.Dispatching;
using DarkRift.Server;
using RTS.Configuration;
using RTS.Database;
using RTS.Models;
using RTS.Models.Server;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.WorldServer
{
    public static class WorldPlayerConnexionManager
    {
        /// <summary>
        /// Try to connect 
        /// </summary>
        /// <param name="pPlayerCredentials"></param>
        public static void TryIdentifyPlayer(IClient pClient, CredentialMessage pPlayerCredentials)
        {
            try
            {
                //Log
                LoggingEvent log = new LoggingEvent(LogLevel.DEBUG, "Player " + pPlayerCredentials.playerId + " try to connect", null);
                DispatcherThread.Instance.EnqueueEvent(log);

                //Check player information
                PlayerSessionModel playerSession = PlayerSessionFactory.GetByPlayerId(
                    WorldServerInitializationPlugin.Instance.DBConnection.Connection,
                    pPlayerCredentials.playerId);

                ///////////////////////////////////
                /// Check for valid player
                if (playerSession == null)
                {
                    WorldPlayerCommunicationPlugin.Instance.SendDisconnectionMessage(pClient, DisconnectionErrorCode.INVALID_TOKEN);
                    throw new Exception("No player session found for : " + pPlayerCredentials.playerId);
                }

                ///////////////////////////////////
                /// Check for valid session
                if (playerSession.Token != pPlayerCredentials.token)
                {
                    WorldPlayerCommunicationPlugin.Instance.SendDisconnectionMessage(pClient, DisconnectionErrorCode.INVALID_TOKEN);
                    throw new Exception("received token : " + pPlayerCredentials.token + " - Database token : " + playerSession.Token);
                }

                /////////////////////////
                /// If player already exists                
                ClientInformation client = WorldPlayerCommunicationPlugin.Instance.GetClientByPlayerId(pPlayerCredentials.playerId);
                if (client != null)
                {
                    DispatcherThread.Instance.EnqueueEvent(
                    new LoggingEvent(LogLevel.WARNING, string.Format("Player is already connected, remove the last session for player id {0}.", pPlayerCredentials.playerId), null));

                    WorldPlayerCommunicationPlugin.Instance.RemoveClientByPlayerId(pPlayerCredentials.playerId);
                }

                //Add the client
                client = new ClientInformation
                {
                    ClientId = pClient.ID,
                    PlayerId = pPlayerCredentials.playerId,
                    Token = pPlayerCredentials.token,
                    DeviceId = "", //Doesn't need it
                    PlayerSession = playerSession,
                    Simulation = null //Doesn't need it
                };

                WorldPlayerCommunicationPlugin.Instance.AddClient(client);

                ///////////////////////////////////
                /// Get player world zone
                //TODO : Get player zone 

                ///////////////////////////////////
                /// Check for valid world zone
                GameServerZoneModel gameServerZone = WorldServerData.GetGameServerNameForZone(1);
                if (gameServerZone == null)
                {
                    WorldPlayerCommunicationPlugin.Instance.SendDisconnectionMessage(pClient, DisconnectionErrorCode.INVALID_WORLD_ZONE);
                    throw new Exception("invalid world zone asked by player : " + pPlayerCredentials.playerId);
                }

                ///////////////////////////////////
                /// Send GameServer information to player
                ServerInstanceInformation gameServerInformation = WorldInterServerCommunicationPlugin.Instance.GetServerByName(gameServerZone.ServerName);
                if (gameServerInformation == null)
                {
                    WorldPlayerCommunicationPlugin.Instance.SendDisconnectionMessage(pClient, DisconnectionErrorCode.INVALID_GAME_SERVER);
                    throw new Exception("Unable to redirect to : " + gameServerZone.ServerName +". Check if the server is started");
                }

                LoginResponseMessage loginResponseMessage = new LoginResponseMessage()
                {
                    RedirectedServerName = gameServerInformation.ServerInstanceModel.Name,
                    RedirectedServerHost = gameServerInformation.ServerInstanceModel.Host,
                    RedirectedServerPort = gameServerInformation.ServerInstanceModel.Port,
                    loginErrorCode = (int)LoginErrorCode.NO_ERROR,
                };

                //Create and send message
                using (Message loginMessage = Message.Create(CommunicationTag.Connection.LOGIN_RESPONSE, loginResponseMessage))
                {
                    pClient.SendMessage(loginMessage, SendMode.Reliable);
                }


                log = new LoggingEvent(LogLevel.DEBUG, string.Format("Player {0} will be redirected to the game server {1}", pPlayerCredentials.playerId, gameServerInformation.ServerInstanceModel.Name), null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }
    }
}
