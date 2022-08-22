using System;
using System.Collections.Generic;
using System.Text;

namespace SOM.Compilers
{
    public class CompileDefinition 
    {
        public CompileDefinition()
        { 
        } 
        public List<CompilerDef> ContentCompilers { get; set; }
        public List<CompilerDef> FilenameCompilers { get; set; }
        public List<Compilation> Compilations { get; set; }
    
        public class CompilerDef
        {
            public string CompilerType { get; set; } 
            public List<object> Args { get; set; }
            public string Params { get; set; }
        }
        public class Compilation
        {
            public string FileFilter { get; set; }
            public string Source { get; set; }
            public string Dest { get; set; }
        }
    }
    public class YamlTuple<T1, T2>
    {
        public YamlTuple() { }

        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public static implicit operator YamlTuple<T1, T2>(Tuple<T1, T2> t)
        {
            return new YamlTuple<T1, T2>()
            {
                Item1 = t.Item1,
                Item2 = t.Item2
            };
        } 
        public static implicit operator Tuple<T1, T2>(YamlTuple<T1, T2> t)
        {
            return Tuple.Create(t.Item1, t.Item2);
        }
    } 
}
