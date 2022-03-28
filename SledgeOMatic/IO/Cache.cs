using System.Configuration;
using System.Diagnostics; 

namespace SOM.IO
{
    public static class Cache
    { 
        public static string Read() { 
            return Reader.Read(AppSettings.Cache);
        }
        public static void Write(string content)
        { 
            FileWriter w = new FileWriter(AppSettings.Cache);
            w.Write(content);
        }
        public static void WriteLine(string content)
        {
            Cache.Append($"{content}\n");
        }
        public static void Debug(string content)
        {
            Cache.Append($"\ndebug: {content}");
        }
        public static void Append(string content)
        {
            FileWriter w = new FileWriter(AppSettings.Cache);
            w.Write(Cache.Read() + content);
        }
        public static void Inspect()
        {
            Cache.CacheEdit();
        }
        public static void Inspect(string content)
        {
            Cache.Write(content);
            Cache.CacheEdit();
        } 
        public static void CacheEdit() {
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Users\\Tim\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe";
            p.StartInfo.Arguments = AppSettings.Cache; 
            p.Start();
        }
    }
}
