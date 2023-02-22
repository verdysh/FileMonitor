using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines a class for reading from and writing to the backup_file table from the SQLite 
    /// database
    /// </summary>
    internal class BackupFile : BaseTable
    {
        private const string tableName = "backup_file";

        // Column names
        private const string pathColumn = "path";
        private const string idColumn = "id";

        // Column values
        private List<int>? iDs;
        private ObservableCollection<string>? paths;

        public ObservableCollection<string>? Paths { get { return paths; } }

        public BackupFile() 
        {
            List<object> pathValues = SQLSelectFromColumn(tableName, pathColumn);
            List<object> idValues = SQLSelectFromColumn(tableName, idColumn);

            // Cast from object list
            this.iDs = ToGenericList<int>(idValues);
            List<string> temp = ToGenericList<string>(pathValues);

            // Get observable collection
            paths = new ObservableCollection<string>(temp);
        }
    }
}
