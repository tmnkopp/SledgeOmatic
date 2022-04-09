
using CommandLine; 
using SOM;
using SOM.Compilers;
using SOM.IO;
using SOM.Parsers;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;

namespace SOM
{
    public interface ISomOptions{
        string Task { get; set; }
        string Path { get; set; }
        string Args { get; set; }
        int SearchDepth { get; set; }
        SomMode Mode { get; set; }
        bool Verbose { get; set; }
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
    }

    [Serializable]
    [Verb("parse", HelpText = @"Command Runner: som compile -p SomDocParser -m Cache")]
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
    } 
    [Serializable]
    [Verb("config", HelpText = "Config Runner.")]
    public class ConfigOptions
    {
        [Option('a', "Args", HelpText = "-a \"/p param1 /p param2\"")]
        public string Args { get; set; }
        [Option('b', "Bootstrap", HelpText = "Bootstraps Som")]
        public bool Bootstrap { get; set; } 
        private string path;
        [Option('p', "Path", HelpText = "-p \" c:\\basepath_to_som  \"")]
        public string Path
        {
            get { return path.Trim().Replace(@"\\",@"\"); }
            set { path = value; }
        }

        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
    } 
}