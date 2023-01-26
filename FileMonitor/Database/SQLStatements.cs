namespace FileMonitor.Database
{
    /// <summary>
    /// A data class to store all SQL statements in one location
    /// </summary>
    internal class SQLStatements
    {
        public readonly string[] tablesColumnsCreate = new string[]
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
    }
}
