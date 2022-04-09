using System.Collections.Generic;

namespace SOM.Parsers
{
    public class DirectoryParseDefinition{
        public List<string> Directories{ get; set; } 
        public string ParseType { get; set; }
        public List<object> ParseTypeArgs { get; set; }
        public string FileFilter { get; set; } 
        public string ResultFormat { get; set; }
        public string Dest { get; set; }
    }
}

