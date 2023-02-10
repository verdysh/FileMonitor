using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines an abstract class for all SQL table classes to inherit from 
    /// </summary>
    internal abstract class SQLQuery
    {
        /// <summary>
        /// Query the table for a single ID column and retrieve the next available ID
        /// </summary>
        protected int GetNextAvailableID(string table, string column)
        {
            List<object> list = GetColumnValues(table, column);

            if (list.Count == 0) return 0;
            else
            {
                object lastId = list[list.Count - 1];
                return 1 + (int)lastId; // cast to integer
            }
        }

        /// <summary>
        /// Get a list of object values from the specified database column
        /// </summary>
        protected List<object> GetColumnValues(string table, string column)
        {
            // start connection
            SQLiteConnection connection = new SQLiteConnection($"Data Source={MainWindow.databasePath};Version=3;");
            connection.Open();
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
}
