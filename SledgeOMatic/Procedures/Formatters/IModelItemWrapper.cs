using SOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures 
{
    public interface IModelItemWrapper
    { 
        string Format(AppModelItem AppModelItem);
    } 
}
