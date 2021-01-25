﻿using SOM.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public static class Invoker
    {
        public static object Invoke(string InvocationCommand)
        {
            string[] commands = InvocationCommand.Split(new string[] { " -" }, StringSplitOptions.None); 
            Type type = Type.GetType($"{commands[0]}");
            ConstructorInfo ctor = type.GetConstructors()[0];
            ParameterInfo[] PI = ctor.GetParameters();
            object[] typeParams = new object[PI.Count()];
            int i = 0;
            foreach (ParameterInfo parm in PI){ 
                string parmName = parm.Name; 
                if (parm.ParameterType == typeof(int)) 
                    typeParams[i] = Convert.ToInt32(commands[i + 1]); 
                else 
                    typeParams[i] = commands[i + 1].RemoveAsChars("'");
                
                i++;
            }
            object procedure = ctor.Invoke(typeParams);
            return procedure;
        }
        public static object InvokeProcedure(string InvocationCommand)
        {
            string[] commands = InvocationCommand.Split(new string[] { " -" }, StringSplitOptions.None); 
            Type type = Type.GetType($"SOM.Procedures.{commands[0]}, SOM");
            ConstructorInfo ctor = type.GetConstructors()[0];
            ParameterInfo[] PI = ctor.GetParameters();
            object[] typeParams = new object[PI.Count()];
            int parmCnt = 0;
            int commandCnt = 1;
            foreach (ParameterInfo parm in PI){
                if (commandCnt<=commands.Length)
                {
                    if (parm.ParameterType == typeof(int))
                        typeParams[parmCnt] = Convert.ToInt32(commands[commandCnt]);
                    else
                        typeParams[parmCnt] = commands[commandCnt].RemoveAsChars("'");
                    parmCnt++;
                    commandCnt++;
                } 
            }
            return ctor.Invoke(typeParams); 
        }

    }
     
    public static class TypeUtils
    {
        public static Type GetType(string type)
        {
            return Type.GetType($"{type}");
        }
        public static string GetCompilerFormat(string CompileType)
        {
            return  $"SOM.Procedures.{CompileType}, SOM";
        }
        public static bool IsInt(object arg)
        {
            try
            {
                int i = Convert.ToInt32(arg);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}