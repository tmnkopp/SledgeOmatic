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
        private string _filename = AppSettings.Cache;
        private string _basepath = AppSettings.BasePath; 
        public FileReader( )
        { 
        }
        public FileReader(string filename)
        {
            _filename = filename;   
        }
        public string Read()
        {
            _filename = _filename.Replace(Placeholder.Basepath, _basepath).Trim(); 
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
