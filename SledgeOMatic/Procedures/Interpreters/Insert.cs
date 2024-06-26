﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class Insert : ICompilable
    {
        #region PROPS  
        public string Pattern { get; set; } 
        public string Format { get; set; }
        #endregion 
        #region CTOR 
        public Insert()
        { 
        }
        [CompilableCtorMeta()]
        public Insert(string MatchPattern,  string Format)
        {
            this.Pattern = MatchPattern; 
            this.Format = Format.Replace(@"\n", System.Environment.NewLine);
        }
        #endregion

        #region METHODS 
        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            if (Regex.IsMatch(content, this.Pattern, RegexOptions.Singleline))
            {
                content = Regex.Replace(content, this.Pattern,
                    m =>
                    {
                        if (m.Groups.Count > 0)
                        { 
                            for (int i = 0; i < m.Groups.Count; i++)
                            {
                                this.Format = this.Format.Replace($"{{{i}}}", m.Groups[i].Value); // {1}
                            } 
                        }
                        return this.Format;
                    }
                    , RegexOptions.Singleline);
            };
            return content;
        } 
        #endregion
    }
}
