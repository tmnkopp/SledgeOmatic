using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM
{
    public interface IDirRenamer
    {
        void Rename(List<IProcedure> procedures);
    }
}
