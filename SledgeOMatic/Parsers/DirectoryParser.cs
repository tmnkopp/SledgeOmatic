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

    public interface IDirectoryParser
    {
        Func<string, string> ContentFormatter { set; }
        List<string> Directories { get; set; }
        string FileFilter { get; set; }
        IParser<string> Parser { get; set; }
        string PathExcludePattern { get; set; }
        string ResultFormat { get; set; }
        Dictionary<string, string> Results { get; }
         
        void ParseDirectory();  
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
        public void ParseDirectory()
        {
            Results.Clear();
            foreach (var dir in Directories)
            {
                string ff = (from p in dir.Split(@"\").Reverse() select p).FirstOrDefault();
                string filter = (!string.IsNullOrWhiteSpace(ff)) ? ff : "*.*"; 
                DirectoryInfo DI = new DirectoryInfo($"{dir.Replace(filter, "")}");
                SearchOption SearchDepth = (SearchOption)somContext.Options.SearchDepth;
                foreach (var file in DI.GetFiles(filter, SearchDepth))
                { 
                    if (Regex.IsMatch($"{file.DirectoryName}", PathExcludePattern, RegexOptions.IgnoreCase)){
                        if (this.somContext.Options.Verbose)
                            somContext.Logger.Information($"EXCLUDE: {file.DirectoryName} {file.Name}");
                        continue;
                    }
                    if (this.somContext.Options.Verbose) 
                        somContext.Logger.Information($"{file.DirectoryName} {file.Name}");

                    using (TextReader tr = File.OpenText(file.FullName))
                        somContext.Content = tr.ReadToEnd();
            
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in this.Parser.Parse(somContext))
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
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (somContext.Options.Mode != SomMode.Cache)
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

