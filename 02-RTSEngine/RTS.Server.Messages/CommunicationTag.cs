namespace RTS.Server.Messages
{
    /// <summary>
    /// Class wich identify a tag. Values can be between 0..65535
    /// </summary>
    public static class CommunicationTag
    {
        #region Connexion [0000x]

        public struct Connection
        {
            public const ushort LOGIN_PASSWORD_REQUEST = 00000;
            public const ushort DEVICE_IDENTIFICATION_REQUEST = 00001;
            public const ushort LOGIN_RESPONSE = 00002;
            public const ushort PLAYER_CREATION_INFORMATION_MESSAGE = 00003;
            public const ushort PLAYER_IDENTIFICATION_REQUEST = 00004;
            public const ushort GAME_SERVER_ADRESS_RESPONSE = 00005;
            public const ushort PLAYER_DISCONNECTION = 00009;            
        }
        
        #endregion

        #region PlayerMap [0001x]

        public struct PlayerMap
        {
            public const ushort PLAYER_MAP_REQUEST = 00010;
            public const ushort PLAYER_MAP_RESPONSE = 00011;

            public const ushort CREATE_MAP_EXTENT_REQUEST = 00012;
            public const ushort CREATE_MAP_EXTENT_RESPONSE = 00013;

        }


        #endregion

        #region PlayerConsumableResources [0002x] to [0003x]

        public struct PlayerConsumableResources
        {
            public const ushort ALL_RESOURCE_REQUEST = 00020;
            public const ushort ALL_RESOURCE_RESPONSE = 00021;
        }


        #endregion

        #region PlayerBuildings [0004x] to [0014x]

        public struct PlayerBuildings
        {
            public const ushort CREATE_PLAYER_BUILDING_REQUEST = 00040;
            public const ushort CREATE_PLAYER_RESOURCE_BUILDING_RESPONSE = 00041;

            public const ushort ALL_PLAYER_BUILDINGS_REQUEST = 00042;
            public const ushort ALL_PLAYER_BUILDINGS_CENTER_RESPONSE = 00043;
            public const ushort ALL_PLAYER_BUILDINGS_PRODUCER_RESPONSE = 00044;
            public const ushort ALL_PLAYER_BUILDINGS_PASSIVE_RESPONSE = 00045;

            public const ushort DESTROY_PLAYER_BUILDING_REQUEST = 00050;

            public const ushort INCREMENT_LEVEL_PLAYER_BUILDING_REQUEST = 00051;
            public const ushort UPDATE_LEVEL_PLAYER_BUILDING_RESPONSE = 00052;

            public const ushort ACTIVATE_PLAYER_BUILDING_REQUEST = 00053;
            public const ushort PAUSE_PLAYER_BUILDING_REQUEST = 00054;



        }

        #endregion
       
        #region Clock [0015x] to [0015x]
        public struct Clock
        {
            public const ushort HOUR_SYNC_REQUEST = 00150;
            public const ushort HOUR_SYNC_RESPONSE = 00151;
        }

        #endregion

        #region Player market [0016x] to [0017x]
        public struct PlayerMarket
        {
            public const ushort PLAYER_MARKET_REQUEST = 00060;
            public const ushort PLAYER_MARKET_TRADE_REQUEST = 00061;
            public const ushort PLAYER_MARKET_RESPONSE = 00062;
        }

        #endregion

        #region Player center [0018x]

        public struct PlayerCenter
        {
            public const ushort PLAYER_CENTER_REQUEST = 00180;
            public const ushort PLAYER_CENTER_RESPONSE = 00181;
        }

        #endregion

        #region PlayerCity [0019x]
        public struct PlayerCity
        {
            public const ushort PLAYER_CITY_REQUEST = 00190;
            public const ushort PLAYER_CITY_RESPONSE = 00191;
            public const ushort NEIGHBOORS_CITY_REQUEST = 00192;
            public const ushort NEIGHBOORS_CITY_RESPONSE = 00193;

        }
        #endregion

        #region Player [0020x)
        public struct Player
        {
            public const ushort ALL_PLAYERS_REQUEST = 00200;
            public const ushort ALL_PLAYERS_RESPONSE = 00201;

        }

        #endregion

        #region RESERVED Server communication [65xxx]

        #endregion


    }

}
