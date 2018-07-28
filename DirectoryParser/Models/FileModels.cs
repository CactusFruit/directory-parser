using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryParser.Models.FileModels
{
    public class FileDescription
    {
        public String DirectoryPath { get; set; }
        public String Filename { get; set; }
        public long SizeBytes { get; set; }
        public DateTime FileCreationTime { get; set; }
    }
}
