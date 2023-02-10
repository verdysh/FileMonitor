using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines a class for reading from and writing to the source_file table from the SQLite 
    /// database
    /// </summary>
    internal class SourceFile : SQLQuery
    {
        private const string tableName = "source_file";

        // Column names
        private const string pathColumn = "path";
        private const string idColumn = "id";

        // Column values
        private List<int> iDs;
        private List<string> paths;

        public SourceFile()
        {
            paths = GetPaths();
        }

        /// <summary>
        /// A method to access all files paths stored in the source_file table
        /// </summary>
        /// <returns> A string list containing all file paths </returns>
        public List<string>? GetPaths()
        {
            List<object> data = GetColumnValues(tableName, pathColumn);
            List<string> paths = new List<string>();
            foreach (object entry in data)
            {
                paths.Add((string)entry); // Cast object to string
            }
            return paths;
        }
    }
}
