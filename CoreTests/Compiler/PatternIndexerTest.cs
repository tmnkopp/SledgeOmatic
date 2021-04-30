using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTests
{
    [TestClass]
    public class PatternIndexerTest
    {
        [TestMethod]
        public void PatternIndexer_Indexes()
        {
            string c = Reader.Read(@"C:\_som\_cache.txt"); 
            c = new PatternIndexer("QGroup, (\\d{1,2}),",  1).Compile(c); 
            //c = new PatternIndexer("(\\d{5}), @FormName", 22800).Compile(c); 
            Cache.Inspect(c); 

        }
    }

}
