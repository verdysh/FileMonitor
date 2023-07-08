namespace Services.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class SourceFolderDto
    {
        /// <summary>
        /// The database primary key. 
        /// </summary>
        /// <remarks>
        /// Use to remove Entities from the database.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// The full file path to where the folder is stored.
        /// </summary>
        public string? Path { get; set; }
    }
}
