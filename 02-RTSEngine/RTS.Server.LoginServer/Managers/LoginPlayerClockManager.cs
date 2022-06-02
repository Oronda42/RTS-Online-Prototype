using DarkRift;
using DarkRift.Server;
using RTS.Configuration;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.LoginServer
{
    public static class LoginPlayerClockManager
    {
       
        /// <summary>
        /// Handle Hour Sync Request from client
        /// </summary>
        /// <param name="pThread"></param>
        /// <param name="pClient"></param>
        /// <param name="pMessage"></param>
        public static void HandleHourSyncRequest(ThreadBase pThread, IClient pClient, CredentialMessage pMessage)
        {
            try
            {
                HourSyncResponseMessage HourSyncData = new HourSyncResponseMessage
                {
                    serverTime = DateTime.Now.ToString(LocaleSettings.DATETIME_FORMAT_KEYCODE, DateTimeFormatInfo.InvariantInfo)
                };

                using (Message m = Message.Create(CommunicationTag.Clock.HOUR_SYNC_RESPONSE, HourSyncData))
                {
                    pClient.SendMessage(m, SendMode.Reliable);
                    Console.WriteLine("[RTS] INFO : Hour Sent to client : " + pClient.ID);
                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }
    }
}
