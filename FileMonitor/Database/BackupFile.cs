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
        private const string _tableName = "backup_file";

        // Column names
        private const string _pathColumn = "path";
        private const string _idColumn = "id";

        // Private collection of column values
        private List<int> _iDs;
        private List<string> _paths;

        /// <summary>
        /// Defines the class constructor
        /// </summary>
        /// <param name="connection"> A SQLiteConnection object </param>
        public BackupFile(SQLiteConnection connection) 
        {
            _paths = GetPaths(connection);
        }

        /// <summary>
        /// A method to access all files stored in the backup_file table
        /// </summary>
        /// <param name="connection"> A SQLiteConnection object to access the database </param>
        /// <returns> A string list containing all file paths from the backup_file table </returns>
        public List<string>? GetPaths(SQLiteConnection connection)
        {
            List<object> data = GetColumnValues(connection, _pathColumn);
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
            string query = $"SELECT {column} FROM {_tableName}";

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
            List<object> list = GetColumnValues(connection, _idColumn);
            if (list.Count == 0) return 0;
            else
            {
                object lastId = list[list.Count - 1];
                return 1 + (int)lastId; // cast to integer
            }
        }
    }
}
