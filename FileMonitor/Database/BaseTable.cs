using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines an abstract class for all SQL table classes to inherit from 
    /// </summary>
    /// <remarks>
    /// All tables use the INTEGER PRIMARY KEY for each row id. 
    /// </remarks>
    /// 
    /// The following text provides a layout for all database tables:
    /// 
    /// The "source_file" table stores paths to files that the user wants to monitor. 
    /// 
    /// +----------------------------+
    /// |         source_file        |
    /// +----+-----------------------+
    /// | id |         path          |
    /// +----+-----------------------+
    /// | 1  | C:\\Windows\\File.txt |
    /// +----+-----------------------+
    /// 
    /// 
    /// 
    /// The "backup_file" table stores paths that have been copied to another drive. The copied files retain
    /// their original folder structure.
    /// 
    /// +----------------------------+
    /// |         backup_file        |
    /// +----+-----------------------+
    /// | id |         path          |
    /// +----+-----------------------+
    /// | 1  | D:\\Windows\\File.txt |
    /// +----+-----------------------+
    /// 
    /// 
    /// 
    /// The "source_backup_file_rel" table relates all source file paths to their copied file paths.
    /// 
    /// +--------------------------------------+
    /// |        source_backup_file_rel        |
    /// +----+----------------+----------------+
    /// | id | source_file_id | backup_file_id |
    /// +----+----------------+----------------+
    /// |  1 |        1       |        1       |
    /// +----+----------------+----------------+
    /// 
    /// 
    /// 
    /// The "source_hash" table stores hash codes based on all source files. The hash codes 
    /// are used to determine if a file has changed since the last time it was copied to the 
    /// backup location.
    /// 
    /// +------------------+
    /// |    source_hash   |
    /// +----+-------------+
    /// | id |  hash_code  |
    /// +----+-------------+
    /// |  1 | 0be624[...] |
    /// +----+-------------+
    /// 
    /// 
    /// 
    /// The "backup_hash" table stores hash codes based on all of the copied files. If this hash code 
    /// differs from the related source file hash code, then the file has changed.
    /// 
    /// +------------------+
    /// |    backup_hash   |
    /// +----+-------------+
    /// | id |  hash_code  |
    /// +----+-------------+
    /// |  1 | 0yrxi4[...] |
    /// +----+-------------+
    /// 
    /// 
    /// 
    /// The "source_backup_hash_rel" table relates the source file hash code to the backup file hash code.
    /// This is used to compare the hash codes to determine if a file has changed.
    /// 
    /// +--------------------------------------+
    /// |        source_backup_hash_rel        |
    /// +----+----------------+----------------+
    /// | id | source_hash_id | backup_hash_id |
    /// +----+----------------+----------------+
    /// |  1 |        1       |        1       |
    /// +----+----------------+----------------+
    /// 
    /// 
    /// 
    /// The "source_file_hash_rel" table relates the source file to its hash code. 
    /// 
    /// +--------------------------------------+
    /// |         source_file_hash_rel         |
    /// +----+----------------+----------------+
    /// | id | source_file_id | source_hash_id |
    /// +----+----------------+----------------+
    /// |  1 |        1       |        1       |
    /// +----+----------------+----------------+
    /// 
    ///
    /// 
    /// The "backup_file_hash_rel" table relates the backup file to its hash code.
    /// 
    /// +--------------------------------------+
    /// |         backup_file_hash_rel         |
    /// +----+----------------+----------------+
    /// | id | backup_file_id | backup_hash_id |
    /// +----+----------------+----------------+
    /// |  1 |        1       |        1       |
    /// +----+----------------+----------------+
    /// 
    /// 
    /// 
    /// The "full_backup_location" table refers to user-specified locations. If the user initiates a full
    /// backup of all monitored files, the backup will be moved to one of these locations.
    /// 
    /// +----------------------+
    /// | full_backup_location |
    /// +------+---------------+
    /// |  id  |      path     |
    /// +------+---------------+
    /// |   1  |  F:\\Backups  |
    /// +------+---------------+
    /// 
    /// 
    /// 
    /// The "consecutive_backup_location" table refers to user-specified locations. If the user wants to 
    /// copy the monitored files only when they change, the copied files will be moved to one of these 
    /// locations.
    /// 
    /// +-----------------------------+
    /// | consecutive_backup_location |
    /// +----------+------------------+
    /// |    id    |       path       |
    /// +----------+------------------+
    /// |     1    |    G:\\Backups   |
    /// +----------+------------------+


    internal abstract class BaseTable
    {
        /// <summary>
        /// Return an opened database connection object
        /// </summary>
        protected SQLiteConnection GetConnection()
        {
            SQLiteConnection connection = new SQLiteConnection($"Data Source={MainWindow.databasePath};Version=3;");
            connection.Open();
            return connection;
        }

        protected int GetNextAvailableID(Dictionary<int, string> columns)
        {
            for (int i = 0; i < columns.Count; i++) 
            {
                if (!columns.ContainsKey(i)) return i;
            } 
            return columns.Count;
        }

        /// <summary>
        /// Select all values from the specified database column, returned as a Dictionary
        /// 
        /// The following sample shows the query as a formatted string: 
        /// $"SELECT * FROM {table}"
        /// 
        /// Sample query:
        /// SELECT path FROM source_file
        /// </summary>
        protected Dictionary<int, string> SQLSelectFrom(string table, string idColumn, string valueColumn)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            string query = $"SELECT * FROM {table}";
            int key;
            string value;
            using (SQLiteConnection connection = GetConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) 
                        {
                            key = reader.GetInt32(idColumn); 
                            value = reader.GetString(valueColumn);
                            result.Add(key, value);
                        } 
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// A method for executing the SQL INSERT statement
        /// 
        /// The following sample shows the statement as a formatted string: 
        /// $"INSERT INTO {table} ({column1}, {column2}) values ({id}, \'{varCharValue}\')";
        /// 
        /// Sample statement:
        /// INSERT INTO source_file (id, path) VALUES (45, 'C:\\NewFilePath')
        /// </summary>
        /// <remarks> Overload for any table that has an integer and a varchar column </remarks>
        protected void SQLInsertInto(string table, string column1, string column2, int id, string varCharValue)
        {
            string insertStatement = $"INSERT INTO {table} ({column1}, {column2}) VALUES ({id}, \'{varCharValue}\')";
            using (SQLiteConnection connection = GetConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(insertStatement, connection))
                {
                    command.ExecuteNonQuery();
                }
            }         
        }

        /// <summary>
        /// A method for executing the SQL INSERT statement
        /// 
        /// The following sample shows the statement as a formatted string: 
        /// $"INSERT INTO {table} ({column1}, {column2}) values ({id1}, {id2})"
        /// 
        /// Sample statement: 
        /// INSERT INTO source_file_hash_rel (source_file_id, source_hash_id) values (56, 72) 
        /// </summary>
        /// <remarks> Overload for any table that has two integer columns </remarks>
        protected void SQLInsertInto(string table, string column1, string column2, int id1, int id2)
        {
            string insertStatement = $"INSERT INTO {table} ({column1}, {column2}) values ({id1}, {id2})";
            using (SQLiteConnection connection = GetConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(insertStatement, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Remove a single row from the specified database table
        /// 
        /// The following sample shows the statement as a formatted string:
        /// $"DELETE FROM {table} WHERE {column} = {id}"
        /// 
        /// Sample statement: DELETE FROM source_file WHERE id = 4
        /// </summary>
        protected void SQLDeleteFrom(string table, string column, int id)
        {
            string deleteStatement = $"DELETE FROM {table} WHERE {column} = {id}";
            using(SQLiteConnection connection = GetConnection())
            {
                using(SQLiteCommand command = new SQLiteCommand(deleteStatement, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
