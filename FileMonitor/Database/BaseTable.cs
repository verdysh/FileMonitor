using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;

namespace FileMonitor.Database
{
    /// <summary>
    /// Defines an abstract class for all SQL table classes to inherit from 
    /// </summary>
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
        /// <remarks> 
        /// Example query: "SELECT {column} FROM {table}" where "column" and 
        /// "table" represent the method arguments.
        /// </remarks>
        protected List<object> SQLSelectFromColumn(string table, string column)
        {
            List<object> result = new List<object>();
            string query = $"SELECT {column} FROM {table}";
            using (SQLiteConnection connection = GetConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) result.Add(reader[column]);
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// This method casts each object in a List to the specified type.
        /// </summary>
        /// <param name="values"> A List of objects from the database column </param>
        protected List<T>? CastListFromObject<T>(List<object> values)
        {
            List<T> valuesCast = new List<T>();
            foreach (object entry in values)
            {
                valuesCast.Add((T)entry); // Cast object to <T>
            }
            return valuesCast;
        }

        /// <summary>
        /// Convert a List to an ObservableCollection
        /// </summary>
        protected ObservableCollection<T>? ConvertToObservableCollection<T>(List<T> values)
        {
            ObservableCollection<T> result = new ObservableCollection<T>();
            foreach (T entry in values)
            {
                result.Add(entry); // Cast object to <T>
            }
            return result;
        }
    }
}
