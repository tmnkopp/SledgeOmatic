using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SOM.Core
{
    public interface ISomContext
    {
        IConfiguration config { get; set;  }
        ILogger logger { get; set;  }
    }
}
