using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class Extractor : ICompilable
    {
        #region PROPS  
        public string From { get; set; } 
        public string To { get; set; } 
        #endregion 
       
        #region CTOR 
        public Extractor()
        { 
        }
        [CompilableCtorMeta()]
        public Extractor(string From,  string To)
        {
            this.From = From; 
            this.To = To;
        }
        #endregion

        #region METHODS 
        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            int from = Regex.IsMatch(From,"^\\d{4}$") ? Convert.ToInt32(From): content.IndexOf(From);
            int to = Regex.IsMatch(To, "^\\d{4}$") ? Convert.ToInt32(To) : content.IndexOf(To);
            if (from > 0 && to > from){
                content = content.Substring(from, to - from);
            } 
            return content;
        } 
        #endregion
    }
}
