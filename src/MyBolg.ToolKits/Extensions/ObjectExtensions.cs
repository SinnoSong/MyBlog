using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBolg.ToolKits.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}