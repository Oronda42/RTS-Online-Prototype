using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Database
{
    internal static class Constants
    {
        public struct TableName
        {
            ////////////////////////////////////
            // Buildings
            public static string BUILDING = "building";
            
            public static string BUILDING_CENTER = "building_center";
            public static string BUILDING_CENTER_LEVEL = "building_center_level";

            public static string BUILDING_PASSIVE = "building_passive";
            public static string BUILDING_PASSIVE_LEVEL = "building_passive_level";

            public static string BUILDING_PRODUCER = "building_producer";
            public static string BUILDING_PRODUCER_LEVEL = "building_producer_level";
            
            public static string BUILDING_STATE = "building_state";
            public static string BUILDING_LEVEL_COST = "building_level_cost";            

            ////////////////////////////////////
            // Resources
            public const string RESOURCE = "resource";
            public const string RESOURCE_TYPE = "resource_type";            
            public const string RESOURCE_BAG = "resource_bag";

            ////////////////////////////////////
            // Player
            public const string PLAYER = "player";
            public const string PLAYER_SESSIONS = "player_sessions";
            public const string PLAYER_MAP = "player_map";
            public const string PLAYER_MAP_EXTENT = "player_map_extent";
            public const string PLAYER_MAP_EXTENT_ELEMENT = "player_map_extent_element";
            public const string PLAYER_RESOURCE_BAG = "player_resource_bag";
            public const string PLAYER_RESOURCE_BAG_DEFAULT = "player_resource_bag_default";

            ////////////////////////////////////
            // Player Market
            public const string PLAYER_MARKET_TRADES = "player_market_trades";
            public const string PLAYER_MARKET = "player_market";

            ////////////////////////////////////
            // Player buildings
            public const string PLAYER_BUILDING = "player_building";
            public const string PLAYER_BUILDING_CENTER = "player_building_center";
            public const string PLAYER_BUILDING_PRODUCER = "player_building_producer";
            public const string PLAYER_BUILDING_PASSIVE = "player_building_passive";

            ////////////////////////////////////
            // Player City
            public const string PLAYER_CITY = "player_city";
            public const string CITY_LEVEL = "city_level";

            ////////////////////////////////////
            // Map
            public const string MAP_ELEMENT_TYPE = "map_element_type";
            public const string MAP_ELEMENT = "map_element";
            public const string MAP_EXTENT = "map_extent";
            public const string MAP_EXTENT_ELEMENT = "map_extent_element";
            
            ////////////////////////////////////
            // Market
            public const string MARKET_RESOURCE_RATIO = "market_resource_ratio";
            public const string MARKET_LEVEL = "market_level";

            ////////////////////////////////////
            // Server
            public const string SERVER_INSTANCE = "server_instance";
            public const string GAME_SERVER_ZONE = "game_server_zone";

        }
    }
}
