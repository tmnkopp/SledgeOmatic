
using CommandLine; 
using SOM;
using SOM.Compilers;
using SOM.IO;
using SOM.Parsers;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SOM
{
    public interface ISomOptions{
        string Task { get; set; }
        string Path { get; set; }
        string Args { get; set; }
        int SearchDepth { get; set; }
        SomMode Mode { get; set; }
        bool Verbose { get; set; }
        List<string> GetArgs { get;  }

    }
    [Serializable]
    [Verb("compile", HelpText = @"Command Runner: som compile -p SomDocParser -m Cache")]
    public class CompileOptions : ISomOptions
    { 
        [Option('t', "Task")]
        public string Task { get; set; }
        [Option('p', "Path", Default = "", HelpText = "Config File Path.")]
        public string Path { get; set; }
        [Option('a', "Args", Default = "")]
        public string Args { get; set; }
        [Option('m', "Mode", Default = SomMode.Cache)]
        public SomMode Mode { get; set; }
        [Option('d', "SearchDepth", Default = 1, HelpText = "SearchOption.AllDirectories = 1")]
        public int SearchDepth { get; set; }
        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
        public List<string> GetArgs
        { 
            get{
                return (from s in new List<string>(Regex.Split(this.Args.Trim(), @"/p ")) where !string.IsNullOrWhiteSpace(s) select s.Trim()).ToList();
            }
        }
}

    [Serializable]
    [Verb("parse", HelpText = @"Command Runner: som parse -p CS -v -d 1 --help ")]
    public class ParseOptions : ISomOptions
    {
        [Option('a', "Args", Default = "")]
        public string Args { get; set; }
        [Option('t', "Task")]
        public string Task { get; set; }
        [Option('p', "Path", Default = "", HelpText = "Config File Path.")]
        public string Path { get; set; }
        [Option('m', "Mode", Default = SomMode.Cache)]
        public SomMode Mode { get; set; }
        [Option('d', "SearchDepth", Default = 1, HelpText = "SearchOption.AllDirectories = 1")]
        public int SearchDepth { get; set; } 
        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
        public List<string> GetArgs
        {
            get
            {
                return (from s in new List<string>(Regex.Split(this.Args.Trim(), @"/p ")) where !string.IsNullOrWhiteSpace(s) select s.Trim()).ToList();
            }
        }
    } 
    [Serializable]
    [Verb("config", HelpText = "Config Runner.")]
    public class ConfigOptions
    {
        [Option('a', "Args", HelpText = "-a \"/p:param1=value /p:param2=value \"")]
        public string Args { get; set; }
        [Option('b', "Bootstrap", HelpText = "Bootstraps Som")]
        public bool Bootstrap { get; set; } 
        private string path;
        [Option('p', "Path", HelpText = "-p \" c:\\basepath_to_som  \"")]
        public string Path
        {
            get { return path?.Trim().Replace(@"\\",@"\"); }
            set { path = value; }
        }

        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
    } 
}