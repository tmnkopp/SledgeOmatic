﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public interface ICompilable
    {
         string Compile(string content);
    } 
}