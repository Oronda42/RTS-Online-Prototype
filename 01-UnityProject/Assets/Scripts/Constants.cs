public static class Constants
{

    public struct Tags
    {
        public struct Map
        {
            public const string MAP_ELEMENT = "MapElement";
        }

    }

    public struct Scenes
    {
        public struct Core
        {
            public const string APPLICATION_MANAGER = "C01-ApplicationManager";
            public const string LOADING_SCENE = "C02-LoadingScene";

        }
        public struct Menu
        {
            public const string LOGIN = "M01-Login";
        }

        public struct PlayerMap
        {
            public const string FIRST_CONNECTION = "G00-FirstConnection";
            public const string PLAYERMAP = "G01-PlayerMap";
        }

        public struct PlayerWorld
        {
            public const string PLAYER_WORLD = "G02-PlayerWorld";
        }

    }

    public struct Bundles
    {
        public const string BUILDING_PRODUCER = "building_producer";
        public const string BUILDING_PASSIVE = "building_passive";
        public const string BUILDING_CENTER = "building_center";
        public const string BUILDING_STATE = "building_state";
        public const string RESOURCES = "resources";
        public const string UI = "ui";
        public const string CITIES = "cities";
    }

    public struct Input
    {
        public const int HORIZONTAL_LIMIT = 2 * MapExtent.MAP_SIZE;
        public const int VERTICAL_LIMIT = HORIZONTAL_LIMIT;
        public const float ZOOM_IN_LIMIT = 1.5f;
        public const float ZOOM_OUT_LIMIT = 3;

    }

    public struct MapExtent
    {
        public const int MAP_SIZE = 10;
    }

    public struct Icons
    {
        public struct Interation
        {
            public const string DESTROY = "IconDestroy.png";
            public const string VIEW = "IconView.png";
            public const string LEVEL_UP = "IconLevelUp.png";
            public const string CONSTRUCT_BUILDING = "IconConstructBuilding.png";
            public const string ACTIVATE_BUILDING = "IconActivateBuilding.png";

            public struct Building
            {
                public const string CONSTRUCT_BUILDING_PRODUCER = "IconConstructBuilding.png";
            }

        }
    }

    public struct UI
    {
        //TODO -> mettre à jour les structs

        public const string UI_PLAYERMAP = "UI";

        public struct Components
        {
            public const string ICON_INFORMATION = "IconInformation";
            public const string LIST_CONTAINER = "ListContainer";
            public const string TEXT_INFORMATION = "TextInformation";
            public const string GO_UP_TRIGGER = "GoUp";
            public const string STAY_TRIGGER = "Stay";
            public const string GO_TO_LOCATION_1_TRIGGER = "GoToLocation1";
            public const string GO_TO_LOCATION_2_TRIGGER = "GoToLocation2";
            public const string GO_TO_LOCATION_3_TRIGGER = "GoToLocation3";
            public const string FADE_OUT_AND_DESTROY_TRIGGER = "FadeOutAndDestroy";

            public const string INDICATOR_PREFAB = "Indicator";
        }

        public struct Resources
        {
            public const string DEFAULT_RESOURCE_SLOT = "DefaultResourceSlot";
        }

        public struct Panels
        {
            public const string CONSTRUCTION_MENU_PANEL = "ConstructionMenuPanel";
            public const string BUILDING_PRODUCER_INFO = "BuildingProducerInfo";
            public const string BUILDING_PASSIVE_INFO = "BuildingPassiveInfo";
            public const string BUILDING_MENU_INFO = "BuildingMenuInfoPanel";
            public const string CONSTRUCTION_BUILDING_MENU_SLOT = "ConstructionBuildingSlot";
            public const string COMMAND_CENTER = "CommandCenterPanel";
            public const string PLAYER_MARKET_PANEL = "PlayerMarketPanel";
            public const string PLAYER_BUILDING_INFO_PANEL = "PlayerBuildingInfoPanel";
            public const string PLAYER_NAME_FORM_PANEL = "PlayerNameFormPanel";

        }
    }

    public struct Cities
    {
        public const string PLAYER_CITY = "PlayerCity";
    }
}
