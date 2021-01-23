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

    public class DirectoryParser   
    {
        #region Props
        private List<string> _Directories; 
        public void AddDirectory(string Dir) => _Directories.Add(Dir);

        public Func<string, string> _ContentFormatter = (c) => (c);
        public Func<string, string> ContentFormatter
        {
            set { _ContentFormatter = value; }
        }

        private Dictionary<string, string> _Results;
        public Dictionary<string, string> Results {
            get { return _Results; } 
        }
        private IParser _Parser;
        public IParser Parser
        { 
            set { _Parser = value; }
            get { return _Parser; }
        } 
        private string _Directory;
        public string Directory
        {
            get { return _Directory; }
            set { _Directory = value; }
        } 
        public string FileFilter
        {
            get { return Directory.ReverseString().Split(new[] { '\\' })[0].ReverseString(); } 
        } 
        #endregion

        #region ctor

        public DirectoryParser()
        {
            _Results = new Dictionary<string, string>(); 
        }
        public DirectoryParser(string Directory) : this()
        {
            _Directory = Directory;  
        }

        #endregion
       
        public void ParseDirectory()
        {
            _Results.Clear();
            DirectoryInfo DI = new DirectoryInfo($"{this._Directory.Replace(FileFilter, "")}");
            foreach (var file in DI.GetFiles(FileFilter, SearchOption.AllDirectories))
            {
                string content = new FileReader(file.FullName).Read(); 
                StringBuilder result = new StringBuilder();
                foreach (var item in _Parser.Parse(content)) { 
                    result.Append(_ContentFormatter(item));
                }
                if (result.ToString() != "") 
                    _Results.Add($"{file.FullName}", $"{result.ToString()}"); 
            }
        }
        public void ParseTo(IWriter Writer)
        {
            ParseDirectory();
            Writer.Write(ToString());
        }
        public void Inspect()
        {
            Cache.Write("");
            ParseDirectory();
            Cache.Write(ToString());
            Cache.CacheEdit();
        }
        
        public override string ToString() {
            StringBuilder result = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in _Results)
                result.Append($"{kvp.Key}\n");
            foreach (KeyValuePair<string,string> kvp in _Results)
                result.Append($"{kvp.Key}\n{kvp.Value}\n");
            return result.ToString();
        }  
    }
}

