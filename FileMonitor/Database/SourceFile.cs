﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines a class for reading from and writing to the source_file table from the SQLite 
    /// database
    /// </summary>
    internal class SourceFile : BaseTable
    {
        private const string tableName = "source_file";
        private const string idColumn = "id";
        private const string pathColumn = "path";
        private Dictionary<int, string> columns;
        private ObservableCollection<string> files;
        private ReadOnlyObservableCollection<string> readOnlyFiles;

        public INotifyCollectionChanged? FilePaths { get => readOnlyFiles; }

        public SourceFile() 
        {
            columns = SQLSelectFrom(tableName, idColumn, pathColumn);
            files = new ObservableCollection<string>(columns.Values);
            readOnlyFiles = new ReadOnlyObservableCollection<string>(files);
        }

        /// <summary>
        /// Adds a new file path to the database
        /// </summary>
        /// <remarks> Updates FilePaths property </remarks>
        public void AddFile(string path)
        {
            if (!columns.ContainsValue(path))
            {
                int id = GetNextAvailableID(columns);
                SQLInsertInto(tableName, idColumn, pathColumn, id, path);
                columns.Add(id, path);
                files.Add(path);
            }
        }

        /// <summary>
        /// Removes a file from the database column
        /// </summary>
        /// <remarks> Updates FilePaths property </remarks>
        public void RemoveFile(string path)
        {
            int id;
            foreach(int key in columns.Keys) 
            { 
                if (columns[key] == path) 
                {
                    id = key;
                    SQLDeleteFrom(tableName, idColumn, id);
                    columns.Remove(id); 
                    files.Remove(path); 
                    return; 
                } 
            }
        }
    }
}
