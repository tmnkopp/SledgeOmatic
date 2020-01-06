using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.IO
{
    public interface IWriter
    {
        void Write(string writeme);
    } 
    public class FileWriter :  IWriter
    {
        private string _path = ConfigurationManager.AppSettings["FileOut"].ToString();
        private string _basepath = ConfigurationManager.AppSettings["BasePath"].ToString();
        public FileWriter()
        {
        }
        public FileWriter(string Path)
        {
            _path = Path;
        }
        public void Write(string writeme)
        { 
            try
            { 
                Console.WriteLine($"write {_path}");
                File.WriteAllText($"{_path}", writeme);
            }
            catch (Exception)
            {
                Console.WriteLine($"\n\nbad path {_path}\n\n");
                throw new InvalidProgramException();
                
            }
            
        }
    }
}
