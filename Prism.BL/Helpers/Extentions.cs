using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Configrations
{
    public static class Extentions
    {
        public static Func<T, bool> AndAlso<T>(this Func<T, bool> predicate1, Func<T, bool> predicate2)
        {
            return arg => predicate1(arg) && predicate2(arg);
        }
    }
}
