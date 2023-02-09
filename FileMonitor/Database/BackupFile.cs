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
            List<object> data = GetColumnValues(connection, _tableName, _pathColumn);
            List<string> paths = new List<string>();
            foreach (object entry in data)
            {
                paths.Add((string)entry); // Cast object to string
            }
            return paths;
        }
    }
}
