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
            string[] filenames = new string[] { "_cache", "_input", "_output", "_compile", "_compiled", "_parse", "_parsed" };

            foreach (string filename in filenames)
            {
                using (StreamWriter w = File.AppendText($"{AppSettings.BasePath }\\{filename}{AppSettings.Extention}"))
                {
                }
            }
            DirectoryInfo DI = new DirectoryInfo(AppSettings.BasePath);
            foreach (var file in DI.GetFiles("$*"))
                file.Delete();
            foreach (var file in DI.GetFiles("_parsed_*"))
                file.Delete();
            //DI.Exists()
            //DI.CreateSubdirectory();


        }
    }
}