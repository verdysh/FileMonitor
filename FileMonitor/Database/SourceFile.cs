using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    internal class SourceFile : SQLQuery
    {
        // Table name
        private const string tableName = "source_file";

        // Column names
        private const string pathColumn = "path";
        private const string idColumn = "id";

        // Private collection of column values
        private List<int> iDs;
        private List<string> paths;

        /// <summary>
        /// Defines the class constructor
        /// </summary>
        /// <param name="connection"> A SQLiteConnection object </param>
        public SourceFile(SQLiteConnection connection)
        {
            paths = GetPaths(connection);
        }

        /// <summary>
        /// A method to access all files stored in the source_file table
        /// </summary>
        /// <param name="connection"> A SQLiteConnection object to access the database </param>
        /// <returns> A string list containing all file paths from the source_file table </returns>
        public List<string>? GetPaths(SQLiteConnection connection)
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
