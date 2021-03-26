using SOM.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;
using System.Text.RegularExpressions;
using CommandLine;

namespace SOM.Procedures
{
    public enum ParseMode  {
        Debug, Verbose, Default
    }
    public interface IParser<R>
    {
        IEnumerable<R> Parse(string content);
        ParseMode ParseMode { get; set; }
    }  
    public class CommandParseResult {
        public string ArgumentCommandLine = "";
        public string[] Args {
            get
            {
                MatchCollection mc = Regex.Matches(ArgumentCommandLine.Split("\n")[0], @"-\w [^-]*");
                return (from m in mc select m.Value.Trim().Replace("\\n", "\n").Replace("\\t", "\t") ?? "{0}").ToArray();
            }
        }
        public SchemaParseArguments Arguments {
            get
            {
                return new CommandLine
                    .Parser(with => with.HelpWriter = null)
                    .ParseArguments<SchemaParseArguments>(Args)
                    .MapResult(o => o, o => null);
            }
        }
        public CommandParseResult(string Result, string ArgumentCommandLine)
        {
            this.Result = Result;
            this.ArgumentCommandLine = ArgumentCommandLine; 
        }
        public string Result { get; set; } 
    }
     
    public abstract class BaseParser
    {
        private string content = "";
        public string Content
        {
            get  { return content;  }
            set  { content = value; }
        } 
        private ParseMode _ParseResultMode = ParseMode.Default;
        public ParseMode ParseMode
        { 
            set { _ParseResultMode = value; }
            get { return  _ParseResultMode; }
        } 
        public BaseParser()
        { 
        } 
    }
}
