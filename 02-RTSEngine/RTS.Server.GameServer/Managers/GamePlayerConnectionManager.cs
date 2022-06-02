using DarkRift;
using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Configuration;
using RTS.Database;
using RTS.Models;
using RTS.Models.Server;
using RTS.Server.Messages;
using RTS.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTS.Server.GameServer
{
    internal static class GamePlayerConnectionManager
    {
        #region Implementation

        /// <summary>
        /// Try to identify the player
        /// </summary>
        /// <param name="pClient"></param>
        /// <param name="pDeviceMessage"></param>
        public static void TryIdentifyDevice(ThreadBase pThread, IClient pClient, CredentialMessage pPlayerCredentials)
        {
            try
            {
                //Log
                LoggingEvent log = new LoggingEvent(LogLevel.DEBUG, "Player " + pPlayerCredentials.playerId + " try to connect", null);
                DispatcherThread.Instance.EnqueueEvent(log);

                //Check player information
                PlayerModel player = PlayerFactory.GetById(
                    pThread.DBConnection.Connection,
                    pPlayerCredentials.playerId);

                ///////////////////////////////////
                /// Check for valid player
                if (player == null)
                {
                    GamePlayerCommunicationPlugin.Instance.SendDisconnectionMessage(pClient, DisconnectionErrorCode.INVALID_TOKEN                        );
                    throw new Exception("No player found for : " + pPlayerCredentials.playerId);
                }

                ///////////////////////////////////
                /// Check for valid session
                if (player.CurrentToken != pPlayerCredentials.token)
                {
                    GamePlayerCommunicationPlugin.Instance.SendDisconnectionMessage(pClient, DisconnectionErrorCode.INVALID_TOKEN);
                    throw new Exception("received token : " + pPlayerCredentials.token + " - Database token : " + player.CurrentToken);
                }

                /////////////////////////
                /// If player already exists                
                ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pPlayerCredentials.playerId);
                if (client != null)
                {
                    DispatcherThread.Instance.EnqueueEvent(
                    new LoggingEvent(LogLevel.WARNING, string.Format("Player is already connected, remove the last session for player id {0}.", pPlayerCredentials.playerId), null));

                    GamePlayerCommunicationPlugin.Instance.RemoveClientByPlayerId(pPlayerCredentials.playerId);
                }

                //Add the client with a simulation
                client = new ClientInformation
                {
                    ClientId = pClient.ID,
                    PlayerId = pPlayerCredentials.playerId,
                    Token = pPlayerCredentials.token,
                    DeviceId = player.DeviceId,
                    Simulation = new Simulation(
                        pThread.DBConnection.Connection,
                        player.id)
                };
                GamePlayerCommunicationPlugin.Instance.AddClient(client);


                client.Simulation.Simulate();
                client.Simulation.Persist(pThread.DBConnection.Connection);


                LoginResponseMessage loginResponseMessage = new LoginResponseMessage()
                {
                    loginErrorCode = (int)LoginErrorCode.NO_ERROR,
                };

                //Create and send message
                using (Message loginMessage = Message.Create(CommunicationTag.Connection.LOGIN_RESPONSE, loginResponseMessage))
                {
                    pClient.SendMessage(loginMessage, SendMode.Reliable);
                }


                log = new LoggingEvent(LogLevel.DEBUG, string.Format("Player {0} is now connected to the game server {1}", pPlayerCredentials.playerId, GameServerInitializationPlugin.Instance.InstanceInformation.ServerInstanceModel.Name), null);
                DispatcherThread.Instance.EnqueueEvent(log);

            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }

        public static void DisconnectPlayer(ThreadBase pThread, IClient pClient, ClientInformation pClientInformation)
        {
            try
            {
                //Log
                LoggingEvent log = new LoggingEvent(LogLevel.DEBUG, "Player " + pClientInformation.PlayerId + " disconnected", null);
                DispatcherThread.Instance.EnqueueEvent(log);

                if (pClientInformation != null)
                { 
                    //Simulate and persist
                    pClientInformation.Simulation.Simulate();
                    pClientInformation.Simulation.Persist(pThread.DBConnection.Connection);
                }

            }
            catch(Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }

        #endregion
    }
}
