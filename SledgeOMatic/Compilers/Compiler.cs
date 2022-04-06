﻿using SOM.Extentions; 
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
        private string src = "~"; 
        public string Source
        {
            get => src.Replace("~", somContext.Config.GetSection("AppSettings:BasePath").Value);
            set { src = value; }
        } 
        private string dst = "~";
        public string Dest
        {
            get => dst.Replace("~", somContext.Config.GetSection("AppSettings:BasePath").Value);
            set { dst = value; }
        } 
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
        private readonly ISomContext somContext;
        public Compiler(ISomContext somContext)
        {
            this.somContext = somContext;
            ContentCompilers = new List<ICompilable>();
            FilenameCompilers = new List<ICompilable>();
            somContext.Cache.Write("");
        }
        #endregion

        #region Methods  
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
             
            foreach (FileInfo file in DI.GetFiles(FileFilter, SearchOption.AllDirectories))
            {
                string content = "";
                using (TextReader tr = File.OpenText(file.FullName))
                    content = tr.ReadToEnd();
                 
                content = ContentPreFormatter(content);
                content = CompileContent(content);
                content = ContentPostFormatter(content);
                var CompiledFileName = CompileFileName(file.Name);

                args.File = file;
                args.CompiledFileName = CompiledFileName;
                args.ContentCompiled = content;
                Compiling(args);

                string SavePath = file.FullName
                    .Replace(Source, Dest)
                    .Replace(file.Name, CompiledFileName);

                string SaveDir = SavePath.Replace(CompiledFileName, "");
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);
                 
                CommitFile(content, $"{SavePath}"); 
            }
            args = new CompilerEventArgs(Source, Dest);
            Compiled(args); 
        }
        protected virtual string CompileContent(string content)
        {
            somContext.Content = content;
            foreach (ICompilable proc in ContentCompilers) {
                this.somContext.Content = proc.Compile(somContext);
            }     
            return _ContentPostFormatter(this.somContext.Content);
        }
        protected virtual string CompileFileName(string Filename)
        {
            somContext.Content = Filename;
            foreach (ICompilable proc in FilenameCompilers)
                this.somContext.Content = proc.Compile(somContext).RemoveWhiteAndBreaks();
            return _FileNameFormatter(this.somContext.Content);
        }
        private void CommitFile(string Content, string FileName)
        {
            this.somContext.Logger.Information($"FileName: {FileName}");

            if (CompileMode == CompileMode.Commit){ 
                File.WriteAllText($"{FileName}", Content, Encoding.Unicode); 
            }
            if (CompileMode == CompileMode.Debug){ 
                this.somContext.Logger.Debug($"FileName: {FileName}");
                this.somContext.Cache.Append($"\n\n som! -p {FileName} \n!som \n\n{Content}\n");
             }
                
            if (CompileMode == CompileMode.Cache){
                this.somContext.Cache.Append($"{Content}\n");
            }      
        }
        #endregion
    }
}
