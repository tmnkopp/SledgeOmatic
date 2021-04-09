using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.Procedures;
using SOM.IO;
using Newtonsoft.Json;

namespace CoreTests 
{
    [TestClass]
    public class VersionIncrementerTests
    {
        [TestMethod]
        public void VersionIncrementer_Increments()
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            d.Add("0", 2);
            d.Add("45", 2); 
            d.Add("51", 2); 
            var s = Reader.Read(@"C:\_som\_src\_compile\IG\DB_Update7.34_IG_2021.sql");
            ICompilable comp = new VersionIncrementer(",N'(\\d{1,2}.*?)',", JsonConvert.SerializeObject(d));
            var c = comp.Compile(s);
            Cache.Inspect(c);
            string  repeat = "";

        }
    }

    public class VersionIncrementer : ICompilable
    {
        #region CTOR
        int major, minor, patch = 0;
        string Pattern = "";
        Dictionary<string, int> d = new Dictionary<string, int>();
        public VersionIncrementer(string ReplacePattern, string VersionsJson)
        {
            this.d =  JsonConvert.DeserializeObject<Dictionary<string, int>>(VersionsJson) ;
            this.Pattern = ReplacePattern;
        }
        #endregion
        public string Compile(string content)
        {
            StringBuilder sb = new StringBuilder();
            string[] lines = content.Split("\n");
            foreach (string line in lines)
            {
                var matches = Regex.Match(line, Pattern);
                if (matches.Groups.Count > 1)
                { 
                    if (this.d.ContainsKey(major.ToString()))
                    {
                        int v = Convert.ToInt32(this.d[major.ToString()]);
                        string sub = Regex.Replace(
                            line
                            , this.Pattern
                            , (m) => m.Groups[0].Value.Replace(m.Groups[1].Value, $"{major}.{minor}")  
                        );
                        sb.AppendFormat("{0}\n", sub);
                        minor++; 
                        if (minor >= v) {
                            minor=0; 
                            this.d.Remove(major.ToString());
                            major++;
                        } 
                    } else { 
                        minor=0;
                        string sub = Regex.Replace(
                            line
                            , this.Pattern
                            , (m) => m.Groups[0].Value.Replace(m.Groups[1].Value, $"{major}")
                        );
                        sb.AppendFormat("{0}\n", sub);
                        major++;
                    }    
                }
            }
            return sb.ToString();
        }
    }
}
