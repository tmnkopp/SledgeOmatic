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
        public static string ProcAssembly = "SOM.ProcAssembly";
        public static string BasePath = ConfigurationManager.AppSettings["BasePath"].ToString();
        public static string Extention = ".txt"; 
        public static string FileIn = AppSettings.BasePath + ConfigurationManager.AppSettings["FileIn"].ToString();
        public static string FileOut = AppSettings.BasePath + ConfigurationManager.AppSettings["FileOut"].ToString();
        public static string Cache = AppSettings.BasePath + ConfigurationManager.AppSettings["Cache"].ToString();
    }
    public static class Placeholders
    {
        public static string Basepath = "[basepath]";
        public static string Index = "[index]";
        public static string EXT = "[ext]";
    }
}
