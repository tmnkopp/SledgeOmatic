using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Parsers
{
    public class ParseBuilder<T> where T : BaseParser, new()
    {
        public T parser = new T();
        public ParseBuilder<T> Init()
        {
            parser = new T();
            return this;
        }
        public ParseBuilder<T> DirSource(string DirSource)
        {
            parser.DirSource = DirSource;
            return this;
        }
        public ParseBuilder<T> FileFilter(string FileFilter)
        {
            parser.FileFilter = FileFilter;
            return this;
        }
        public ParseBuilder<T> ExcludePath(string ExcludePath)
        {
            parser.ExcludeList.Add(ExcludePath);
            return this;
        }
        public ParseBuilder<T> Find(string Find)
        {
            parser.Find = Find;
            return this;
        }
        public ParseBuilder<T> ParseResultMode(ParseResultMode ParseResultMode)
        {
            parser.ParseResultMode = ParseResultMode;
            return this;
        }
        
        public ParseBuilder<T> Parse()
        {
            parser.Parse();
            return this;
        }
    }
}
