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
        private string _filename; 
        public string _basepath
        { 
            get => Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
        }
        public string _cachepath
        {
            get => $"{this._basepath}_cache.txt";
        }
        public FileWriter()
        { 
            this._filename = _cachepath;
        }
        public FileWriter(string Path)
        { 
            _filename = Path;
            _filename = _filename.Replace("~", _basepath).Trim();
        }
        public void Write(string writeme , bool Create)
        { 
            if (Create) 
                using (StreamWriter w = File.AppendText($"{_filename}")) { } 
            Write(writeme); 
        }
        public void Write(string writeme)
        {
            _filename = _filename.Replace("\\\\", "\\");
            try
            { 
                File.WriteAllText($"{_filename}", writeme, Encoding.Unicode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\n{_filename} {ex.Message}\n\n");
                throw new Exception(); 
            } 
        }
    }
}
