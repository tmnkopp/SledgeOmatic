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
        public void TypeEnumerator_Enumerates()
        { 
            string typename = "Compiler.Models.Invoice";
            //TODO: Unit test this
            Assert.IsNotNull(typename);
        } 
    }
}
