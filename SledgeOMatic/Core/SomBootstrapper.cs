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
        public static void Run(ConfigOptions o)
        {
            DirectoryInfo DI;
            if (!string.IsNullOrWhiteSpace(o.Path))
            {
                DI = new DirectoryInfo(o.Path);
                if (!DI.Exists) Directory.CreateDirectory(o.Path);
                Environment.SetEnvironmentVariable("som", o.Path, EnvironmentVariableTarget.User);
            }  
            string basepath = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User).Trim(); 
            if (string.IsNullOrWhiteSpace(basepath))
            {
                string err = "\nProvide a path for som:\n";
                err += " -p \" c:\\basepath_to_som  \" ";
                Console.WriteLine(err);
                return;
            }
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