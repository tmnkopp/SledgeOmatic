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
  
        private string _filename = AppSettings.FileIn;
        private string _basepath = AppSettings.BasePath;
        public FileWriter()
        {
        }
        public FileWriter(string Path)
        {
            _filename = Path;
        }
        public void Write(string writeme)
        { 
            try
            {  
                File.WriteAllText($"{_filename}", writeme);
            }
            catch (Exception)
            {
                Console.WriteLine($"\n\nbad path {_filename}\n\n");
                throw new InvalidProgramException();
                
            }
            
        }
    }
}
