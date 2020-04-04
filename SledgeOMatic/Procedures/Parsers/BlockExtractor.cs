using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public class BlockExtractor : IProcedure
    {
        private string _extractTarget;
        private string _fromWhere;
        private string _toWhere;
        private string _id;
        public string ID
        {
            get {
                if (string.IsNullOrEmpty(_id))
                    _id = this.GetHashCode().ToString();
                return _id; 
            }
            set { _id = value; }
        }

        public BlockExtractor( string ExtractTarget, string FromWhere, string ToWhere)
        {
        
            _extractTarget = ExtractTarget;
            _fromWhere = FromWhere;
            _toWhere = ToWhere; 
        }
        public string Execute(string content)
        {
            StringBuilder result = new StringBuilder(); 
            if (content.Contains(_fromWhere))
            {
                string[] FromStmts = content.Split(new[] {_fromWhere}, StringSplitOptions.None);
                foreach (string FromStmt in FromStmts)
                {
                    int toPos = FromStmt.IndexOf( _toWhere );
                    if (FromStmt.Contains(_extractTarget) && toPos < FromStmt.Length)
                    {
                        result.AppendFormat("{0}{1}", _fromWhere, FromStmt.Substring(0, toPos + _toWhere.Length));
                        return result.ToString();
                    } 
                }
            } 
            return result.ToString();
        }
        public override string ToString()
        {
            return $"{base.ToString().Replace(AppSettings.ProcAssembly, "")}-{ID.ToString()}";
        }
    }
}
