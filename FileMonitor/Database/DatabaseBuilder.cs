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
            string[] SQLCommands = new string[]
            {
                "CREATE TABLE source_file (id INT, path VARCHAR(260))",
                "CREATE TABLE backup_file (id INT, path VARCHAR(260))",
                "CREATE TABLE source_backup_relational (source_file_id INT, backup_file_id INT)",
                "CREATE TABLE source_hash (id INT, hashcode VARCHAR(160))",
                "CREATE TABLE backup_hash (id INT, hashcode VARCHAR(160))",
                "CREATE TABLE hash_relational (source_hash_id INT, backup_hash_id INT)"
            };
            SQLiteConnection.CreateFile(path);
            SQLiteConnection SQLconnection = new SQLiteConnection("Data Source=FMDB.sqllite;Version=3");
            SQLconnection.Open();
        }
    }
}
