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
    public class DirectoryParseDefinition{
        public List<string> Directories{ get; set; } 
        public string ParseType { get; set; }
        public List<object> ParseTypeArgs { get; set; }
        public string FileFilter { get; set; } 
        public string ResultFormat { get; set; }
    }
    public class DirectoryParser
    {
        #region Props
        public List<string> Directories { get; set; } = new List<string>(); 
        public Func<string, string> _ContentFormatter = (c) => (c);
        public Func<string, string> ContentFormatter
        {
            set { _ContentFormatter = value; }
        }
        private string _ResultFormat; 
        public string ResultFormat
        {
            get { return (string.IsNullOrWhiteSpace(_ResultFormat)) ? "{0}" : _ResultFormat; }
            set { _ResultFormat = value; }
        } 
        private Dictionary<string, string> _Results;
        public Dictionary<string, string> Results {
            get { return _Results; }
        } 
        public IParser<string> Parser { get; set; } 
        public string PathExcludePattern { get; set; } = @"\$";
        public string FileFilter { get; set; }
        #endregion

        #region Ctor

        public DirectoryParser()
        {
            _Results = new Dictionary<string, string>();
            Cache.Write("");
        }
        public DirectoryParser(string Directory) : this()
        {
            Directories.Add(Directory);
        }
        #endregion

        #region METHODS

        public void ParseDirectory(string Directory) 
        {
            Directories.Add(Directory);
            ParseDirectory();
        }
        public void ParseDirectory()
        {
            _Results.Clear();
            foreach (var dir in Directories)
            { 
                string ff = (from p in dir.Split(@"\").Reverse() select p).FirstOrDefault(); 
                string filter = (!string.IsNullOrWhiteSpace(ff)) ? ff : FileFilter;

                DirectoryInfo DI = new DirectoryInfo($"{dir.Replace(filter, "")}");
                foreach (var file in DI.GetFiles(filter, SearchOption.AllDirectories))
                {
                    if (this.Parser.ParseMode == ParseMode.Debug)
                        Console.WriteLine($"[DBG]: {file.DirectoryName} {file.Name}"); 

                    if (Regex.IsMatch($"{file.DirectoryName}", PathExcludePattern)) 
                        continue;
                    
                    string content = Reader.Read(file.FullName);
                    StringBuilder result = new StringBuilder(); 
                    foreach (var item in this.Parser.Parse(content))
                    {
                        result.Append(_ContentFormatter(item) + "\n");
                    }
                    if (result.ToString() != ""){
                        if (!_Results.ContainsKey(file.FullName))
                        {
                            string fresult = result.ToString();
                            fresult = ResultFormat.Replace("{{0}}", fresult).Replace("{{1}}", file.FullName);
                            fresult = fresult.Replace("/r", "\n");
                            _Results.Add($"{file.FullName}", $"{fresult}");
                        }
                    }     
                }
            } 
        }
        public void ParseTo(IWriter Writer)
        {
            ParseDirectory();
            Writer.Write(ToString());
        }
        public void Inspect()
        {
            ParseDirectory();
            Cache.Inspect(ToString());
        } 
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            if (this.Parser.ParseMode != ParseMode.Default)
            {
                foreach (KeyValuePair<string, string> kvp in _Results)
                    result.Append($"{kvp.Key}\n");
                foreach (KeyValuePair<string, string> kvp in _Results)
                    result.Append($"{kvp.Key}\n{kvp.Value}\n");
            }
            else
            {
                foreach (KeyValuePair<string, string> kvp in _Results)
                    result.Append($"{kvp.Value}\n");
            }
            return result.ToString();
        } 
        #endregion
    }
}

