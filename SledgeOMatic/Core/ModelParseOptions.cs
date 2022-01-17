 
using CommandLine; 
using SOM;
using SOM.IO;
using SOM.Parsers;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOM
{
    public class SomParseOptions
    {
        [Option('m', "Model", Default = null)]
        public string Model { get; set; } 
        [Option('f', "Format", Default = null)]
        public string Format { get; set; } 
        [Option('t', "Template", Default = null)]
        public string Template { get; set; }
        [Option('p', "Params", Default = null)]
        public string Params { get; set; }
        public string[] ParamParsed { get { 
                return (from p in Params?.Split(new[] { "/p" }, StringSplitOptions.None)
                        where !string.IsNullOrWhiteSpace(p)
                        select p.Trim()
                    ).ToArray();
            } 
        }
        [Option('v', "Verbose", HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }  
        [Option('l', Separator = ',', Default = new string[0])]
        public IEnumerable<string> Sequence { get; set; } = new string[0];
        [Option('j')]
        public string Json { get; set; }
    } 
}