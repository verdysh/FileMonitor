namespace Services.Dto
{
    /// <summary>
    /// A data transfer object related to the BackupPath Entity. Provides a full directory path for the backup location as selected by the user.
    /// </summary>
    public class BackupPathDto
    {
        /// <summary>
        /// The database primary key. 
        /// </summary>
        /// <remarks>
        /// Use to remove Entities from the database.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// The full file path to the backup directory location.
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// A boolean value to determine if the path has been selected by the user.
        /// </summary>
        /// <remarks>
        /// Data binding is mapped to a checkbox in the UI.
        /// </remarks>
        public bool IsSelected { get; set; }
    }
}
