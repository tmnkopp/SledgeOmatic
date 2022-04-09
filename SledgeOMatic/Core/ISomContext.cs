using Microsoft.Extensions.Configuration;
 
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SOM
{
    public interface ISomContext
    {
        IConfiguration Config { get;   }
        ILogger Logger { get;  }
        ICacheService Cache { get;  }
        string Content { get; set; }
        ISomOptions Options { get; set; }
        string BasePath { get; }
    }
    public class SomContext: ISomContext
    {
        public IConfiguration Config { get => this.config ; }
        public ILogger Logger { get => this.logger;  }
        public ICacheService Cache { get => this.cacheService;  }
        public ISomOptions Options { get; set; }
        public string Content { get; set; } = ""; 
        public string BasePath => this.config.GetSection("AppSettings:BasePath").Value;

        private readonly IConfiguration config;
        private readonly ILogger logger;
        private readonly ICacheService cacheService;
        public SomContext(IConfiguration config, ILogger logger, ICacheService CacheService)
        {
            this.config = config;
            this.logger = logger; 
            this.cacheService = CacheService; 
        }
    }
}
