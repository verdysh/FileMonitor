using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines a class for reading from and writing to the backup_file table from the SQLite 
    /// database
    /// </summary>
    internal class BackupFile : SQLQuery
    {
        private const string tableName = "backup_file";

        // Column names
        private const string pathColumn = "path";
        private const string idColumn = "id";

        // Column values
        private List<int> iDs;
        private List<string> paths;

        public List<int> IDs { get { return iDs; } }
        public List<string> Paths { get { return paths; } }

        public BackupFile(SQLiteConnection connection) 
        {
            paths = GetPaths(connection);
        }

        /// <summary>
        /// A method to access all file paths stored in the backup_file table
        /// </summary>
        /// <returns> A string list containing all file paths </returns>
        private List<string>? GetPaths(SQLiteConnection connection)
        {
            List<object> data = GetColumnValues(connection, tableName, pathColumn);
            List<string> paths = new List<string>();
            foreach (object entry in data)
            {
                paths.Add((string)entry); // Cast object to string
            }
            return paths;
        }
    }
}
