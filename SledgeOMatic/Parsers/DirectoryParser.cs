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
        public string Dest { get; set; }
    }

    public interface IDirectoryParser
    {
        Func<string, string> ContentFormatter { set; }
        List<string> Directories { get; set; }
        string FileFilter { get; set; }
        IParser<string> Parser { get; set; }
        string PathExcludePattern { get; set; }
        string ResultFormat { get; set; }
        Dictionary<string, string> Results { get; }

        void Inspect();
        void ParseDirectory();
        void ParseDirectory(string Directory); 
        void ParseToFile(string Filename);
        string ToString();
    }

    public class DirectoryParser : IDirectoryParser
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
        public Dictionary<string, string> Results { get; private set; }

        public IParser<string> Parser { get; set; }
        public string PathExcludePattern { get; set; } = @"\$";
        public string FileFilter { get; set; }
        #endregion

        #region Ctor
        private readonly ISomContext somContext;
        public DirectoryParser(ISomContext somContext)
        {
            this.somContext = somContext;
            Results = new Dictionary<string, string>();
            somContext.Cache.Write("");
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
            Results.Clear();
            foreach (var dir in Directories)
            {
                string ff = (from p in dir.Split(@"\").Reverse() select p).FirstOrDefault();
                string filter = (!string.IsNullOrWhiteSpace(ff)) ? ff : FileFilter;

                DirectoryInfo DI = new DirectoryInfo($"{dir.Replace(filter, "")}");
                foreach (var file in DI.GetFiles(filter, SearchOption.AllDirectories))
                {
                    somContext.Logger.Information($"{file.DirectoryName} {file.Name}");
                    if (this.Parser.ParseMode == ParseMode.Debug)
                        Console.WriteLine($"[DBG]: {file.DirectoryName} {file.Name}");

                    if (Regex.IsMatch($"{file.DirectoryName}", PathExcludePattern))
                        continue;

                    string content = "";
                    using (TextReader tr = File.OpenText(file.FullName))
                        content = tr.ReadToEnd();
                     
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in this.Parser.Parse(content))
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                            sb.Append(_ContentFormatter(item) + "\n");
                    }
                    if (sb.ToString() != "")
                    {
                        if (!Results.ContainsKey(file.FullName))
                        {
                            string fresult = sb.ToString();
                            fresult = ResultFormat.Replace("{{0}}", fresult).Replace("{{1}}", file.FullName);
                            fresult = fresult.Replace("/r", "\n");
                            Results.Add($"{file.FullName}", $"{fresult}");
                        }
                    }
                }
            }
        } 
        public void ParseToFile(string Filename)
        { 
            ParseDirectory();
            Filename = Filename.Replace("~", somContext.BasePath);
            using (StreamWriter w = File.AppendText($"{Filename}")) { }
            File.WriteAllText($"{Filename}", ToString(), Encoding.Unicode); 
        }
        public void Inspect()
        {
            ParseDirectory();
            somContext.Cache.Write(ToString());
            somContext.Cache.Inspect();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Parser.ParseMode != ParseMode.Default)
            {
                foreach (KeyValuePair<string, string> kvp in Results)
                    sb.Append($"{kvp.Key}\n");
                foreach (KeyValuePair<string, string> kvp in Results)
                    sb.Append($"{kvp.Key}\n{kvp.Value}\n");
            }
            else
            {
                foreach (KeyValuePair<string, string> kvp in Results)
                    sb.Append($"{kvp.Value}\n");
            }
            return sb.ToString();
        }
        #endregion
    }
}

