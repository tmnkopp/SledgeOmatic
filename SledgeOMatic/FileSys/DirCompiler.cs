using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM 
{
    public class DirCompiler
    {
        private string _SourceDir = AppSettings.SourceDir;
        public DirCompiler(string SourceDir)
        {
            _SourceDir = SourceDir;
        }  
        private string _DestDir = AppSettings.DestDir;
        public string DestDir
        {
            get { return _DestDir;  }
            set { _DestDir = value; }
        }
        public void Compile(List<IProcedure> CompileProcedures) {
            DirectoryInfo DI = new DirectoryInfo($"{_SourceDir}");
            
            foreach (var file in DI.GetFiles("*", SearchOption.AllDirectories))
            {
                string content = new FileReader(file.FullName).Read().ToString();
                foreach (IProcedure proc  in CompileProcedures)
                    content = proc.Execute(content);

                FileWriter fw = new FileWriter($"{DestDir}\\{file.Name}");
                fw.Write(content);
            }
        }
        public void Rename(List<IProcedure> RenameProcedures)
        {
            DirectoryInfo DI = new DirectoryInfo($"{DestDir}"); 
            foreach (var file in DI.GetFiles("*", SearchOption.AllDirectories))
            {
                string newname = file.Name;
                foreach (IProcedure proc in RenameProcedures)
                    newname = proc.Execute(newname);

                file.MoveTo($"{DestDir}\\{newname}");
            }
        }
    }
}
