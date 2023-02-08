using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    internal class SQLNonQuery
    {
        private string _path;

        /// <summary>
        /// Defines the class constructor
        /// </summary>
        /// <param name="path"> Path to the program database file </param>
        public SQLNonQuery(string path)
        {
            this._path = path;
        }

        /// <summary>
        /// A method to insert data into the database tables. This method creates a dictionary called 
        /// 'commands' where all the 'insert' SQL statements are stored. 
        /// </summary>
        /// <param name="key"> Name of the the database table, points to the SQL command </param>
        /// <param name="data"> 
        /// Data to insert into the table. 
        /// Example data value: "(12, 'C:\\Program Files\\File.txt')"
        /// </param>
        /// <remarks>
        /// Example of a complete statement: 
        /// "INSERT INTO source_file (id, path) values (12, 'C:\\Program Files\\File.txt')"
        /// </remarks>
        public void Insert(string key, string data)
        {
            Dictionary<string, string> commands = new Dictionary<string, string>() 
            {
                { "source_file", $"INSERT INTO source_file (id, path) values {data}"},
                { "backup_file", $"INSERT INTO backup_file (id, path) values {data}"},
                { "source_backup_file_rel", $"INSERT INTO source_backup_file_rel (source_file_id, backup_file_id) values {data}"},
                { "source_hash", $"INSERT INTO source_hash (id, hashcode) values {data}"},
                { "backup_hash", $"INSERT INTO backup_hash (id, hashcode) values {data}"},
                { "source_backup_hash_rel", $"INSERT INTO source_backup_hash_rel (source_hash_id, backup_hash_id) values {data}"},
                { "source_file_hash_rel", $"INSERT INTO source_file_hash_rel (source_file_id, source_hash_id) values {data}"},
                { "backup_file_hash_rel", $"INSERT INTO backup_file_hash_rel (backup_file_id, backup_hash_id) values {data}"},
            };

            SQLiteConnection connection = new SQLiteConnection($"Data Source={_path};Version=3;");
            connection.Open();

            string command = commands[key];
            SQLiteCommand sqlCommand = new SQLiteCommand(command, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
