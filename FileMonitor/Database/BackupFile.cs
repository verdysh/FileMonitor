using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines a class for reading from and writing to the backup_file table from the SQLite 
    /// database
    /// </summary>
    internal class BackupFile : TableBase
    {
        private const string tableName = "backup_file";

        // Column names
        private const string pathColumn = "path";
        private const string idColumn = "id";

        // Column values
        private List<int>? iDs;
        private ObservableCollection<string>? paths;

        public List<int>? IDs { get { return iDs; } }
        public ObservableCollection<string>? Paths { get { return paths; } }

        public BackupFile() 
        {
            List<object> pathValues = GetColumnValuesAsObjects(tableName, pathColumn);
            List<object> idValues = GetColumnValuesAsObjects(tableName, idColumn);
            paths = CastToObservableCollection<string>(pathValues);
            iDs = CastToList<int>(idValues);
        }
    }
}
