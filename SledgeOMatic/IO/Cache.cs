using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.IO
{
    public static class Cache
    { 
        public static string Read() {
            string _filename = ConfigurationManager.AppSettings["Cache"].ToString();
            FileReader r = new FileReader(_filename);
            return r.Read();
        }
        public static void Write(string content)
        {
            string _filename = ConfigurationManager.AppSettings["Cache"].ToString();
            FileWriter w = new FileWriter(_filename);
            w.Write(content);
        }
        public static void Append(string content)
        {
            string _filename = ConfigurationManager.AppSettings["Cache"].ToString();
            FileWriter w = new FileWriter(_filename);
            w.Write(Cache.Read() + content);
        }
        public static void CacheEdit() {
            Process p = new Process();
            p.StartInfo.FileName = ConfigurationManager.AppSettings["CodeViewer"].ToString();
            p.StartInfo.Arguments = ConfigurationManager.AppSettings["Cache"].ToString();
            p.Start();
        }
    }
}
