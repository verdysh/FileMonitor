using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor.Database
{
    internal class DatabaseWriter
    {
        public void InsertRow(string table, Dictionary<string, string> args)
        {
            string command = $"INSERT INTO {table} ";
        }

        public void InsertRow(string table, Dictionary<string, int> args)
        {

        }
    }
}
