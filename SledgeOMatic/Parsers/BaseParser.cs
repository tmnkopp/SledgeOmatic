using Newtonsoft.Json;
using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Parsers
{
  
    public abstract class BaseParser
    {
        public string DirSource = "";
        public IWriter Dest;
        public string Find = "";
        public string FileFilter = "*.*"; 
        public List<ICompiler> Parsers ; 
        public List<string> ExcludeList = new List<string>(); 
        public Dictionary<string, string> Result = new Dictionary<string, string>(); 
        public void Parse()
        {
            int cnt = 0;
            DirectoryInfo DI = new DirectoryInfo($"{this.DirSource}");
            foreach (var file in DI.GetFiles(FileFilter, SearchOption.AllDirectories))
            {
                if (!IsPathExcluded(file.FullName))
                {
                    FileReader r = new FileReader(file.FullName);
                    string content = r.Read().Replace("\t", "").Replace("  ", " ");
 
                    if (content.Contains(Find)) 
                    { 
                        foreach (ICompiler proc in this.Parsers)
                            content = proc.Compile(content); 

                        if (!Result.ContainsKey(file.FullName))
                            Result.Add(file.FullName, $"{file.FullName}\n");

                        Result[file.FullName] = content; 
                        cnt++;
                    } 
                } 
            }
        }
        public void ParseTo(IWriter writer) {
            Parse(); 
            writer.Write(this.ToString()); 
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this.Result);
        }
        public override string ToString()
        {
            StringBuilder _results = new StringBuilder();
            foreach (KeyValuePair<string, string> KVP in this.Result) 
                _results.Append( $"[{KVP.Key}]\n{KVP.Value}\n" );
           
            return _results.ToString();
        }
        private bool IsPathExcluded(string FullFilePath)
        {
            bool ret = false;
            foreach (string exclude in ExcludeList)
            {
                if (FullFilePath.Contains(exclude))
                    return true;
            }
            return ret;
        }
    }
}
