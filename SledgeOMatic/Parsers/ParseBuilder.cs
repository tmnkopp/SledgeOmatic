using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Parsers
{
    public class ParseBuilder<T> where T : BaseParser, new()
    {
        public T Parser = new T();
        public ParseBuilder<T> Init()
        {
            Parser = new T();
            return this;
        }
        public ParseBuilder<T> DirSource(string DirSource)
        {
            Parser.DirSource = DirSource;
            return this;
        }
        public ParseBuilder<T> FileFilter(string FileFilter)
        {
            Parser.FileFilter = FileFilter;
            return this;
        }
        public ParseBuilder<T> ExcludePath(string ExcludePath)
        {
            Parser.ExcludeList.Add(ExcludePath);
            return this;
        }
        public ParseBuilder<T> Find(string Find)
        {
            Parser.Find = Find;
            return this;
        }
        public ParseBuilder<T> Parsers(List<ICompiler> Parsers)
        {
            Parser.Parsers = Parsers;
            return this;
        }
        public ParseBuilder<T> ParseTo(IWriter Writer)
        {
            Parser.ParseTo(Writer);
            return this;
        }
        public ParseBuilder<T> Parse()
        {
            Parser.Parse();
            return this;
        }
        public override string ToString()
        {
            return Parser.ToString(); 
        }
    }
}
