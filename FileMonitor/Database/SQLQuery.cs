using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    internal class SQLQuery
    {

        private string table = "foo";
        /// <summary>
        /// Query the table for a single ID column and retrieve the next available ID
        /// </summary>
        /// <param name="column"> Column to retrieve data from </param>
        /// <returns> An integer of the next available column ID </returns>
        private int GetNextAvailableID(SQLiteConnection connection, string column)
        {
            List<object> list = GetColumnValues(connection, column);

            if (list.Count == 0) return 0;
            else
            {
                object lastId = list[list.Count - 1];
                return 1 + (int)lastId; // cast to integer
            }
        }

        /// <summary>
        /// Get a list of monitored file paths from this object instance
        /// </summary>
        /// <returns> A list of file paths from the Table property of this instance </returns>
        public List<string>? GetPaths(SQLiteConnection connection)
        {
            List<object> data = GetColumnValues(connection, "path");
            List<string> paths = new List<string>();
            foreach (object entry in data)
            {
                paths.Add((string)entry); // Cast object to string
            }
            return paths;
        }

        /// <summary>
        /// Get a list of values from the specified column
        /// </summary>
        /// <param name="column"> Database table name to get data from </param>
        /// <returns> A list of objects from all entries within the table </returns>
        private List<object> GetColumnValues(SQLiteConnection connection, string column)
        {
            // start connection
            connection.Open();
            List<object> result = new List<object>();
            string query = $"SELECT {column} FROM {table}";

            // execute command
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) result.Add(reader[column]);
            connection.Close();
            return result;
        }
    }
}
