using System.Collections.Generic;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    abstract class SQLQuery
    {
        /// <summary>
        /// Query the table for a single ID column and retrieve the next available ID
        /// </summary>
        /// <param name="column"> Column to retrieve data from </param>
        /// <returns> An integer of the next available column ID </returns>
        protected int GetNextAvailableID(SQLiteConnection connection, string table, string column)
        {
            List<object> list = GetColumnValues(connection, table, column);

            if (list.Count == 0) return 0;
            else
            {
                object lastId = list[list.Count - 1];
                return 1 + (int)lastId; // cast to integer
            }
        }

        /// <summary>
        /// Get a list of values from the specified column
        /// </summary>
        /// <param name="column"> Database table name to get data from </param>
        /// <returns> A list of objects from all entries within the table </returns>
        protected List<object> GetColumnValues(SQLiteConnection connection, string table, string column)
        {
            // start connection
            connection.Open();
            List<object> result = new List<object>();
            string query = $"SELECT {column} FROM {table}";

            // execute command
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) result.Add(reader[column]);
            connection.Close();
            return result;
        }
    }
}
