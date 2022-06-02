using DarkRift;
using RTS.Configuration;
using RTS.Models;
using System;
using System.Globalization;

namespace RTS.Server.Messages
{
    public class PlayerMarketTradeMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Player market
        /// </summary>
        public PlayerMarketTradeModel PlayerTrade;

        /// <summary>
        /// This is usefull to control data from the client
        /// </summary>
        public int amountGivenControl, amountReceivedControl; 

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            PlayerTrade = new PlayerMarketTradeModel
            {
                playerId = e.Reader.ReadInt32(),
                resourceIdGiven = e.Reader.ReadInt32(),
                resourceIdReceived = e.Reader.ReadInt32(),
                quantity = e.Reader.ReadInt32(),
                amountReceivedForOneGiven = e.Reader.ReadInt32(),
                creation = DateTime.ParseExact(e.Reader.ReadString(), LocaleSettings.DATETIME_FORMAT, new CultureInfo(LocaleSettings.DATE_FORMAT_PROVIDER), DateTimeStyles.None)
            };

            amountGivenControl = e.Reader.ReadInt32();
            amountReceivedControl = e.Reader.ReadInt32();
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            e.Writer.Write(PlayerTrade.playerId);
            e.Writer.Write(PlayerTrade.resourceIdGiven);
            e.Writer.Write(PlayerTrade.resourceIdReceived);
            e.Writer.Write(PlayerTrade.quantity);
            e.Writer.Write(PlayerTrade.amountReceivedForOneGiven);
            e.Writer.Write(PlayerTrade.creation.ToString(LocaleSettings.DATETIME_FORMAT_KEYCODE, DateTimeFormatInfo.InvariantInfo));

            e.Writer.Write(PlayerTrade.AmountGiven); ;
            e.Writer.Write(PlayerTrade.AmountReceived);
        }
        #endregion
    }
}
