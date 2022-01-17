using SOM.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;

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
