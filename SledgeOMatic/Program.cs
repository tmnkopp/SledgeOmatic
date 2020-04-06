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
using SOM.Compilers;

namespace SOM
{
    
    class Program
    {
        static void Main(string[] args)
        {
        
            CompilationBuilder<Compiler> compiler = new CompilationBuilder<Compiler>();
            FileWriter w = new FileWriter("c:\\temp\\dev\\_in.txt");
            w.Write($"public class [entity] \n{{\n\t[entity]() {{ \n\t}} \n [fields] \n}}" , true);

            w = new FileWriter("c:\\temp\\dev\\_form.txt");
            w.Write($"\n[fields]\n", true);

            Func<string, string, string> integrator = (c,r) => c.Replace("[fields]", r);
            Func<DBColumnDefinition, string> converter = (cd) => (string.Format( "this.{0} == form.{0};", cd.COLUMN_NAME));
            compiler.Init()
                .CompileMode(CompileMode.Commit)
                .Source("c:\\temp\\dev\\")
                .Dest("c:\\temp\\dev\\", true)
                .FileFilter("*_in.txt")
                .ContentCompilation(
                    new List<IProcedure> {
                        new ModelCompile(
                            "fsma_Questions",
                            new ccCustomConverter(converter),
                            (c,r) => c.Replace("[fields]", r)
                        ),
                        new JsonCompile("{\"[entity]\":\"Question\"}")
                    }
                )
                .FilenameCompilation(
                        new List<IProcedure> { new JsonCompile("{\"_in\":\"_question\"}") }
                )
                .Compile()
                .ContentCompilation(
                    new List<IProcedure> {
                        new ModelCompile(
                            "fsma_Answers",
                            new ccCustomConverter(converter),
                            (c,r) => c.Replace("[fields]", r)
                        ),
                        new JsonCompile("{\"[entity]\":\"Answer\"}")
                    }
                )
                .FilenameCompilation(
                        new List<IProcedure> { new JsonCompile("{\"_in\":\"_answer\"}")}
                )
                .Compile()
                ;
        }  
    }
}

/*
 ,
                (r, p1) => r.Replace("[fields]", p1) 
 */
