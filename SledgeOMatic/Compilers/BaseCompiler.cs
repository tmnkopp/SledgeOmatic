using SOM.Extentions;
using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Compilers 
{
    public enum CompileMode
    {
        Debug,
        Cache,
        Commit,
        ForceCommit
    }
    public class AppSettingsCompiler : BaseCompiler
    {
        public AppSettingsCompiler()
        {
            Source = AppSettings.SourceDir;
            Dest = AppSettings.DestDir;
            FileFilter = "*";
        }
        public override void Compile()
        { 
            base.Compile(); 
        }
    }

    public abstract class BaseCompiler
    { 
        public string Source = "";
        public string Dest = "";
        public string FileFilter = "*";
        public List<ICompiler> ContentCompilers;
        public List<ICompiler> FilenameCompilers;
        public CompileMode CompileMode;
        
        private string content = "";
        public BaseCompiler()
        {
            ContentCompilers = new List<ICompiler>();
            FilenameCompilers = new List<ICompiler>();
        }
        public virtual void Compile()
        {
            DirectoryInfo DI = new DirectoryInfo($"{Source}");
            foreach (var file in DI.GetFiles(FileFilter, SearchOption.TopDirectoryOnly))
            {
                content = new FileReader(file.FullName).Read().ToString();
                foreach (ICompiler proc in ContentCompilers)
                    content = proc.Compile(content);

                string newFileName = file.Name;
                if (FilenameCompilers != null)
                {
                    foreach (ICompiler proc in FilenameCompilers)
                        newFileName = proc.Compile(newFileName).RemoveWhiteAndBreaks();
                } 
                CommitFile(content, $"{Dest}\\{newFileName}"); 
            } 
        }
        private void CommitFile(string Content, string FileName)
        {
            if (CompileMode == CompileMode.ForceCommit)
                FileSys.Utils.DirectoryCreator(FileName, AppSettings.BasePath);

            if (CompileMode == CompileMode.Commit)
            {
                FileWriter fw = new FileWriter($"{FileName}");
                fw.Write(Content);
            }
            if (CompileMode == CompileMode.Debug)
            {
                Cache.Append($"[{FileName}]\n{Content}\n");
            }             
            if (CompileMode == CompileMode.Cache)
            {
                Cache.Append($"{Content}\n");
            } 
        }
    }
}
