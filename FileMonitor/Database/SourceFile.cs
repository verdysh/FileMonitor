using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Markup;

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
        private const string filePathColumn = "path";
        private const string idColumn = "id";

        // Column values
        private List<int>? iDs;
        private ObservableCollection<string>? filePaths;
        private ReadOnlyObservableCollection<string>? readOnlyFilePaths;

        public INotifyCollectionChanged FilePaths { get => readOnlyFilePaths; }

        public SourceFile() 
        {
            // Query database
            List<object> pathValues = SQLSelectFromColumn(tableName, filePathColumn);
            List<object> idValues = SQLSelectFromColumn(tableName, idColumn);
            
            // Cast from object list
            this.iDs = ToGenericList<int>(idValues);
            List<string> temp = ToGenericList<string>(pathValues);

            // Convert list to ObservableCollection
            filePaths = new ObservableCollection<string>(temp);
            readOnlyFilePaths = new ReadOnlyObservableCollection<string>(filePaths);
        }

        /// <summary>
        /// Adds a new file path to the database
        /// </summary>
        /// <remarks> Updates FilePaths property </remarks>
        public void AddFile(string path)
        {
            int id = GetNextAvailableID(iDs);
            string insertStatement = $"INSERT INTO source_file (id, path) values ({id}, \'{path}\')";
            if (!filePaths.Contains(path))
            {
                SQLInsertInto(tableName, idColumn, filePathColumn, id, path);
                iDs.Add(id);
                filePaths.Add(path);
            }
        }
    }
}
