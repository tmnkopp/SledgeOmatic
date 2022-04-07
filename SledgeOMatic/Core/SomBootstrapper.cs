using SOM.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM
{
    public static class SomBootstrapper
    {
        public static void Run()
        {
            string basepath = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User); 
            if (string.IsNullOrEmpty(basepath)) 
                Environment.SetEnvironmentVariable("som", "c:\\_som\\", EnvironmentVariableTarget.User);
            basepath = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);

            DirectoryInfo DI;
            string[] dirnames = new string[] {
                basepath, $"{basepath}parse", $"{basepath}compile", $"{basepath}logs"
            }; 
            foreach (string dir in dirnames)
            {
                DI = new DirectoryInfo(dir);
                if (!DI.Exists) Directory.CreateDirectory(dir);
            } 
            string[] filenames = new string[] { "som.cache", "somexec.ps1" }; 
            foreach (string filename in filenames)
            {
                using (StreamWriter w = File.AppendText($"{basepath}\\{filename}"))  {  }
            } 
        }
    }
}