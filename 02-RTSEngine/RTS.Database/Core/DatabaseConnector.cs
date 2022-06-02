//using MySql.Data.MySqlClient;

//namespace RTS.Database
//{
//    public static class DatabaseConnector
//    {
//        #region Properties

//        private static string host = "remotemysql.com";
//        private static string database = "pAa2bIMH5M";
//        private static string user = "pAa2bIMH5M";
//        private static string password = "WElfPwBLrv";

//        #endregion

//        /// <summary>
//        /// Returns a new connection
//        /// </summary>
//        /// <param name="open"></param>
//        /// <param name="convertZeroDatetime"></param>
//        /// <param name="allowZeroDatetime"></param>
//        /// <returns></returns>
//        public static MySqlConnection GetNewConnection(bool open = true, bool convertZeroDatetime = false, bool allowZeroDatetime = false)
//        {
//            //Build connection string
//            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder()
//            {
//                Server = host,
//                Database = database,
//                UserID = user,
//                Password = password,
//                AllowZeroDateTime = allowZeroDatetime,
//                ConvertZeroDateTime = convertZeroDatetime
//            };

//            MySqlConnection connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
//            connection.Open();

//            return connection;
//        }

//        /// <summary>
//        /// Returns a new connection
//        /// </summary>
//        /// <param name="pConnectionString"></param>
//        /// <returns></returns>
//        public static MySqlConnection GetNewConnection(MySqlConnectionStringBuilder pConnectionString)
//        {
//            MySqlConnection connection = new MySqlConnection(pConnectionString.ConnectionString);
//            connection.Open();

//            return connection;
//        }

//    }
//}
