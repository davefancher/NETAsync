using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Extensions
    {
        public static U Pipe<T, U>(this T @this, Func<T, U> fn) => fn(@this);
        public static void Apply<T>(this T @this, Action<T> fn) => fn(@this);
    }
}
