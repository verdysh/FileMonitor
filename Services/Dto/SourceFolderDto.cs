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

        /// <summary>
        /// A collection of all file paths that are within this folder, including any subdirectories of the folder. The files are represented as data transfer objects.
        /// </summary>
        public IEnumerable<SourceFileDto>? SourceFiles { get; set; }
    }
}
