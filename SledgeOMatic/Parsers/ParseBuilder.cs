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
        public ParseBuilder<T> Init(string Path)
        {
            this.Init();
            Parser.Path = Path; 
            return this;
        }
        public ParseBuilder<T> Path(string Path)
        {
            Parser.Path = Path;
            return this;
        }  
        public ParseBuilder<T> Find(string Find, int ContextLines)
        {
            Parser.Compilers.Add(new LineExtractor(Find, ContextLines));
            return this;
        }
        public ParseBuilder<T> Compilers(List<ICompiler> Compilers)
        {
            Parser.Compilers.AddRange(Compilers);
            return this;
        } 
        public ParseBuilder<T> AddCompiler(ICompiler Compiler)
        {
            Parser.Compilers.Add(Compiler);
            return this;
        }
        public ParseBuilder<T> ParseTo(IWriter Writer, Func<string, string> Formatter)
        {
            Parser.Fromatter = Formatter;
            Parser.ParseTo(Writer);
            return this;
        }
        public ParseBuilder<T> Parse()
        {
            Parser.Parse();
            return this;
        } 
    }
}
