﻿using SOM.Core;
using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class SomTagInterpreter : ICompilable
    {
        #region PROPS 
        public string Options { get; set; }
        #endregion

        #region FIELDS 
        private bool verbose = true;
        #endregion

        #region CTOR 
        public SomTagInterpreter()
        {

        }
        public SomTagInterpreter(string Options)
        {
            verbose = Options.Contains("-v");
        } 
        #endregion

        #region METHODS

        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;

            for (int i = 12; i >= 0; i -= 4)
            { 
                var parsed = new SomDocParser(i).Parse(somContext); 
                foreach (CommandParseResult pr in parsed)
                {   
                    var parseItem = pr.Parsed;  
                    somContext.Logger.Information("{o}", new{ RawOptions= pr.RawOptions });
                     
                    ICompilable obj = GenericFactory<ICompilable>.Create(pr.CommandType.FullName, pr.Options.Params); 
                    somContext.Content = parseItem;
                    parseItem = obj.Compile(somContext);
                    parseItem = RemoveTags(parseItem); 
                    content = content.Replace(pr.Parsed, parseItem);
                    somContext.Content = content;
                    if (pr.Options.Verbose)
                        somContext.Cache.Write(content);
                }
            }
            return somContext.Content;
        }
        public string RemoveTags(string tagged)
        {
            StringBuilder sb = new StringBuilder();
            tagged= tagged.Replace("\r","\n").Replace("\n\n", "\n");
            var lines = Regex.Split(tagged, @"[\n]");
            foreach (var line in lines)
            {
                if (!Regex.IsMatch(line, $@"(som!\w+|\w+!som)") )
                    sb.AppendLine(line);
            }
            string ret = sb.ToString();
            ret = ret.Replace("\r", "\n").Replace("\n\n", "\n");
            return ret;
        }
        public static ConstructorInfo GetConstructorInfo(Type typ)
        {
            return (from con in typ.GetConstructors()
                    let attr = (CompilableCtorMeta[])con.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                    from a in attr
                    where a.Invokable
                    select con).FirstOrDefault();
        } 
        #endregion

    }
}
