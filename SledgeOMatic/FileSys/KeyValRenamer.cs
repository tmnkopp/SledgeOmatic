 
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
    public class KeyValRenamer: IDirRenamer
    {
        private string _dir;
        private IProcedure _proc;
        public KeyValRenamer(string Dir, IProcedure RenameProcedure)
        {
            _dir = Dir;
            _proc = RenameProcedure;
        }  
        public void Rename() {
            DirectoryInfo DI = new DirectoryInfo($"{_dir}");
            foreach (FileInfo file in DI.GetFiles("*", SearchOption.AllDirectories))
            {
                string newname = _proc.Execute(file.Name); 
                file.MoveTo($"{_dir}\\{newname}") ;
            }
        } 
    }
}
