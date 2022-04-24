using CommandLine;
using Serilog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SOM;
using SOM.Compilers;
using SOM.IO;
using SOM.Procedures;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace CoreTests
{
    [TestClass]
    public class CompilerTests
    { 
        [TestMethod]
        public void ModelCompile_Compiles()
        { 
            var som = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache);
            var compiler = new Compiler(somContext);
            compiler.Source = som;
            compiler.FileFilter = "*.som"; 
            compiler.ContentCompilers.Add(new ModelCompile("aspnet_Users", ".*")); 
            compiler.Compile();
            cache.Inspect();
            Assert.IsNotNull(compiler); 
        }
   
        [TestMethod]
        public void Regex_Compiles()
        {
            var som = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache);
            var compiler = new Compiler(somContext);
            compiler.Source = som;
            compiler.FileFilter = "*.som";
            compiler.ContentCompilers.Add(new RegexReplacer("{\"FOO\":\"BAR\"}"));
            compiler.ContentCompilers.Add(new NumericIncrementer(1000, 2000, @"\d{4}"));
            compiler.Compile();
            cache.Inspect();
            Assert.IsNotNull(compiler);
        }
        [TestMethod]
        public void Inserter_Inserts()
        {
            string readall = File.ReadAllText(@"C:\Users\timko\source\repos\SledgeOmatic\SledgeOMatic\Procedures\Interpreters\Modulo.cs");
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache) { Content = readall }; 
            var obj = new Inserter(@"(?<after>using SOM\.Procedures)", "FOO");
            var result = obj.Compile(somContext);
            Assert.IsNotNull(obj);
        }
    }
    public class Inserter : ICompilable
    {
        private string SearchPattern;
        private string NewContent; 
        public Inserter(string SearchPattern, string NewContent)
        {
            this.SearchPattern = SearchPattern;
            this.NewContent = NewContent;
        }
        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            if (Regex.IsMatch(content, this.SearchPattern))
            {
                content = Regex.Replace(content, this.SearchPattern,
                    m =>
                    {
                        if (m.Groups.Count > 0)
                            return $"{m.Groups[0].Value}\n{this.NewContent}\n";
                        return this.NewContent;
                    }
                    , RegexOptions.Singleline);
            };
            return content;
        }
    }

}


