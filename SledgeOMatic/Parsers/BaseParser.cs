using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Parsers
{
 
    public abstract class BaseParser
    {
        public string DirSource = "";
        public string Find = "";
        public string FileFilter = "*.*";
        public string CurrentFilePath = "";
        public List<string> ExcludeList = new List<string>();
        public StringBuilder FindingResults = new StringBuilder();
        public StringBuilder FilesFound = new StringBuilder();

        public virtual bool IsFound(string content)
        {
            return content.Contains(Find);
        }
        public virtual void Parse()
        {
            SearchDir();
            Display();
        }
        public virtual void Display()
        {
            Cache.Write("");
            Cache.Append($"{FilesFound.ToString()}\n{FindingResults.ToString()}");
            Cache.CacheEdit(); 
        }
        public virtual string ParseFinding(string content)
        {
            string result = new LineExtractor(Find , 3, true).Execute(content); 
            return $"{CurrentFilePath}\n{result}";
        }

        public void SearchDir()
        {
            int cnt = 0;
            DirectoryInfo DI = new DirectoryInfo($"{this.DirSource}");
            foreach (var file in DI.GetFiles(FileFilter, SearchOption.AllDirectories))
            {
                if (!IsPathExcluded(file.FullName))
                {
                    FileReader r = new FileReader(file.FullName);
                    string content = r.Read().Replace("\t", "").Replace("  ", " ");
                    if (IsFound(content))
                    {
                        this.CurrentFilePath = file.FullName;
                        string result = ParseFinding(content);
                        FindingResults.Append($"{result}\n");
                        FilesFound.Append($"{cnt.ToString()} : {file.FullName}\n");
                        cnt++;
                    }
                }
            }
        }
        private bool IsPathExcluded(string FullFilePath)
        {
            bool ret = false;
            foreach (string exclude in ExcludeList)
            {
                if (FullFilePath.Contains(exclude))
                    return true;
            }
            return ret;
        }
    }
}
