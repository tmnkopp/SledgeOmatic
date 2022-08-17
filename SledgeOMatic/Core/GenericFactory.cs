using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Core
{
    public class GenericFactory<T>
    {
        private T obj;
        public GenericFactory()
        {
        }
        public T Create(string FullTypeName, string paramString)
        {
            Type type = Type.GetType($"{FullTypeName}, SOM");
            paramString = Regex.Replace(paramString, $@"\r|\t", "");
            var m = Regex.Match(paramString, $@"(/p:.*)\n");
            if (m.Success)
            {
                var props = new Dictionary<string, string>();
                foreach (var item in Regex.Split(m.Groups[1].Value, $"/p:"))
                {
                    if (item.Contains("="))
                        props.Add(item.Split("=")[0], item.Split("=")[1].TrimEnd());
                }

                this.obj = (T)Activator.CreateInstance(type);

                (from p in obj.GetType().GetProperties()
                 where props.ContainsKey(p.Name)
                 select p).ToList().ForEach(p => {
                     p.SetValue(obj, props[p.Name], null);
                 });
            }

            m = Regex.Match(paramString, $@"(/p\s.*)\n");
            if (m.Success)
            {
                var oparms = new List<object>();
                foreach (var item in Regex.Split(m.Groups[1].Value, $"\\s/p\\s"))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        oparms.Add(item.TrimEnd());
                }
                this.obj = (T)Activator.CreateInstance(type, oparms.ToArray());
            }
            return this.obj;
        }
    }
}
