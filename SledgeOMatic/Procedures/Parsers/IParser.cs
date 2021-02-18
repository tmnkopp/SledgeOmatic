using SOM.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;
namespace SOM.Procedures
{
    public enum ParseResultMode
    {
        Debug,  Verbose,  Default
    }
    public interface IParser
    {
        IEnumerable<string> Parse(string content);
        ParseResultMode ParseResultMode { get; set; }
    }
    public abstract class BaseParser
    {
        private string _content = "";
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        private ParseResultMode _ParseResultMode = ParseResultMode.Default;
        public ParseResultMode ParseResultMode
        { 
            set { _ParseResultMode = value; }
            get { return  _ParseResultMode; }
        } 
        public BaseParser()
        {
     
        } 
    }
}
