using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SOM.Compilers
{ 
    public interface ICompiler
    {
        #region Props 
        public string Source { get; set; }
        public string Dest { get; set; }
        public string FileFilter { get; set; }
        public CompileMode CompileMode { get; set; }
        public List<ICompilable> ContentCompilers { get; set; }
        public List<ICompilable> FilenameCompilers { get; set; }
        #endregion

        #region Func

        Func<string, string> ContentFormatter { set; }
        Func<string, string> FileNameFormatter { set; }

        #endregion

        #region Events

        event EventHandler<CompilerEventArgs> OnCompiled;
        event EventHandler<CompilerEventArgs> OnPreCompile;
        event EventHandler<CompilerEventArgs> OnCompiling;

        #endregion

        #region Methods

        void Compile();

        #endregion

    }
}
