using Microsoft.VisualStudio.TestTools.UnitTesting; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Linq;

namespace CoreTests
{ 
    [TestClass]
    public class Bootstrapper
    {
        [TestMethod]
        public void ArgsParser()
        {
            string str = "/p HttpContext /p 4" ;
   
            var result = (from s in new List<string>(Regex.Split(str.Trim(), @"/p ")) where !string.IsNullOrWhiteSpace(s) select s.Trim()).ToList();
            
            Console.WriteLine("");

        }
    }
 
 
  
}
