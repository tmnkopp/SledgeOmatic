using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Compilers
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
        public CompilationBuilder<T> ContentCompilation(List<ICompiler> ContentCompilation)
        {
            compiler.ContentCompilation = ContentCompilation;
            return this;
        }
        public CompilationBuilder<T> FilenameCompilation(List<ICompiler> FilenameCompilation)
        {
            compiler.FilenameCompilation = FilenameCompilation;
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
