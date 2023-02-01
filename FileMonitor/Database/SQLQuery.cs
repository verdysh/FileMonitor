using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace FileMonitor.Database
{
    internal class SQLQuery
    {
        private readonly string path;
        private readonly string table;
        public string Table { get => table; } // return this.table

        /// <summary>
        /// Defines the class constructor
        /// </summary>
        /// <param name="path"> Path to database file </param>
        public SQLQuery(string path, string table)
        {
            this.path = path;
            this.table = table;
        }

        public int GetNextAvailableID(string column)
        {
            List<object> list = GetColumnIDs(column);

            if (list.Count == 0) return 0;
            else
            {
                object lastId = list[list.Count - 1];
                return -1;
            }
        }

        private List<object> GetColumnIDs(string column)
        {
            // init
            List<object> result = new List<object>();
            string query = $"SELECT {column} FROM {table}";

            // start connection
            SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Version=3;");
            connection.Open();

            // execute command
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) result.Add(reader["id"]);
            connection.Close();
            return result;
        }
    }
}
