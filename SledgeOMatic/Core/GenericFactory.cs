using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Core
{
    public static class GenericFactory<T>
    {
        public static T Create(string FullTypeName, List<object> parms)
        { 
            Type type = Type.GetType($"{FullTypeName}, SOM");
            T obj = (T)Activator.CreateInstance(type, parms.ToArray());
            return obj;
        }
        public static T Create(string FullTypeName, string paramString = "")
        {
            T obj = default; 
            Type type = Type.GetType($"{FullTypeName}, SOM");
            paramString = Regex.Replace(paramString, $@"\r|\t", "");
            if (string.IsNullOrWhiteSpace(paramString))
            {
                obj = (T)Activator.CreateInstance(type);
                return obj; 
            }

            var m = Regex.Match(paramString, $@"(/p:.*)");
            if (m.Success)
            {
                var props = new Dictionary<string, string>();
                foreach (var item in Regex.Split(m.Groups[1].Value, $"/p:"))
                {
                    if (item.Contains("="))
                        props.Add(item.Split("=")[0], item.Split("=")[1].TrimEnd());
                }

                obj = (T)Activator.CreateInstance(type);

                (from p in obj.GetType().GetProperties()
                 where props.ContainsKey(p.Name)
                 select p).ToList().ForEach(p => {
                     object result = props[p.Name];
                     result = Convert.ChangeType(result, p.PropertyType); 
                     p.SetValue(obj, result, null);
                 });
                return obj;
            }

            m = Regex.Match(paramString, $@"(/p\s.*)");
            if (m.Success)
            {
                var oparms = new List<object>();
                foreach (var item in Regex.Split(m.Groups[1].Value, $"(\\s|^)/p\\s"))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        oparms.Add(item.TrimEnd());
                }
                obj = (T)Activator.CreateInstance(type, oparms.ToArray());
                return obj;
            }
            return obj;
        }
    }
}
