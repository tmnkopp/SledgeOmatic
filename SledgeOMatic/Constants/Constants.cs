using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM 
{
    public static class AppSettings
    {
        public static string ProcAssembly = ConfigurationManager.AppSettings["ProcAssembly"].ToString();
        public static string BasePath = ConfigurationManager.AppSettings["BasePath"].ToString();
        public static string Extention = ConfigurationManager.AppSettings["Extention"].ToString(); 
        public static string FileIn = ConfigurationManager.AppSettings["FileIn"].ToString();
        public static string FileOut = ConfigurationManager.AppSettings["FileOut"].ToString();   
    }
    public static class Placeholders
    {
        public static string Dir = "[dir]";
        public static string Index = "[index]";
        public static string EXT = "[ext]";
    }
}
