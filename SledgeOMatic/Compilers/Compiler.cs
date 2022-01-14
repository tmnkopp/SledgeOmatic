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
                return _fileFilter ?? Dest.Split("\\").SkipWhile(s => !s.Contains("*")).FirstOrDefault() ?? "*";
            }
            set { _fileFilter = value; }
        }
        public List<ICompilable> ContentCompilers { get; set; }
        public List<ICompilable> FilenameCompilers { get; set; }
        public CompileMode CompileMode { get; set; }
        #endregion

        #region Events 
        public event EventHandler<CompilerEventArgs> OnPreCompile; 
        protected virtual void PreCompile(CompilerEventArgs e)
        { 
            OnPreCompile?.Invoke(this, e);
        }
        public event EventHandler<CompilerEventArgs> OnCompiling;
        protected virtual void Compiling(CompilerEventArgs e)
        { 
            OnCompiling?.Invoke(this, e);
        }
        public event EventHandler<CompilerEventArgs> OnCompiled;
        protected virtual void Compiled(CompilerEventArgs e)
        {
            OnCompiled?.Invoke(this, e);
            string onCompiledPs = $"{this.Source}\\OnCompiled.ps1";
            if (this.CompileMode == CompileMode.Commit && File.Exists(onCompiledPs))
            {
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = @"powershell.exe",
                    Arguments = $"& '{onCompiledPs}'",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process process = new Process();
                process.StartInfo = psi;
                process.Start();
            }
        }
        #endregion

        #region Formatters
        private Func<string, string> _ContentPreFormatter = (c) => (c);
        public Func<string, string> ContentPreFormatter
        {
            set { _ContentPreFormatter = value; }
            private get { return _ContentPreFormatter; }
        }

        private Func<string, string> _ContentPostFormatter = (c) => (c);
        public Func<string, string> ContentPostFormatter
        {
            set { _ContentPostFormatter = value; }
            private get { return _ContentPostFormatter; }
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
            ContentCompilers = new List<ICompilable>();
            FilenameCompilers = new List<ICompilable>();
            Cache.Write("");
        }
        #endregion

        #region Methods 
        public void Compile(string FileFilter, string Source, string Dest)
        {
            this.FileFilter = FileFilter;
            this.Source = Source;
            this.Dest = Dest; 
            Compile();
        }
        public void Compile(string FileFilter, string Dest)
        {
            this.Dest = Dest;
            this.FileFilter = FileFilter;
            Compile();
        }
        public void Compile(string FileFilter)
        {
            this.FileFilter = FileFilter;
            Compile();
        }
        public virtual void Compile()
        {
            var args = new CompilerEventArgs(Source, Dest); 
            PreCompile(args);
            DirectoryInfo DI = new DirectoryInfo($"{Source}");
            foreach (FileInfo file in DI.GetFiles(FileFilter, SearchOption.TopDirectoryOnly))
            {
                string content = Reader.Read(file.FullName);
                content = ContentPreFormatter(content);
                content = CompileContent(content);
                content = ContentPostFormatter(content);
                var CompiledFileName = CompileFileName(file.Name);

                args.File = file;
                args.CompiledFileName = CompiledFileName;
                args.ContentCompiled = content;
                Compiling(args);
 
                CommitFile(content, $"{Dest}\\{CompiledFileName}");
            }
            args = new CompilerEventArgs(Source, Dest);
            Compiled(args); 
        }
        protected virtual string CompileContent(string content)
        {
            foreach (ICompilable proc in ContentCompilers) 
                content = proc.Compile(content); 
            return _ContentPostFormatter(content);
        }
        protected virtual string CompileFileName(string Filename)
        { 
            foreach (ICompilable proc in FilenameCompilers)
                Filename = proc.Compile(Filename).RemoveWhiteAndBreaks();
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
        #endregion
    }
}
