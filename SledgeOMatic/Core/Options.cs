
using CommandLine; 
using SOM;
using SOM.Compilers;
using SOM.IO;
using SOM.Parsers;
using SOM.Procedures;
using System;
using System.Collections.Generic;

namespace SOM
{
    public interface ISomOptions{
        string Task { get; set; }
        string Path { get; set; }
        SomMode Mode { get; set; }
        bool Verbose { get; set; }
    }
    public class SomOptions : ISomOptions
    {
        [Option('t', "Task")]
        public string Task { get; set; }
        [Option('p', "Path", Default = "", HelpText = "Configuration File Path.")]
        public string Path { get; set; }
        [Option('m', "Mode", Default = SomMode.Cache)]
        public SomMode Mode { get; set; }
        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
    }

    [Serializable]
    [Verb("compile", HelpText = @"Command Runner: som compile -p SomDocParser -m Cache")]
    public class CompileOptions
    {
        [Option('t', "Task")]
        public string Task { get; set; }
        [Option('p', "Path", Default="", HelpText = "Configuration File Path.")]
        public string Path { get; set; }
        [Option('m', "CompileMode", Default = CompileMode.Cache)]
        public CompileMode CompileMode { get; set; }
        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
    }

    [Serializable]
    [Verb("parse", HelpText = "Parse Runner.")]
    public class ParseOptions
    {
        [Option('m', "ParseMode", Default = ParseMode.Default)]
        public ParseMode ParseMode { get; set; }
        [Option('d', "Dir", Default = "")]
        public string Dir { get; set; }
        [Option('f', "Filter", Default = "")]
        public string Filter { get; set; }
        [Option('p', "ConfigFile", Default = "config")]
        public string ConfigFile { get; set; }
        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }  
    }

    [Serializable]
    [Verb("config", HelpText = "Config Runner.")]
    public class ConfigOptions
    {
        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
    } 
}