namespace Services.Dto
{
    public class SourceFileDto : IHasPath
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool IsModified { get; set; }
    }
}
