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
        public static string BasePath = ConfigurationManager.AppSettings["BasePath"].ToString(); 
        public static string SourceDir = ConfigurationManager.AppSettings["SourceDir"].ToString();         
        public static string DestDir = ConfigurationManager.AppSettings["DestDir"].ToString(); 
        public static string Cache = AppSettings.BasePath + "_cache.txt";    
    }
    public static class Placeholder
    {
        public static string Basepath = "~"; 
        public static string SRC = "%src%";
        public static string DEST = "%dest%";  
    }
}
