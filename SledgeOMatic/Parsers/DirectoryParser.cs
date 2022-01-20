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
        public string ParseMode { get; set; }
        public string ParseType { get; set; }
        public List<object> ParseTypeArgs { get; set; }
        public string FileFilter { get; set; }
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

        private Dictionary<string, string> _Results;
        public Dictionary<string, string> Results {
            get { return _Results; }
        }
        private IParser<string> _Parser;
        public IParser<string> Parser
        {
            set { _Parser = value; }
            get { return _Parser; }
        } 
        private string _PathExcludePattern = @"\$";
        public string PathExcludePattern
        {
            get { return _PathExcludePattern ?? "~~~~"; }
            set { _PathExcludePattern = value; }
        }
        private string _PathIncludePattern = @".*";
        public string PathIncludePattern
        {
            get { return _PathIncludePattern ?? ".*"; }
            set { _PathIncludePattern = value; }
        }
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
                DirectoryInfo DI = new DirectoryInfo($"{dir.Replace(FileFilter, "")}");
                foreach (var file in DI.GetFiles(FileFilter, SearchOption.AllDirectories))
                {
                    if (_Parser.ParseMode == ParseMode.Debug)
                        Console.WriteLine($"debug DirectoryName: {file.DirectoryName}");

                    if (Regex.IsMatch($"{file.DirectoryName}", PathExcludePattern))
                    {
                        if (_Parser.ParseMode == ParseMode.Debug)
                            Console.WriteLine($"debug PathExcludePattern: {PathExcludePattern} {file.DirectoryName}");
                        continue;
                    }

                    string content = Reader.Read(file.FullName);
                    StringBuilder result = new StringBuilder();
                    foreach (var item in _Parser.Parse(content))
                    {
                        result.Append(_ContentFormatter(item));
                    }
                    if (result.ToString() != "")
                        _Results.Add($"{file.FullName}", $"{result.ToString()}");
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
            if (_Parser.ParseMode != ParseMode.Default)
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

