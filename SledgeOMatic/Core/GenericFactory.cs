using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
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
                var cmdProps = new Dictionary<string, string>();
                foreach (var item in Regex.Split(m.Groups[1].Value, $"/p:"))
                {
                    if (item.Contains("=")){
                        cmdProps.Add(item.Split("=")[0], item.Split("=")[1].TrimEnd());
                    }     
                }

                obj = (T)Activator.CreateInstance(type);
                 
                PropertyInfo[] typeProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo typeProp in typeProps)
                { 
                    string alias = $"^{typeProp.Name}$";
                    InlineParam inlineParamAttr = (from a in typeProp.GetCustomAttributes(true) where a.GetType() == typeof(InlineParam) select a).FirstOrDefault() as InlineParam; 
                    if (inlineParamAttr != null) 
                        alias = $"^{typeProp.Name}$|{inlineParamAttr.Alias}";

                    var cmdProperty = (from cmdProp in cmdProps where Regex.IsMatch(cmdProp.Key, alias) select cmdProp).FirstOrDefault();
                    if (cmdProperty.Key != null)
                    { 
                        object result = Convert.ChangeType(cmdProperty.Value, typeProp.PropertyType);
                        obj.GetType().GetProperties().Where(p => p.Name == typeProp.Name).FirstOrDefault().SetValue(obj, result, null); 
                    } 
                } 
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
