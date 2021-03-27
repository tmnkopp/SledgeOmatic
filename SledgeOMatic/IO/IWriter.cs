using SOM.Extentions;
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
    public class FileWriter : IWriter
    { 
        private string _filename = AppSettings.Cache; 
        public FileWriter()
        {
        }
        public FileWriter(string Path)
        {
            _filename = Path;
        }
        public void Write(string writeme , bool Create)
        { 
            if (Create) 
                using (StreamWriter w = File.AppendText($"{_filename}")) { } 
            Write(writeme); 
        }
        public void Write(string writeme)
        { 
            try
            {
                do
                {
                    writeme = writeme.TrimTrailingNewline();
                } while (writeme.EndsWith("\n"));
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
