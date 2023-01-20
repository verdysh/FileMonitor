using System.Data.SQLite;
using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class DatabaseBuilder
    {
        private string path;
        public DatabaseBuilder(string path) 
        {
            this.path = path;
        }
        SQLCommands s = new SQLCommands();

        public void Create()
        {
            SQLiteConnection.CreateFile(path);
            SQLiteConnection SQLconnection = new SQLiteConnection("Data Source=FMDB.sqllite;Version=3");
            SQLconnection.Open();

            foreach (KeyValuePair<string, string> entry in s.TablesColumnsCreate)
            {
                string sql = $"{s.create} {entry.Key} {entry.Value}";
                SQLiteCommand command = new SQLiteCommand(sql, SQLconnection);
                command.ExecuteNonQuery();
            }
        }
    }
}