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
        public static string Extention = ".txt";
        public static string FileIn = AppSettings.BasePath + "_input.txt";
        public static string FileOut = AppSettings.BasePath + "_output.txt";
        public static string Cache = AppSettings.BasePath + "_cache.txt";

        public static string BasePath = ConfigurationManager.AppSettings["BasePath"].ToString();
        public static string SourceDir = ConfigurationManager.AppSettings["SourceDir"].ToString();
        public static string DestDir = ConfigurationManager.AppSettings["DestDir"].ToString(); 
        
    }
    public static class Placeholder
    {
        public static string Basepath = "[basepath]";
        public static string Index = "[index]";
        public static string EXT = "[ext]";
    }
}
