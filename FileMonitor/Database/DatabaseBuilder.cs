using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Class dedicated to creating the initial database
    /// </summary>
    internal class DatabaseBuilder : BaseTable
    {
        /// <summary>
        /// An array containing all the commands for creating the database tables
        /// </summary>
        private readonly string[] commands = new string[]
        {
            "CREATE TABLE source_file (id INTEGER NOT NULL UNIQUE, path VARCHAR(260), PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE backup_file (id INTEGER NOT NULL UNIQUE, path VARCHAR(260), PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE source_backup_file_rel (id INTEGER NOT NULL UNIQUE, source_file_id INTEGER, backup_file_id INTEGER, PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE source_hash (id INTEGER NOT NULL UNIQUE, hash_code VARCHAR(160), PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE backup_hash (id INTEGER NOT NULL UNIQUE, hash_code VARCHAR(160), PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE source_backup_hash_rel (id INTEGER NOT NULL UNIQUE, source_hash_id INTEGER, backup_hash_id INTEGER, PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE source_file_hash_rel (id INTEGER NOT NULL UNIQUE, source_file_id INTEGER, source_hash_id INTEGER, PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE backup_file_hash_rel (id INTEGER NOT NULL UNIQUE, backup_file_id INTEGER, backup_hash_id INTEGER, PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE full_backup_location (id INTEGER NOT NULL UNIQUE, path VARCHAR(260), PRIMARY KEY(id AUTOINCREMENT))",
            "CREATE TABLE consecutive_backup_location (id INTEGER NOT NULL UNIQUE, path VARCHAR(260), PRIMARY KEY(id AUTOINCREMENT))"
        };

        /// <summary>
        /// A method to execute all 'CREATE TABLE' commands
        /// </summary>
        public void Build()
        {
            SQLiteConnection.CreateFile(MainWindow.databasePath);
            using(SQLiteConnection connection = GetConnection())
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    using (SQLiteCommand command = new SQLiteCommand(commands[i], connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}