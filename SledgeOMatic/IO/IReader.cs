using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.IO
{
    public interface IReader
    {
        string Read();
    }
    public static class Reader {
        public static string Read(string FileName)
        {
            FileName = FileName.Replace(@"\\", @"\");
            FileReader r = new FileReader(FileName);
            return r.Read();
        }
    }
    public class FileReader : IReader
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
        public FileReader( )
        { 
            this._filename = _cachepath;
        }
        public FileReader(string filename)
        {
            _filename = filename;   
        }
        public string Read()
        {
            _filename = _filename.Replace("~", _basepath).Trim(); 
            try {
                using (TextReader tr = File.OpenText(_filename)) 
                    return tr.ReadToEnd(); 
            }
            catch (Exception e)  {
                Console.WriteLine($"SOM.IO FileReader Read(): {_filename} {e.StackTrace} {e.Source} {e.Message}"); 
            }
            return ""; 
        } 
    } 
}
