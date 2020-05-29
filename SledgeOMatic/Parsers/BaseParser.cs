using Newtonsoft.Json;
using SOM.Extentions;
using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SOM.Parsers
{
  
    public abstract class BaseParser
    {
        public List<ICompiler> Compilers = new List<ICompiler>(); 
        public List<string> PathExclusions = new List<string>();
        public Func<string, string> Fromatter = (r) => (r);
        public string Path { get; set; } 
        private string _FileFilter=""; 
        public string FileFilter {
            get {
                if (_FileFilter=="") 
                    _FileFilter = Path.ReverseString().Split(new[] { '\\' })[0].ReverseString(); 
                return (_FileFilter == "") ? "*.*" : _FileFilter;
            }
            set { _FileFilter = value; }
        }
            
        public Dictionary<string, string> Dict = new Dictionary<string, string>(); 

        public void Parse()
        { 
            DirectoryInfo DI = new DirectoryInfo($"{this.Path.Replace(FileFilter, "")}");
            foreach (var file in DI.GetFiles(FileFilter, SearchOption.AllDirectories))
            {
                if (!IsPathExcluded(file.FullName))
                {
                    FileReader r = new FileReader(file.FullName);
                    string content = r.Read();
 
                    foreach (ICompiler proc in this.Compilers)
                        content = proc.Compile(content); 
                      
                    if (content != "")
                        Dict.Add(file.FullName, $"{content}\n");  
                } 
            }
        }
        public void ParseTo(IWriter writer) {
            Parse(); 
            writer.Write(this.ToString()); 
        } 
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this.Dict);
        }
        public override string ToString()
        {
            StringBuilder _results = new StringBuilder();
            foreach (KeyValuePair<string, string> KVP in this.Dict)
                _results.Append($"[{KVP.Key}]\n");
            _results.Append($"\n");
            foreach (KeyValuePair<string, string> KVP in this.Dict) 
                _results.Append( $"[{KVP.Key}]\n{KVP.Value}\n" );

            string result = Fromatter(_results.ToString() );
            return result; 
        }
        private bool IsPathExcluded(string FullFilePath)
        {
            bool ret = false;
            foreach (string exclude in PathExclusions)
            {
                if (FullFilePath.Contains(exclude))
                    return true;
            }
            return ret;
        }
    }
}
