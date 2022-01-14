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
        public string RawOptions = "";
        public string[] Args {
            get
            {
                MatchCollection mc = Regex.Matches(RawOptions.Split("\n")[0], $@"-\w [^-]*");
                return (from m in mc select m.Value.Trim().Replace("\\n", "\n").Replace("\\t", "\t") ?? "{0}").ToArray();
            }
        } 
        public T Options<T>()  {
       
                return new CommandLine
                    .Parser(with => with.HelpWriter = null)
                    .ParseArguments<T>(Args)
                    .MapResult(o => o, o => default(T));
       
        }
        public CommandParseResult(string Result, string RawOptions)
        {
            this.Result = Result;
            this.RawOptions = RawOptions; 
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
