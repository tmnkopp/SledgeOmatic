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
    public enum ParseResultMode
    {
        Debug, 
        Verbose,
        Default
    }

    public abstract class BaseParser
    {
        public List<IParseStrategy> Parsers;
        public List<IInterpreter> Compilers;
        public List<string> PathExclusions; 
        public string Path { get; set; } 
        public ParseResultMode ParseResultMode { get; set; }

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
        public BaseParser()
        {
            ParseResultMode = ParseResultMode.Default;
            PathExclusions = new List<string>();
            Parsers = new List<IParseStrategy>(); 
        }
        public void Parse()
        { 
            DirectoryInfo DI = new DirectoryInfo($"{this.Path.Replace(FileFilter, "")}");
            foreach (var file in DI.GetFiles(FileFilter, SearchOption.AllDirectories))
            {
                if (!IsPathExcluded(file.FullName))
                {
                    FileReader r = new FileReader(file.FullName);
                    string content = r.Read();
                    string result = "";
                    foreach (IParseStrategy proc in this.Parsers)
                        content=proc.Parse(content); 
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
            if (ParseResultMode == ParseResultMode.Verbose)
            {
                foreach (KeyValuePair<string, string> KVP in this.Dict)
                    _results.Append($"[{KVP.Key}]\n");
                _results.Append($"\n");
                foreach (KeyValuePair<string, string> KVP in this.Dict)
                    _results.Append($"[{KVP.Key}]\n{KVP.Value}\n");
            }
            else 
            {
                foreach (KeyValuePair<string, string> KVP in this.Dict)
                    _results.Append($"{KVP.Value}\n");
            }  
            string result = _results.ToString();
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
