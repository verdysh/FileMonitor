using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines a class for reading from and writing to the backup_file table from the SQLite 
    /// database
    /// </summary>
    internal class BackupFile
    {
        // Table name
        private const string _table = "backup_file";

        // Column names
        private const string _path = "path";
        private const string _id = "id";

        // Private collection of column values
        private List<int> _iDs;
        private List<string> _paths;

        /// <summary>
        /// A property to read from or write to the 
        /// </summary>
        public List<int> IDs { get => _iDs; }
        public List<string> Paths { get => _paths; }

        public BackupFile(SQLiteConnection connection) 
        {
            _paths = GetPaths(connection);
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

        // Get a list of object values from the specified column
        private List<object> GetColumnValues(SQLiteConnection connection, string column)
        {
            // start connection
            connection.Open();
            List<object> result = new List<object>();
            string query = $"SELECT {column} FROM {_table}";

            // execute command
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) result.Add(reader[column]);
            connection.Close();
            return result;
        }

        // Query the table for a single ID column and retrieve the next available ID
        private int GetNextAvailableID(SQLiteConnection connection)
        {
            List<object> list = GetColumnValues(connection, _id);
            if (list.Count == 0) return 0;
            else
            {
                object lastId = list[list.Count - 1];
                return 1 + (int)lastId; // cast to integer
            }
        }
    }
}
