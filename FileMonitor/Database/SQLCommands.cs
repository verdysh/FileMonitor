using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal record class SQLCommands
    {
        public readonly string create = "CREATE TABLE";
        public readonly string insert = "INSERT INTO";
        public readonly Dictionary<string, string> TablesColumnsCreate = new Dictionary<string, string>()
        {
            { "source_file", "(id INT, path VARCHAR(260))" },
            { "backup_file", "(id INT, path VARCHAR(260))" },
            { "source_backup_file_rel", "(source_file_id INT, backup_file_id INT)" },
            { "source_hash", "(id INT, hashcode VARCHAR(160))" },
            { "backup_hash", "(id INT, hashcode VARCHAR(160))" },
            { "source_backup_hash_rel", "(source_hash_id INT, backup_hash_id INT)" },
            { "source_file_hash_rel", "(source_file_id INT, source_hash_id VARCHAR(160))" },
            { "backup_file_hash_rel", "(backup_file_id INT, backup_hash_id VARCHAR(160))" },
        };

        public readonly Dictionary<string, string> TablesColumnsInsert= new Dictionary<string, string>()
        {
            { "source_file", "(id, path)" },
            { "backup_file", "(id, path)" },
            { "source_backup_file_rel", "(source_file_id, backup_file_id)" },
            { "source_hash", "(id, hashcode)" },
            { "backup_hash", "(id, hashcode)" },
            { "source_backup_hash_rel", "(source_hash_id, backup_hash_id)" },
            { "source_file_hash_rel", "(source_file_id, source_hash_id)" },
            { "backup_file_hash_rel", "(backup_file_id, backup_hash_id)" },
        };
    }
}
