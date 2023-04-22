using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// An Entity class for the source_file table
    /// </summary>
    [Table("source_file")]
    public class SourceFile
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
