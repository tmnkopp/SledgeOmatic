using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class Insert : ICompilable
    {
        #region PROPS 
        public string _prop;
        public string Prop { get; set; }
        public string _prop1;
        public string Prop1 { get; set; }
        private string _searchPattern;
        private string _newContent;
        private string _format;
        #endregion

        #region CTOR 
        public Insert()
        {

        }
        [CompilableCtorMeta()]
        public Insert(string SearchPattern, string NewContent, string Format)
        {
            this._searchPattern = SearchPattern;
            this._newContent = NewContent.Replace(@"\n", System.Environment.NewLine);
            this._format = Format.Replace(@"\n", System.Environment.NewLine);
        }
        #endregion

        #region METHODS 
        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            if (Regex.IsMatch(content, this._searchPattern, RegexOptions.Singleline))
            {
                content = Regex.Replace(content, this._searchPattern,
                    m =>
                    {
                        if (m.Groups.Count > 0)
                        { 
                            for (int i = 0; i < m.Groups.Count; i++)
                            {
                                this._format = this._format.Replace($"${i}", m.Groups[i].Value);
                            }
                            return this._format.Replace("$R", this._newContent);
                        }
                        return this._newContent;
                    }
                    , RegexOptions.Singleline);
            };
            return content;
        } 
        #endregion
    }
}
