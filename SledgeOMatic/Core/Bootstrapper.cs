using SOM.IO;
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
            string envar = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(envar))
            {
                Environment.SetEnvironmentVariable("som", "c:\\_som\\", EnvironmentVariableTarget.User); 
            }

            DirectoryInfo DI;
            string[] dirnames = new string[] {
                AppSettings.BasePath,  AppSettings.SourceDir, AppSettings.DestDir 
            }; 
            foreach (string dir in dirnames)
            {
                DI = new DirectoryInfo(dir);
                if (!DI.Exists)  Directory.CreateDirectory(dir);
            }
    
            string[] filenames = new string[] { "_cache.txt"  }; 
            foreach (string filename in filenames)
            {
                using (StreamWriter w = File.AppendText($"{AppSettings.BasePath }\\{filename}"))
                {
                }
            } 
        }
    }
}