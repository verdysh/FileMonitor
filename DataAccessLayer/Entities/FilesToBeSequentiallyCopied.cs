using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    [Table("files_to_be_sequentially_copied")]
    public class FilesToBeSequentiallyCopied
    {
        public int Id { get; set; }
        public int SourceFileId { get; set; }
        public SourceFile SourceFile { get; set; }
    }
}
