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

            using (StreamWriter w = File.AppendText($"{AppSettings.BasePath }\\unittest.sql"))
            {
            }
            FileWriter fw = new FileWriter($"{AppSettings.BasePath }\\unittest.sql"); 
            StringBuilder sb = new StringBuilder(); 
            sb.Append(" DECLARE @KVTABLE TABLE(K NVARCHAR(15), V NVARCHAR(255)) \n");
            sb.Append(" INSERT INTO @KVTABLE(K, V) VALUES('[UNITTEST]', 'passed') , ('[DATE]', CONVERT(NVARCHAR(25), GETDATE())) \n");
            sb.Append(" SELECT * FROM @KVTABLE \n");
            fw.Write(sb.ToString());
        }
    }
}