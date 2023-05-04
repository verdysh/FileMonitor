using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// An entity class for the full_backup table
    /// </summary>
    [Table("full_backup")]
    public class FullBackup
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
