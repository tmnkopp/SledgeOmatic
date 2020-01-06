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
        private string _filename = ConfigurationManager.AppSettings["FileIn"].ToString();
        private string _basepath = ConfigurationManager.AppSettings["BasePath"].ToString();
        public FileReader( )
        { 
        }
        public FileReader(string filename)
        {
            _filename = filename;   
        }
        public string Read()
        {
            _filename = String.Format("{0}", _filename.Replace(Placeholders.Basepath, _basepath));
            using (TextReader tr = File.OpenText(_filename))
            {
                return tr.ReadToEnd();
            }
        } 
    } 
}
