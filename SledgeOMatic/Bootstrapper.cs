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
            DirectoryInfo DI;
            string[] dirnames = new string[] {
                AppSettings.BasePath,
                AppSettings.SourceDir,
                AppSettings.DestDir
            };

            foreach (string dir in dirnames)
            {
                DI = new DirectoryInfo(dir);
                if (!DI.Exists)
                    Directory.CreateDirectory(dir);
            }
    
            string[] filenames = new string[] { "_cache", "_input", "_output"  }; 
            foreach (string filename in filenames)
            {
                using (StreamWriter w = File.AppendText($"{AppSettings.BasePath }\\{filename}{AppSettings.Extention}"))
                {
                }
            }

            using (StreamWriter w = File.AppendText($"{AppSettings.BasePath }\\_unittest.sql"))
            {
            }
            FileWriter fw = new FileWriter($"{AppSettings.BasePath }\\_unittest.sql"); 
            StringBuilder sb = new StringBuilder(); 
            sb.Append(" DECLARE @KVTABLE TABLE(K NVARCHAR(15), V NVARCHAR(255)) \n");
            sb.Append(" INSERT INTO @KVTABLE(K, V) VALUES('[UNITTEST]', 'passed') , ('[DATE]', CONVERT(NVARCHAR(25), GETDATE())) \n");
            sb.Append(" SELECT * FROM @KVTABLE \n");
            fw.Write(sb.ToString());
        }
    }
}