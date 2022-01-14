 
using CommandLine; 
using SOM;
using SOM.IO;
using SOM.Parsers;
using SOM.Procedures;
using System.Collections.Generic;

namespace SOM
{
    public class SchemaParseArguments
    {
        [Option('m', "Model", Default = null)]
        public string Model { get; set; }
        [Option('t', "Template", Default = null)]
        public string Template { get; set; }
        [Option('f', "Format", Default = null)]
        public string Format { get; set; }
        [Option('p', "Path", Default = null)]
        public string Path { get; set; }
        [Option('i', "Interpreter", Default = null)]
        public string Interpreter { get; set; }
        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }  
        [Option('s', Separator = ',', Default = new string[0])]
        public IEnumerable<string> sequence { get; set; } = new string[0];
    }
    public class SomParseArguments
    {
        [Option('a', Separator = ',', Default = new string[0])]
        public IEnumerable<string> Args { get; set; } = new string[0];

    }
}