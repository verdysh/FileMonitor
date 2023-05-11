using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// An Entity class for the backup_file table
    /// </summary>
    [Table("backup_file")]
    public class BackupFile
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int SourceFileId { get; set; }
        public SourceFile SourceFile { get; set; }
    }
}
