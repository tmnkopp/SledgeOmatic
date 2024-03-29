﻿using SOM.Extentions;
using SOM.Parsers;
using SOM.Procedures ; 
using SOM.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using SOM.Data;

namespace SOM.Procedures
{
    
    public class RangeExtractor :IParser<string>
    { 
        private string _extractPattern;
        private string _fromWhere;
        private string _toWhere; 
         
        public RangeExtractor(string ExtractPattern, string FromWhere, string ToWhere)
        { 
            _extractPattern = ExtractPattern;
            _fromWhere = FromWhere;
            _toWhere = (ToWhere!= "") ? ToWhere : "~~~~"; 
        }
        public IEnumerable<string> Parse(ISomContext somContext)
        {
            string content = somContext.Content;
            string _fromPattern = $"(?={_fromWhere})";
            string[] lines = Regex.Split(content, _fromPattern);
            lines = (from fs in lines where Regex.IsMatch(fs, _extractPattern) select fs).ToArray();
       
            foreach (var line in lines)
            { 
                if (Regex.IsMatch(line, _extractPattern))
                {
                    int toPos = line.IndexOf(_toWhere);
                    toPos = (toPos < 0) ? line.Length : toPos + _toWhere.Length; 
                    if (toPos > line.Length) toPos = line.Length;
                    yield return string.Format("{0}",line.Substring(0, toPos)); 
                }
            } 
        }
    }
}
