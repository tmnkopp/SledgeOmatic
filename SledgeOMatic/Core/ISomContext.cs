using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SOM
{
    public interface ISomContext
    {
        IConfiguration Config { get;   }
        ILogger Logger { get;  }
        string Content { get; set; }
    }
    public class SomContext: ISomContext
    {
        public IConfiguration Config { get => this.config ; }
        public ILogger Logger { get => this.logger;  }
        public string Content { get; set; }
 
        private readonly IConfiguration config;
        private readonly ILogger logger;
        public SomContext(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger; 
        }
    }
}
