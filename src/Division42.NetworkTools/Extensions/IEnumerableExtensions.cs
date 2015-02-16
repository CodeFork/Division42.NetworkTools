using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TItem> Each<TItem>(this IEnumerable<TItem> instance, Action<TItem> code)
        {
            foreach (TItem item in instance)
            {
                code.Invoke(item);
            }

            return instance;
        }
    }
}
