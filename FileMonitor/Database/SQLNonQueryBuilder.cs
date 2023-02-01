using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Class dedicated to creating the initial database
    /// </summary>
    internal class SQLNonQueryBuilder
    {
        private string path;

        /// <summary>
        /// Defines the class constructor
        /// </summary>
        /// <param name="path"> Path to the program database file </param>
        public SQLNonQueryBuilder(string path) 
        {
            this.path = path;
        }

        /// <summary>
        /// An array containing all the commands for creating the database tables
        /// </summary>
        private readonly string[] commands = new string[]
        {
            "CREATE TABLE source_file (id INT, path VARCHAR(260))",
            "CREATE TABLE backup_file (id INT, path VARCHAR(260))",
            "CREATE TABLE source_backup_file_rel (source_file_id INT, backup_file_id INT)",
            "CREATE TABLE source_hash (id INT, hashcode VARCHAR(160))",
            "CREATE TABLE backup_hash (id INT, hashcode VARCHAR(160))",
            "CREATE TABLE source_backup_hash_rel (source_hash_id INT, backup_hash_id INT)",
            "CREATE TABLE source_file_hash_rel (source_file_id INT, source_hash_id VARCHAR(160))",
            "CREATE TABLE backup_file_hash_rel (backup_file_id INT, backup_hash_id VARCHAR(160))"
        };

        /// <summary>
        /// A method to create a SQLite connection and execute all 'CREATE TABLE' commands
        /// </summary>
        public void Create()
        {
            SQLiteConnection.CreateFile(path);
            SQLiteConnection SQLconnection = new SQLiteConnection($"Data Source={path};Version=3;");
            SQLconnection.Open();

            for (int i = 0; i < commands.Length; i++)
            {
                SQLiteCommand command = new SQLiteCommand(commands[i], SQLconnection);
                command.ExecuteNonQuery();
            }
            SQLconnection.Close();
        }
    }
}