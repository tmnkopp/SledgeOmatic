using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Data;
using Microsoft.Extensions.Configuration;

namespace SOM.Compilers 
{
    public enum CompileMode {
        Debug, Cache, Commit 
    }

    public interface ICompiler
    {
        public string Source { get; set; }
        public string Dest { get; set; } 
        public string FileFilter { get; set; }

        public CompileMode CompileMode { get; set; }
        public List<IInterpreter> ContentCompilers { get; set; }
        public List<IInterpreter> FilenameCompilers { get; set; }
        
        Func<string, string> ContentFormatter { set; }
        Func<string, string> FileNameFormatter { set; } 
        event EventHandler<CompilerEventArgs> OnCompiled;
        event EventHandler<CompilerEventArgs> OnPreCompile; 
        void Compile();
    }
 
    public class Compiler : ICompiler
    {
        #region Props
        public string Source { get; set; }
        public string Dest { get; set; }
        private string _fileFilter = null;
        public string FileFilter
        {
            get
            {
                return _fileFilter ?? Source.Split("\\").SkipWhile(s => !s.Contains("*")).FirstOrDefault() ?? "*";
            }
            set { _fileFilter = value; }
        }
        public List<IInterpreter> ContentCompilers { get; set; }
        public List<IInterpreter> FilenameCompilers { get; set; }
        public CompileMode CompileMode { get; set; }
        #endregion

        #region Events
        public event EventHandler<CompilerEventArgs> OnPreCompile;
        public event EventHandler<CompilerEventArgs> OnCompiled;
        protected virtual void PreCompile(CompilerEventArgs e)
        {
            Cache.Write("");
            OnPreCompile?.Invoke(this, e);
        }
        protected virtual void Compiled(CompilerEventArgs e)
        {
            OnCompiled?.Invoke(this, e);
        }
        #endregion

        #region Formatters
        private Func<string, string> _ContentFormatter = (c) => (c);
        public Func<string, string> ContentFormatter
        {
            set { _ContentFormatter = value; }
        }
        private Func<string, string> _FileNameFormatter = (c) => (c);
        public Func<string, string> FileNameFormatter
        {
            set { _FileNameFormatter = value; }
        }
        #endregion

        #region CTOR
        public Compiler()
        { 
            ContentCompilers = new List<IInterpreter>();
            FilenameCompilers = new List<IInterpreter>();
        }
        #endregion

        public virtual void Compile()
        {
            var args = new CompilerEventArgs(Source, Dest); 
            PreCompile(args);
            DirectoryInfo DI = new DirectoryInfo($"{Source}");
            foreach (var file in DI.GetFiles(FileFilter, SearchOption.TopDirectoryOnly))
            {
                var CompiledContent = CompileContent(Reader.Read(file.FullName));
                var CompiledFileName = CompileFileName(file.Name);
                CommitFile(CompiledContent, $"{Dest}\\{CompiledFileName}");
            }
            Compiled(args); 
        }
        protected virtual string CompileContent(string content)
        {
            foreach (IInterpreter proc in ContentCompilers)
            {
                content = proc.Interpret(content);
            }
            content = _ContentFormatter(content);
            return content;
        }
        protected virtual string CompileFileName(string Filename)
        {
            string newFileName = Filename;
            foreach (IInterpreter proc in FilenameCompilers)
                newFileName = proc.Interpret(newFileName).RemoveWhiteAndBreaks();
            return _FileNameFormatter(Filename);
        }
        private void CommitFile(string Content, string FileName)
        {
            if (CompileMode == CompileMode.Commit)
                new FileWriter($"{FileName}").Write(Content);
            if (CompileMode == CompileMode.Debug)
                Cache.Append($"\n\n som! -p {FileName} \n!som \n\n{Content}\n");
            if (CompileMode == CompileMode.Cache)
                Cache.Append($"{Content}\n");
        }
    }
}
