using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines an abstract class for all SQL table classes to inherit from 
    /// </summary>
    internal abstract class TableBase
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

        /// <summary>
        /// Query the table for a single ID column and retrieve the next available ID
        /// </summary>
        protected int GetNextAvailableID(List<int> iDs, string table, string column)
        {
            if (iDs.Count == 0) return 0;
            else
            {
                object lastId = iDs[iDs.Count - 1];
                return 1 + (int)lastId; // cast to integer
            }
        }

        /// <summary>
        /// Get a list of object values from the specified database column
        /// </summary>
        protected List<object> GetColumnValuesAsObjects(string table, string column)
        {
            // start connection
            using (SQLiteConnection connection = GetConnection())
            {
                List<object> result = new List<object>();
                string query = $"SELECT {column} FROM {table}";

                // execute command
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) result.Add(reader[column]);
                connection.Dispose();
                return result;
            }
        }

        /// <summary>
        /// Cast a list of objects to the specified type. 
        /// </summary>
        /// <remarks> 
        /// Pass in a list of objects from the database. This method converts the list to the specified type.
        /// </remarks>
        protected List<T>? CastObjectValues<T>(List<object> values)
        {
            List<T> paths = new List<T>();
            foreach (object entry in values)
            {
                paths.Add((T)entry); // Cast object to <T>
            }
            return paths;
        }
    }
}
