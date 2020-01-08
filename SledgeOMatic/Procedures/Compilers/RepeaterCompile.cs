﻿using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SOM.Procedures
{
    public class RepeaterCompile : IProcedure
    {
        private int _from = 0;
        private int _to = 1;

        public RepeaterCompile(int From, int To)
        {
            _from = From;
            _to = To;
        }
        public string Execute(string compileme)
        {
            if (_from >= _to)
                throw new InvalidOperationException(); 
            StringBuilder result = new StringBuilder(); 
            for (int index = _from; index <= _to; index++)
            {
                result.AppendFormat("{0}\n", compileme); 
            } 
            return result.ToString();
        }
        public override string ToString()
        {
            return $"{base.ToString()} -{_from.ToString()} -{_to.ToString()}";
        }
    }
}