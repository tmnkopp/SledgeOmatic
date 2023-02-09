using Microsoft.Extensions.Configuration;
using Serilog;
using SOM.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text; 
namespace SOM.IO
{
    public class CacheService: ICacheService
    { 
        private readonly IConfiguration config;
        private readonly ILogger logger;
        private string CachePath;
        private string CodeViewer;
        private string BasePath;
        public CacheService(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
            this.BasePath = config.GetSection("AppSettings:BasePath").Value;
            this.CachePath = (config.GetSection("AppSettings:CachePath").Value ?? "~cache.som").Replace("~", BasePath); 
            this.CodeViewer = config.GetSection("AppSettings:CodeViewer").Value ?? "notepad.exe";
        }
        public string Read(){
            using (TextReader tr = File.OpenText(this.CachePath))
                return tr.ReadToEnd();
        }
        public void Write(string content) => File.WriteAllText(this.CachePath, content, Encoding.UTF8);
        public void Append(string content) => File.WriteAllText(this.CachePath, $"{this.Read()}{content}", Encoding.UTF8);  
        public void Inspect()
        { 
            Process p = new Process();
            p.StartInfo.FileName = this.CodeViewer;
            p.StartInfo.Arguments = this.CachePath;
            p.Start();
        }
    }
}
