using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// An entity class for the sequential_backup table
    /// </summary>
    [Table("sequential_backup")]
    public class SequentialBackup
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
