using System.Data;
using System.Data.SqlClient;

namespace OBS_Zutrittskontrollen_Simulation.Models
{
    public static class Database
    {
        // Get table from database
        public static DataTable GetTable(string SQLQuery)
        {
            SqlConnection connection = GetDatabaseConnection();

            // Get table
            SqlDataAdapter adapter = new(SQLQuery, connection);
            DataTable table = new();
            _ = adapter.Fill(table);
            CloseConnection();
            return table;
        }


        // Connect database and open it
        public static SqlConnection GetDatabaseConnection()
        {
            string connectionString = Properties.Settings1.Default.ConnectionString;
            SqlConnection connection = new(connectionString);

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }


        // Close Database
        public static void CloseConnection()
        {
            string connectionString = Properties.Settings1.Default.ConnectionString;
            SqlConnection connection = new(connectionString);

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }
    }
}
