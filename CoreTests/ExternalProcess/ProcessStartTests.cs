using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.Compilers;
using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using SOM.Procedures;
using SOMAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreTests
{ 
    [TestClass]
    public class ProcessStartTests
    {
        [TestMethod]
        public void VSCode_VsCodes()
        { 
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = ConfigurationManager.AppSettings["CodeViewer"].ToString();
            startInfo.Arguments = $"cd {Environment.GetEnvironmentVariable("repo")}";  
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start(); 
            Assert.IsNotNull(process);
        }
         
        [TestMethod]
        public void Powershell_Powershells()
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = @"& 'D:\PS\test.ps1'";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            Assert.IsNotNull(Cache.Read());
        }
        static long Factorial(int num)
        {
            if(num == 0 || num == 1){
                return 1;
            }else{
                return num * Factorial(num - 1);
            } 
        }
        [TestMethod]
        public void Cache_Writes()
        { 
            Cache.Write(""); 
            Cache.WriteLine($" ");
            Cache.Inspect();
        } 
        interface ILogger{
            void Log(string message);
        }
        class CacheLogger : ILogger{
            public void Log(string message){
                Cache.WriteLine($"{message}");
            }
        }
        static class RangeProvider{ 
            public static IEnumerable<int> Provide(){
                for (var i = 0; i < 100; i++){
                    yield return i;
                }  
            }
        }
    }
}
 