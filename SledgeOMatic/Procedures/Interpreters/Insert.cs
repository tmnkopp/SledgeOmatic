using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class Insert : ICompilable
    {
        private string SearchPattern;
        private string NewContent;
        private string _format;
        public Insert(string SearchPattern, string Format, string NewContent)
        {
            this.SearchPattern = SearchPattern;
            this.NewContent = NewContent;
            this._format = Format.Replace(@"\n", System.Environment.NewLine);
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
                        {
                            return this._format
                            .Replace("$1", this.NewContent)
                            .Replace("$2", m.Groups[0].Value);
                        }
                        return this.NewContent;
                    }
                    , RegexOptions.Singleline);
            };
            return content;
        }
    }
}
