using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines a class for reading from and writing to the source_file table from the SQLite 
    /// database
    /// </summary>
    internal class SourceFile : TableBase
    {
        private const string tableName = "source_file";

        // Column names
        private const string pathColumn = "path";
        private const string idColumn = "id";

        // Column values
        private List<int>? iDs;
        private List<string>? paths;

        public List<int> IDs { get => iDs; }
        public List<string> Paths 
        { 
            get => paths; 
            set 
            {
                foreach (var item in paths) Debug.WriteLine(item);
                Debug.WriteLine(value);
            }
        }

        public SourceFile() 
        {
            List<object> pathValues = GetColumnValuesAsObjects(tableName, pathColumn);
            List<object> idValues = GetColumnValuesAsObjects(tableName, idColumn);
            paths = CastObjectValues<string>(pathValues);
            iDs = CastObjectValues<int>(idValues);
        }
    }
}
