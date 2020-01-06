using SOM.Procedures; 
using SOM.Data;
using SOM.IO;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using System.IO; 
using HtmlAgilityPack;
using System.Data.OleDb;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SOM
{
    class Program
    {
        static void Main(string[] args)
        {
            string root = @"C:\temp\_input.txt"; 
            string content;
            content = new FileReader(root).Read().ToString();
            content = new SqlKeyValCompile(@"C:\temp\unittest.sql").Execute(content);

            Cache.Write(content);
            Cache.CacheEdit();

            //Console.WriteLine("Hello World!");
            //Console.ReadKey();

        }
    }
}
