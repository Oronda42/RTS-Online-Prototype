using DarkRift;
using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Database;
using RTS.Models;
using RTS.Models.Server;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTS.Server.LoginServer
{
    internal static class LoginPlayerConnectionManager
    {
        #region Implementation

        /// <summary>
        /// Try to identify the player
        /// </summary>
        /// <param name="pClient"></param>
        /// <param name="pDeviceMessage"></param>
        public static void TryIdentifyDevice(ThreadBase pThread, IClient pClient, LoginDeviceRequestMessage pDeviceMessage)
        {
            //Response to send
            LoginResponseMessage loginResponse = new LoginResponseMessage();

            //Check if player exists in database
            PlayerModel player = PlayerFactory.GetWithDeviceId(
                pThread.DBConnection.Connection, 
                pDeviceMessage.deviceId);
            
            if (player == null)
            {
                /////////////////////////
                /// If player doesn't exists
                LoggingEvent log = new LoggingEvent(LogLevel.INFO, string.Format("Player with device not found {0}. The player will be created", pDeviceMessage.deviceId), null);
                DispatcherThread.Instance.EnqueueEvent(log);

                loginResponse.loginErrorCode = (int)Configuration.LoginErrorCode.USER_DONT_EXIST;
            }
            else
            {
                /////////////////////////
                /// If player already exists                
                ClientInformation client = LoginPlayerCommunicationPlugin.Instance.GetClientByPlayerId(player.id);
                if (client != null)
                {
                    DispatcherThread.Instance.EnqueueEvent(
                        new LoggingEvent(LogLevel.WARNING, string.Format("Player is already connected, remove the last session for player id {0}.", player.id), null));

                    LoginPlayerCommunicationPlugin.Instance.RemoveClientByPlayerId(player.id);
                }

                //Add the client
                client = new ClientInformation
                {
                    ClientId = pClient.ID,
                    PlayerId = player.id,
                    Token = TokenService.GenerateNewToken(),
                    DeviceId = pDeviceMessage.deviceId,
                    Simulation = null
                };

                //Update token from client to player
                player.CurrentToken = client.Token;

                LoginPlayerCommunicationPlugin.Instance.AddClient(client);

                //Delegate the action to the database thread
                DatabaseEvent dbEvent = new DatabaseEvent(1, GameThread.Instance,
                    new Action<MySqlConnection, PlayerModel>(PlayerFactory.UpdatePlayerInformations),
                    new object[] { pThread.DBConnection.Connection, player });
                
                //Set the same event on error in order to retry the insertion
                dbEvent.ErrorEvent = dbEvent;

                DispatcherThread.Instance.EnqueueEvent(dbEvent);

                PlayerSessionModel playerSession = new PlayerSessionModel()
                {
                    PlayerId = player.id,
                    Start = DateTime.Now,
                    Token = player.CurrentToken
                };

                client.PlayerSession = playerSession;

                //Delegate the action to the database thread
                dbEvent = new DatabaseEvent(1, GameThread.Instance,
                    new Action<MySqlConnection, PlayerSessionModel>(PlayerSessionFactory.Create),
                    new object[] { pThread.DBConnection.Connection, playerSession});

                //Set the same event on error in order to retry the insertion
                dbEvent.ErrorEvent = dbEvent;

                DispatcherThread.Instance.EnqueueEvent(dbEvent);

                ServerInstanceModel worldServerInformation = LoginInterServerCommunicationPlugin.Instance.GetServerByName(Constants.ServerName.WORLD_SERVER).ServerInstanceModel;

                //update login response message
                loginResponse.loginErrorCode = (int)Configuration.LoginErrorCode.NO_ERROR;
                loginResponse.nickname = player.nickname;
                loginResponse.token = client.Token;
                loginResponse.id = player.id;
                loginResponse.RedirectedServerHost = worldServerInformation.Host;
                loginResponse.RedirectedServerPort = worldServerInformation.Port;

                LoggingEvent log = new LoggingEvent(LogLevel.INFO, string.Format("Player {0} will be redirected to the world server", player.id), null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }

            //Create and send message
            using (Message loginMessage = Message.Create(CommunicationTag.Connection.LOGIN_RESPONSE, loginResponse))
            {
                pClient.SendMessage(loginMessage, SendMode.Reliable);
            }
        }

        /// <summary>
        /// Try to create the player
        /// </summary>
        public static void TryCreatePlayer(ThreadBase pThread, IClient pClient, PlayerCreationInformationMessage pPlayerInformation)
        {
            //Response to send
            LoginResponseMessage loginResponse = new LoginResponseMessage();

            //Create the player
            PlayerModel player = PlayerFactory.CreateWithDeviceId(
                pThread.DBConnection.Connection, 
                pPlayerInformation.deviceId);

            //Update player's informations
            player.nickname = pPlayerInformation.nickname;

            LoggingEvent log = new LoggingEvent(LogLevel.INFO, string.Format("The player {0} has been created with device id {1} as player ID {2}", pPlayerInformation.nickname, pPlayerInformation.deviceId, player.id), null);
            DispatcherThread.Instance.EnqueueEvent(log);

            //Create the client
            ClientInformation client = new ClientInformation
            {
                ClientId = pClient.ID,
                PlayerId = player.id,
                Token = TokenService.GenerateNewToken(),
                DeviceId = pPlayerInformation.deviceId,
                Simulation = null
            };
            LoginPlayerCommunicationPlugin.Instance.AddClient(client);

            //Update Player's Token
            player.CurrentToken = client.Token;

            ServerInstanceModel worldServerInformation = LoginInterServerCommunicationPlugin.Instance.GetServerByName(Constants.ServerName.WORLD_SERVER).ServerInstanceModel;
            //Create serializable object
            loginResponse.loginErrorCode = (int)Configuration.LoginErrorCode.NO_ERROR;
            loginResponse.nickname = player.nickname;
            loginResponse.token = client.Token;
            loginResponse.id = player.id;
            loginResponse.RedirectedServerHost = worldServerInformation.Host;
            loginResponse.RedirectedServerPort = worldServerInformation.Port;


            //Delegate the action to the database thread
            DatabaseEvent dbEvent = new DatabaseEvent(1, GameThread.Instance,
                new Action<MySqlConnection, PlayerModel>(PlayerFactory.UpdatePlayerInformations),
                new object[] { pThread.DBConnection.Connection, player });

            //Set the same event on error in order to retry the insertion
            dbEvent.ErrorEvent = dbEvent;

            DispatcherThread.Instance.EnqueueEvent(dbEvent);

            PlayerSessionModel playerSession = new PlayerSessionModel()
            {
                PlayerId = player.id,
                Start = DateTime.Now,
                Token = player.CurrentToken
            };

            client.PlayerSession = playerSession;

            //Delegate the action to the database thread
            dbEvent = new DatabaseEvent(1, GameThread.Instance,
                new Action<MySqlConnection, PlayerSessionModel>(PlayerSessionFactory.Create),
                new object[] { pThread.DBConnection.Connection, playerSession });

            //Set the same event on error in order to retry the insertion
            dbEvent.ErrorEvent = dbEvent;

            DispatcherThread.Instance.EnqueueEvent(dbEvent);

            //Create and send message
            using (Message loginMessage = Message.Create(CommunicationTag.Connection.LOGIN_RESPONSE, loginResponse))
            {
                pClient.SendMessage(loginMessage, SendMode.Reliable);
            }
        }

        #endregion
    }
}
