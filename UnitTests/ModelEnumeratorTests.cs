using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.IO; 
namespace UnitTests
{
    [TestClass]
    public class ModelEnumeratorTests
    { 
        [TestMethod]
        public void TypeEnumerator()
        {
            string typename = "Compiler.Models.Invoice";
            Assert.IsNotNull(typename);
        } 
    }
}
