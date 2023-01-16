using System.Data.SQLite;

namespace FileMonitor.Database
{
    internal class DatabaseBuilder
    {
        private string path;
        public DatabaseBuilder(string path) 
        {
            this.path = path;
        }

        public void Create()
        {
            SQLiteConnection.CreateFile(path);
            SQLiteConnection SQLconnection = new SQLiteConnection("Data Source=FMDB.sqllite;Version=3");
            SQLconnection.Open();
        }
    }
}
