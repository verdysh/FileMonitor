namespace Services.Dto
{
    public class SourceFileDto
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(this, obj)) return true;
            SourceFileDto? sourceFileDto = obj as SourceFileDto;
            if (sourceFileDto == null) return false;
            else return sourceFileDto.Id == Id && sourceFileDto.Path == Path;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
