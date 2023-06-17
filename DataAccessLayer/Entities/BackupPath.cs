using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    /// <summary>
    /// An entity class for the backup_path table.
    /// </summary>
    [Table("backup_path")]
    public class BackupPath
    {
        /// <summary>
        /// Database primary key. 
        /// </summary>
        /// <remarks>
        /// Use to remove Entities from the database.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// Full file path to where the backup directory location is stored.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// A <see cref="bool"/> property that determines whether or not this path is selected in the UI.
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
