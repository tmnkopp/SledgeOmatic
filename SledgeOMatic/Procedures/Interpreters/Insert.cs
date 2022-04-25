using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class Insert : ICompilable
    {
        private string _searchPattern;
        private string _newContent;
        private string _format;
        public Insert(string SearchPattern, string Format, string NewContent)
        {
            this._searchPattern = SearchPattern;
            this._newContent = NewContent.Replace(@"\n", System.Environment.NewLine);
            this._format = Format.Replace(@"\n", System.Environment.NewLine);
        }
        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            if (Regex.IsMatch(content, this._searchPattern))
            {
                content = Regex.Replace(content, this._searchPattern,
                    m =>
                    {
                        if (m.Groups.Count > 0)
                        {
                            return this._format
                            .Replace("$1", this._newContent)
                            .Replace("$2", m.Groups[0].Value);
                        }
                        return this._newContent;
                    }
                    , RegexOptions.Singleline);
            };
            return content;
        }
    }
}
