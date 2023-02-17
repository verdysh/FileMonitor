using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines a class for reading from and writing to the source_file table from the SQLite 
    /// database
    /// </summary>
    internal class SourceFile : BaseTable
    {
        private const string tableName = "source_file";

        // Column names
        private const string pathColumn = "path";
        private const string idColumn = "id";

        // Column values
        private List<int>? iDs;
        private ObservableCollection<string>? paths;

        public ObservableCollection<string> Paths 
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
            // Query database
            List<object> pathValues = SQLSelectFromColumn(tableName, pathColumn);
            List<object> idValues = SQLSelectFromColumn(tableName, idColumn);
            
            // Cast from object list
            this.iDs = CastListFromObject<int>(idValues);
            List<string> temp = CastListFromObject<string>(pathValues);

            // Get observable collection
            paths = ConvertToObservableCollection<string>(temp);
        }
    }
}
