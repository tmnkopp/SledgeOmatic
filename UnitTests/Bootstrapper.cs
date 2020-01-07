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
        public void BasePathCreated()
        {
            SOM.Bootstrapper.Run();
            DirectoryInfo DI = new DirectoryInfo($"{AppSettings.BasePath}");
            Assert.IsTrue(DI.Exists);
        }
       
    }
}
