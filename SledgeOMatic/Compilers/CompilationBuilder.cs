using SOM.Compilers;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public class CompilationBuilder<T> where T : BaseCompiler, new()
    {
        public T compiler = new T();
        public CompilationBuilder<T> Init()
        {
            compiler = new T();
            return this;
        }
        public CompilationBuilder<T> Source(string Source)
        {
            compiler.Source = Source;
            return this;
        }
        public CompilationBuilder<T> Dest(string Dest)
        {
            this.Dest(Dest, false);
            compiler.Dest = Dest;
            return this;
        }
        public CompilationBuilder<T> Dest(string DirDest, bool Create)
        {
            DirectoryInfo DI = new DirectoryInfo(DirDest);
            if (DI.Exists && Create && DirDest.Contains("$"))
            {
                foreach (FileInfo file in DI.GetFiles())
                    file.Delete();
            }
            if (!DI.Exists && Create)
            {
                Directory.CreateDirectory(DirDest);
            }
            compiler.Dest = DirDest;
            return this;
        }
        public CompilationBuilder<T> FileFilter(string FileFilter)
        {
            compiler.FileFilter = FileFilter;
            return this;
        }
        public CompilationBuilder<T> AddContentCompiler( IInterpreter Compiler )
        {
            compiler.ContentCompilers.Add(Compiler);
            return this;
        }
        public CompilationBuilder<T> ContentCompilers(List<IInterpreter> ContentCompilers)
        {
            compiler.ContentCompilers = ContentCompilers;
            return this;
        }
        public CompilationBuilder<T> FilenameCompilers(List<IInterpreter> FilenameCompilers)
        {
            compiler.FilenameCompilers = FilenameCompilers;
            return this;
        }
        public CompilationBuilder<T> CompileMode(CompileMode CompileMode)
        {
            compiler.CompileMode = CompileMode;
            return this;
        }
        public CompilationBuilder<T> Compile()
        {
            compiler.Compile();
            return this;
        }
    }
}
