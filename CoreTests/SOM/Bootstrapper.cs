using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class Bootstrapper
    {
        [TestMethod]
        public void DestDirCreated()
        { 
            SOM.Bootstrapper.Run();
            DirectoryInfo DI = new DirectoryInfo($"{AppSettings.DestDir}");
            Assert.IsTrue(DI.Exists);
        }
       
    }
}
