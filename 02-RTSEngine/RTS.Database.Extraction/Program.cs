using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace RTS.Database.Extraction
{
    class Program
    {
        #region Properties

        static string outputDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../01-UnityProject/Assets/Data/RTS.Database.Extraction"));


        #endregion

        static void Main(string[] args)
        {

            Log("Press a key to start the extraction", TypeOfLog.INFO);
            Log("Files will be written to output directory : " + outputDirectory, TypeOfLog.IMPORTANT);
            Console.ReadKey();

            Log("---", TypeOfLog.INFO);
            Log("Data class generation started", TypeOfLog.INFO);





            //MySqlConnection connection = DatabaseConnector.GetNewConnection(); 
            MySqlConnection connection = new MySqlConnection();

            Log("Data class GenerateBuildingStateData generation started", TypeOfLog.INFO);
            DataClassGenerator.GenerateBuildingStateData(connection);
            Log("Data class GenerateBuildingsData generation started", TypeOfLog.INFO);
            DataClassGenerator.GenerateBuildingsData(connection);
            Log("Data class GenerateResourceData generation started", TypeOfLog.INFO);
            DataClassGenerator.GenerateResourceData(connection);
            Log("Data class GenerateMapElementTypeData generation started", TypeOfLog.INFO);
            DataClassGenerator.GenerateMapElementTypeData(connection);
            Log("Data class GenerateMapElementData generation started", TypeOfLog.INFO);
            DataClassGenerator.GenerateMapElementData(connection);
            Log("Data class GenerateMarketData generation started", TypeOfLog.INFO);
            DataClassGenerator.GenerateMarketData(connection);

            connection.Close();

            Log("Extraction finished successfully", TypeOfLog.SUCCESS);
            //Console.ReadKey();
        }

        #region Implementation

        enum TypeOfLog
        {
            INFO,
            IMPORTANT,
            SUCCESS,
            WARNING,
            ERROR
        }

        static void Log(string pMessage, TypeOfLog TypeOfLog)
        {
            ConsoleColor color = ConsoleColor.Gray;

            switch (TypeOfLog)
            {
                case TypeOfLog.WARNING:
                    color = ConsoleColor.DarkYellow;
                    break;
                case TypeOfLog.IMPORTANT:
                    color = ConsoleColor.White;
                    break;
                case TypeOfLog.SUCCESS:
                    color = ConsoleColor.Green;
                    break;
                case TypeOfLog.ERROR:
                    color = ConsoleColor.Red;
                    break;
            }

            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now + " - " + pMessage);

        }

        #endregion

    }
}