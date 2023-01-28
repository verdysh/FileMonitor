namespace FileMonitor.Database
{
    internal class DatabaseInserter
    {
        public void Insert(string table, string columns, string data)
        {
            string command = $"INSERT INTO {table} {columns} values {data}";
        }
    }
}
