using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        public event PropertyChangedEventHandler? PropertyChanged;
        public INotifyCollectionChanged? FilePaths { get => AsReadOnlyObservableCollection(columns.Values); }

        public SourceFile() 
        {
            // Query database
            columns = SQLSelectFromColumn(tableName, idColumn, pathColumn);
        }

        protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                OnPropertyChanged("FilePaths");
            }
        }

        /// <summary>
        /// Removes a file from the database column
        /// </summary>
        /// <remarks> Updates FilePaths property </remarks>
        public void RemoveFile(string path)
        {
            SQLDeleteFrom(tableName, pathColumn, path);
            //columns.Remove(path);
        }

        /// <summary>
        /// Convert a dictionary value collection to ReadOnlyObservableCollection 
        /// </summary>
        private ReadOnlyObservableCollection<string> AsReadOnlyObservableCollection(Dictionary<int, string>.ValueCollection collection)
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            Dictionary<int, string>.ValueCollection.Enumerator enumerator = collection.GetEnumerator();
            while(enumerator.Current != null)
            {
                result.Add(enumerator.Current.ToString());
                enumerator.MoveNext();
            }
            return new ReadOnlyObservableCollection<string>(result);
        }
    }
}
