using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Class dedicated to creating the initial database
    /// </summary>
    internal class DatabaseBuilder
    {
        private string path;
        public DatabaseBuilder(string path) 
        {
            this.path = path;
        }

        public void Create(string[] createTables)
        {
            SQLiteConnection.CreateFile(path);
            SQLiteConnection SQLconnection = new SQLiteConnection("Data Source=FMDB.sqllite;Version=3");
            SQLconnection.Open();

            for (int i = 0; i < createTables.Length; i++)
            {
                SQLiteCommand command = new SQLiteCommand(createTables[i], SQLconnection);
                command.ExecuteNonQuery();
            }
        }
    }
}