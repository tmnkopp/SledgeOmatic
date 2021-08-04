using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text; 
namespace CoreTests
{ 
    [TestClass]
    public class FsysTests
    {
        [TestMethod]
        public void DestDirCreated()
        {
            List<string> lst = new List<string>();
            DirectoryInfo DI = new DirectoryInfo(@"C:\_som\_src\_compile\SAOP");
            foreach (FileInfo file in DI.GetFiles("", SearchOption.TopDirectoryOnly))
            {
                lst.Add(file.FullName);  
            }
            Assert.IsNotNull(lst);
        } 
    }
}
