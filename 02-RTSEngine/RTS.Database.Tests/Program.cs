using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RTS.Database.Tests
{
    class Program
    {
        //static string ToPascalCase(string text)
        //{
        //    return CultureInfo.InvariantCulture.TextInfo
        //        .ToTitleCase(text.ToLowerInvariant())
        //        .Replace("-", "")
        //        .Replace("_", "")
        //        .Replace(" ", "");
        //}

        static void Main(string[] args)
        {

            Console.WriteLine("Press key to continue ..."); 
            //Console.WriteLine(ToPascalCase("Press key to continue ..."));
            Console.ReadKey();

            //MySqlConnection connection = DatabaseConnector.GetNewConnection();



            //List<MarketResourceRatioModel> ratios = MarketFactory.GetAllRatios();
            //for (int i = 0; i < ratios.Count; i++)
            //{
            //    Console.WriteLine(ratios[i].resourceIdA + ", " + ratios[i].resourceIdB + ", " + ratios[i].amount);
            //}

            //PlayerModel player = PlayerFactory.GetById(32);
            //PlayerMapModel b = PlayerMapFactory.GetByPlayer(player);

            // MarketModel m = MarketFactory.GetMarket(connection);

            //MapExtentModel me =  MapExtentFactory.GetById(connection, 1);
            //me.Elements = MapExtentFactory.GetElements(connection, me);
            //PlayerBuildingPassiveModel pb = new PlayerBuildingPassiveModel();
            //pb.Building = new BuildingPassiveModel();
            //pb.Building.constructionTime = 60;

            //PlayerBuildingModel pbm = pb;

            //Console.WriteLine("test : " + pb.Building.constructionTime);
            //Console.WriteLine("test : " + m.Levels.Count);

            // PlayerMarketFactory.CreateTrade(15, 1, 3, 15);


            InventoryModel inventory = new InventoryModel();
            inventory.Init(10);

            Weapon blackWeapon = new Weapon { InventorySettings = new InventoryItemSettingsModel(0, 5, 1, new InventoryItemTypeModels {id =1, name="Weapon"}) };
            Weapon redWeapon = new Weapon { InventorySettings = new InventoryItemSettingsModel(0, 5, 2, new InventoryItemTypeModels {id = 1, name = "Weapon"}) };

            Module module = new Module { InventorySettings = new InventoryItemSettingsModel(0, 1, 0, new InventoryItemTypeModels { id = 2, name = "Module" }) };

            int w1 = inventory.AddItem(blackWeapon,4);
            int w2 = inventory.AddItem(redWeapon, 4);

            int b = inventory.AddItem(module,2);

            // bool c  = inventory.RemoveObject(weapon, 6);
           var w =  inventory.GetObjectByPosition<Weapon>(0);

            Console.WriteLine("Press key to continue ...");
            Console.ReadKey();

        }
    }
}
