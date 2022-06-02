using MySql.Data.MySqlClient;
using RTS.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RTS.Database.Extraction
{
    public static class DataClassGenerator
    {

        #region Properties

        static string outputClassDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../RTS.Models/_GeneratedData"));

        #endregion

        #region Resources        

        /// <summary>
        /// Generate Resource data
        /// </summary>
        public static void GenerateResourceData(MySqlConnection pConnection)
        {
            ResourceDataTemplate resourceTemplate = new ResourceDataTemplate();
            List<ResourceModel> resources = ResourceFactory.GetAllResources(pConnection);

            resourceTemplate.Session = new Dictionary<string, object>();
            resourceTemplate.Session["Resources"] = resources;

            resourceTemplate.Initialize();
            resourceTemplate.ClearIndent();

            //Create or append file
            System.IO.File.WriteAllText($"{outputClassDirectory}/ResourceData.cs", resourceTemplate.TransformText());
        }

        #endregion

        #region Buildings

        /// <summary> 
        /// Generate Building State data
        /// </summary>
        public static void GenerateBuildingStateData(MySqlConnection pConnection)
        {
            BuildingStateTemplate buildingStateTemplate = new BuildingStateTemplate();
            List<BuildingStateModel> states = BuildingStateFactory.GetAllStates(pConnection);


            buildingStateTemplate.Session = new Dictionary<string, object>();
            buildingStateTemplate.Session["BuildingStates"] = states;

            buildingStateTemplate.Initialize();
            buildingStateTemplate.ClearIndent();

            //Create or append file
            System.IO.File.WriteAllText($"{outputClassDirectory}/BuildingStateData.cs", buildingStateTemplate.TransformText());
        }

        /// <summary>
        /// Generate Building data
        /// </summary>
        public static void GenerateBuildingsData(MySqlConnection pConnection)
        {
            BuildingTemplate buildingTemplate = new BuildingTemplate();

            List<BuildingModel> buildings = new List<BuildingModel>();

            List<BuildingCenterModel> buildingsCenter = BuildingCenterFactory.GetAllBuildings(pConnection);
            buildings.AddRange(buildingsCenter.Cast<BuildingModel>().ToList());

            List<BuildingProducerModel> buildingsProducer = BuildingProducerFactory.GetAllBuildings(pConnection);
            buildings.AddRange(buildingsProducer.Cast<BuildingModel>().ToList());

            List<BuildingPassiveModel> buildingsPassive = BuildingPassiveFactory.GetAllBuildings(pConnection);
            buildings.AddRange(buildingsPassive.Cast<BuildingModel>().ToList());

            buildingTemplate.Session = new Dictionary<string, object>();
            buildingTemplate.Session["Buildings"] = buildings;
            buildingTemplate.Session["BuildingsCenter"] = buildingsCenter;
            buildingTemplate.Session["BuildingsProducer"] = buildingsProducer;
            buildingTemplate.Session["BuildingsPassive"] = buildingsPassive;

            buildingTemplate.Initialize();
            buildingTemplate.ClearIndent();

            //Create or append file
            System.IO.File.WriteAllText($"{outputClassDirectory}/BuildingData.cs", buildingTemplate.TransformText());
        }

        #endregion

        #region Map

        /// <summary>
        /// Generate Map Element type data
        /// </summary>
        public static void GenerateMapElementTypeData(MySqlConnection pConnection)
        {
            MapElementTypeTemplate mapElementTypeTemplate = new MapElementTypeTemplate();
            List<MapElementTypeModel> mapElementTypes = MapElementFactory.GetAllMapElementTypes(pConnection);

            mapElementTypeTemplate.Session = new Dictionary<string, object>();
            mapElementTypeTemplate.Session["Types"] = mapElementTypes;

            mapElementTypeTemplate.Initialize();
            mapElementTypeTemplate.ClearIndent();

            //Create or append file
            System.IO.File.WriteAllText($"{outputClassDirectory}/MapElementTypeData.cs", mapElementTypeTemplate.TransformText());
        }

        /// <summary>
        /// Generate Map Element type data
        /// </summary>
        public static void GenerateMapElementData(MySqlConnection pConnection)
        {
            MapElementTemplate mapElementTemplate = new MapElementTemplate();
            List<MapElementModel> mapElements = MapElementFactory.GetAllMapElements(pConnection);

            mapElementTemplate.Session = new Dictionary<string, object>();
            mapElementTemplate.Session["Elements"] = mapElements;

            mapElementTemplate.Initialize();
            mapElementTemplate.ClearIndent();

            //Create or append file
            System.IO.File.WriteAllText($"{outputClassDirectory}/MapElementData.cs", mapElementTemplate.TransformText());
        }


        #endregion

        #region Market

        /// <summary>
        /// Generate Map Element type data
        /// </summary>
        public static void GenerateMarketData(MySqlConnection pConnection)
        {
            MarketTemplate marketTemplate = new MarketTemplate();
            List<MarketResourceRatioModel> mapElementTypes = MarketFactory.GetAllRatios(pConnection);
            MarketModel market = MarketFactory.GetMarket(pConnection);

            marketTemplate.Session = new Dictionary<string, object>();
            marketTemplate.Session["MarketRatios"] = mapElementTypes;
            marketTemplate.Session["Market"] = market;

            marketTemplate.Initialize();
            marketTemplate.ClearIndent();

            //Create or append file
            System.IO.File.WriteAllText($"{outputClassDirectory}/MarketData.cs", marketTemplate.TransformText());
        }

        #endregion
    }
}

