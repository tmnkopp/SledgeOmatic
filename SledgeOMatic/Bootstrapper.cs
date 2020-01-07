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
                Directory.CreateDirectory(AppSettings.BasePath + "_src");
                Directory.CreateDirectory(AppSettings.BasePath + "_src\\" + "_compiled");
            }  
            string[] filenames = new string[] { "_cache", "_input", "_output"  }; 
            foreach (string filename in filenames)
            {
                using (StreamWriter w = File.AppendText($"{AppSettings.BasePath }\\{filename}{AppSettings.Extention}"))
                {
                }
            } 
        }
    }
}