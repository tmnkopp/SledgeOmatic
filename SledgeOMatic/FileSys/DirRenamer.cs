 
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
    public class DirRenamer: IDirRenamer
    {
        private string _dir; 
        public DirRenamer(string Dir)
        {
            _dir = Dir; 
        }  
        public void Rename(List<IProcedure> RenameProcedures) {
            DirectoryInfo DI = new DirectoryInfo($"{_dir}");
            
            foreach (FileInfo file in DI.GetFiles("*", SearchOption.AllDirectories))
            {
                string newname = file.Name;
                foreach (IProcedure _proc in RenameProcedures) 
                     newname = _proc.Execute(newname); 
                file.MoveTo($"{_dir}\\{newname}") ;
            }
        } 
    }
}
