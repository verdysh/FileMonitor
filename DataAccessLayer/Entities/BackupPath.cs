using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// An entity class for the backup_path table
    /// </summary>
    [Table("backup_path")]
    public class BackupPath
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool IsSelected { get; set; }
    }
}
