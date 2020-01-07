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
        private List<IProcedure> _Procedures;
        public DirCompiler(List<IProcedure> Procedures)
        {
            _Procedures = Procedures;
        } 
        private string _SourceDir = AppSettings.SourceDir;
        public string SourceDir
        {
            get { return _SourceDir; }
            set { _SourceDir = value; }
        }
        private string _DestDir = AppSettings.DestDir;
        public string DestDir
        {
            get {
                if (_DestDir == "")
                    _DestDir = AppSettings.BasePath + "_compiled";
                return _DestDir;
            }
            set { _DestDir = value; }
        }
        public void Compile( ) {
            DirectoryInfo DI = new DirectoryInfo($"{SourceDir}");
            
            foreach (var file in DI.GetFiles("*", SearchOption.AllDirectories))
            {
                string content = new FileReader(file.FullName).Read().ToString();
                foreach (IProcedure proc  in _Procedures)
                    content = proc.Execute(content);

                FileWriter fw = new FileWriter($"{DestDir}\\{file.Name}");
                fw.Write(content);
            }
        }
    }
}
