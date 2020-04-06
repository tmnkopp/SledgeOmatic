using SOM.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.FileSys
{
    public static class Utils
    {
        public static void GenerateFiles(string DirectoryDefinition)
        {
            string[] filepaths = DirectoryDefinition.Split('\n');
            FileWriter w;
            foreach (string filepath in filepaths)
            {
                Utils.DirectoryCreator(filepath, AppSettings.BasePath); 
                w = new FileWriter($"{filepath}");
                w.Write($"{filepath}", true);
            }
        }
        public static void DirectoryCreator(string path, string basepath)
        {
            basepath = basepath.ToLower();
            path = path.ToLower(); 
            path = path.Replace(basepath, "");
            string[] subdirs = path.Split(new[] { "\\" }, StringSplitOptions.None);
            StringBuilder dir = new StringBuilder();
            dir.Append(basepath);
            foreach (string subdir in subdirs)
            {
                if (!subdir.Contains("."))
                {
                    dir.Append($"\\{subdir}");
                    DirectoryInfo DI = new DirectoryInfo(dir.ToString());
                    if (!DI.Exists)
                    {
                        DI.Create();
                    }
                }
    
            }
        } 
    }
}
