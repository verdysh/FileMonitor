using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    internal class SQLQuery
    {
        private readonly string _path;
        private readonly string _table;
        public string Table { get => _table; } // return this.table

        /// <summary>
        /// Defines the class constructor
        /// </summary>
        /// <param name="path"> Path to the program database file </param>
        /// <param name="table"> Table to query from this object instance </param>
        public SQLQuery(string path, string table)
        {
            this._path = path;
            this._table = table;
        }

        /// <summary>
        /// Query the table for a single ID column and retrieve the next available ID
        /// </summary>
        /// <param name="column"> Column to retrieve data from </param>
        /// <returns> An integer of the next available column ID </returns>
        public int GetNextAvailableID(string column)
        {
            List<object> list = GetColumnValues(column);

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
        public List<string>? GetPaths()
        {
            List<object> data = GetColumnValues("path");
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
        private List<object> GetColumnValues(string column)
        {
            List<object> result = new List<object>();
            string query = $"SELECT {column} FROM {_table}";

            // start connection
            SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;");
            connection.Open();

            // execute command
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) result.Add(reader[column]);
            connection.Close();
            return result;
        }
    }
}
