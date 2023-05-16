using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FilesToSequentiallyCopy.Dto
{
    public class FileToSequentiallyCopyDto
    {
        public int Id { get; set; }
        public int SourcFileId { get; set; }
        public string Path { get; set; }
    }
}
