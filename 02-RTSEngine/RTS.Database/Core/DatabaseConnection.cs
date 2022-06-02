using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Database
{
    public class DatabaseConnection
    {
        #region Properties

        /// <summary>
        /// Represents the player connexion to the SQL server
        /// </summary>
        public MySqlConnection Connection
        {
            private set { connection = value; }
            get
            {
                if (connection == null)
                    Connect();

                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Connect();
                }

                return connection;
            }
        }
        private MySqlConnection connection;

        /// <summary>
        /// Connection string
        /// </summary>
        public MySqlConnectionStringBuilder ConnectionString { get; set; }

        #endregion

        #region Implementation

        public DatabaseConnection(MySqlConnectionStringBuilder pConnectionString)
        {
            ConnectionString = pConnectionString;
        }

        /// <summary>
        /// Returns a new connection
        /// </summary>
        /// <param name="pConnectionString"></param>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                connection = new MySqlConnection(ConnectionString.ConnectionString);
                connection.Open();

                Console.WriteLine("New DB connection created");

                return true;

            }catch(MySqlException e)
            {
                throw e;
            }
        }

        #endregion




    }
}
