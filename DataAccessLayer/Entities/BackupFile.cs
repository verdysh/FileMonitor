using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// An Entity class for the backup_file table.
    /// </summary>
    [Table("backup_file")]
    public class BackupFile
    {
        /// <summary>
        /// Database primary key. 
        /// </summary>
        /// <remarks>
        /// Use to remove Entities from the database.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// Full file path to where the backup file is stored.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Id for the related SourceFile. 
        /// </summary>
        /// <remarks>
        /// The BackupFile is a copy of this SourceFile. 
        /// </remarks>
        public int SourceFileId { get; set; }

        /// <summary>
        /// A navigation property. Establishes the relationship between this class and the SourceFile class.
        /// </summary>
        public SourceFile SourceFile { get; set; }
    }
}
