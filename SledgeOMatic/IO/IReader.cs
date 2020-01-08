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
    public class FileReader : IReader
    {
        private string _filename = AppSettings.FileIn;
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
            _filename = String.Format("{0}", _filename.Replace(Placeholder.Basepath, _basepath));
            using (TextReader tr = File.OpenText(_filename))
            {
                return tr.ReadToEnd();
            }
        } 
    } 
}
