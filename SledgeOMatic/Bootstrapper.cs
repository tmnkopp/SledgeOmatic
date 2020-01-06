using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            DirectoryInfo DI = new DirectoryInfo(AppSettings.BasePath);
            if (!DI.Exists)
            {
                Directory.CreateDirectory(AppSettings.BasePath);
            }
             
            string[] filenames = new string[] { "_cache", "_input", "_output"  }; 
            foreach (string filename in filenames)
            {
                using (StreamWriter w = File.AppendText($"{AppSettings.BasePath }\\{filename}{AppSettings.Extention}"))
                {
                }
            }
            
            foreach (var file in DI.GetFiles("$*"))
                file.Delete();
            foreach (var file in DI.GetFiles("_parsed_*"))
                file.Delete();
            //
            //DI.CreateSubdirectory();


        }
    }
}