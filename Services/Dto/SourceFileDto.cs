namespace Services.Dto
{
    public class SourceFileDto : IHasPath
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            SourceFileDto cast = (SourceFileDto)obj;
            if (Id == cast.Id && Path == cast.Path) return true;
            return false;
        }
    }
}
