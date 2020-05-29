using SOM.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Data
{
    public interface IProvider<T> {
        IEnumerable<T> SelectAll();
        string From { get; set; }
        string SearchPattern { get; set; } 
    }

    public class FilePathProvider : IProvider<string>
    {
        public string From { get; set; }
        private string _searchPattern = "";
        public string SearchPattern
        {
            get
            {
                if (_searchPattern == "")
                    _searchPattern = From.ReverseString().Split(new[] { '\\' })[0].ReverseString();
                return (_searchPattern == "") ? "*.*" : _searchPattern;
            }
            set { _searchPattern = value; }
        }
        public IEnumerable<string> SelectAll()
        {
            DirectoryInfo DI = new DirectoryInfo($"{this.From.Replace(SearchPattern, "")}");
            foreach (var file in DI.GetFiles(SearchPattern, SearchOption.AllDirectories))
            {
                yield return file.FullName;
            }
        }
    }
    public abstract class BaseProvider<T>
    { 
        public List<T> Items { get => this.Enumerate().ToList(); }
        public abstract IEnumerable<T> Enumerate(); 
    }
}
