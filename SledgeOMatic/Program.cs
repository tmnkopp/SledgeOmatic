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
using SOM.Procedures.Data;

namespace SOM
{
    class Program
    {
        static void Main(string[] args)
        {
            DBColumnConverter ColumnConverter = new DBColumnConverter(
                "fsma_questions", 
                new NGMapper()
            );

            string result = ColumnConverter.Execute("[fields]");
            Console.Write("RESULT: " + result);
            Console.Read(); 

        }
    }
}

/*
 ,
                (r, p1) => r.Replace("[fields]", p1) 
 */
