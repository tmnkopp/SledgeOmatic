﻿using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SOM.IO;
using SOM.Data;
using SOM.Extentions;

namespace SOM.Procedures
{
    public class RegexReplacer : KeyValReplacer, ICompilable
    {

        #region CTOR
        public RegexReplacer()
        {

        }
        [CompilableCtorMeta()]
        public RegexReplacer(string Source) : base(Source)
        {
        }

        #endregion

        #region METHODS 
        public override string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            StringBuilder result = new StringBuilder();
            var keyvals = base.PopulateKeyVals(somContext);
            foreach (var line in base.ParseLines(content))
            {
                string replacement = line;
                if (Regex.IsMatch(replacement, $@"(som!\w+|\w+!som)"))
                {
                    result.AppendLine(replacement);
                    continue;
                }
                foreach (var item in keyvals)
                {
                    var match = Regex.Match(replacement, item.Key);
                    if (match.Success)
                    {
                        replacement = Regex.Replace(
                              replacement
                            , match.Value
                            , m =>
                            {
                                if (m.Groups.Count > 2)
                                    return $"{ m.Groups[1]}{item.Value}{m.Groups[3]}";
                                else
                                    return item.Value;
                            }
                            , RegexOptions.Singleline);
                    }
                }
                replacement = Regex.Replace(replacement, $@"\n|\r", "");
                result.AppendLine(replacement);
            }
            return result.ToString();
        }

        #endregion
    } 
}
