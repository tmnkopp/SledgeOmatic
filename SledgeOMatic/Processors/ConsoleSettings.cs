using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SOM 
{
    public class ConsoleSettings
    {
        public List<string> Paths { get; set; }
    }
    public class ConsoleSettingsProvider
    {
        public ConsoleSettings Provide()
        {
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            return JObject.Parse(File.ReadAllText(path)).ToObject<ConsoleSettings>();
        }
    }
}
