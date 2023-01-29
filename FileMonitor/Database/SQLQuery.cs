using System.Collections.Generic;
using System.Data.SQLite;


namespace FileMonitor.Database
{
    internal class SQLQuery
    {
        private string path;

        /// <summary>
        /// Defines the class constructor
        /// </summary>
        /// <param name="path"> Path to database file </param>
        public SQLQuery(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Get IDs from database where only one column contains IDs
        /// </summary>
        /// <param name="table"> Table to retrieve IDs from </param>
        /// <returns> An integer list of all table IDs </returns>
        public List<string> GetSingleColumnIDs(string table)
        {
            List<string> result = new List<string>();
            string query = $"SELECT id FROM {table}";

            SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Version=3;");
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) result.Add(reader["id"].ToString());
            connection.Close();
            return result;
        }
        public void TestQuery()
        {
            //string query = $"SELECT id FROM {table}";

            //SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Version=3;");
            //connection.Open();

            //SQLiteCommand command = new SQLiteCommand(query, connection);
            //SQLiteDataReader reader = command.ExecuteReader();
            //Debug.WriteLine()
        }
    }
}
