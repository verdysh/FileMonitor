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
            "CREATE TABLE source_file (id INT PRIMARY KEY, path VARCHAR(260))",
            "CREATE TABLE backup_file (id INT PRIMARY KEY, path VARCHAR(260))",
            "CREATE TABLE source_backup_file_rel (id INT PRIMARY KEY, source_file_id INT, backup_file_id INT)",
            "CREATE TABLE source_hash (id INT PRIMARY KEY, hash_code VARCHAR(160))",
            "CREATE TABLE backup_hash (id INT PRIMARY KEY, hash_code VARCHAR(160))",
            "CREATE TABLE source_backup_hash_rel (id INT PRIMARY KEY, source_hash_id INT, backup_hash_id INT)",
            "CREATE TABLE source_file_hash_rel (id INT PRIMARY KEY, source_file_id INT, source_hash_id INT)",
            "CREATE TABLE backup_file_hash_rel (id INT PRIMARY KEY, backup_file_id INT, backup_hash_id INT)"
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
                    SQLiteCommand command = new SQLiteCommand(commands[i], connection);
                    command.ExecuteNonQuery();
                }
                connection.Dispose();
            }
        }
    }
}