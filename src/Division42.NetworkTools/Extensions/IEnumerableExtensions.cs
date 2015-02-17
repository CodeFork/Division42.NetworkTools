using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.Extensions
{
    /// <summary>
    /// Extension class for <see cref="IEnumerable"/>.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Runs the specified <paramref name="code"/> for each item in the collection.
        /// </summary>
        /// <typeparam name="TItem">The type on which the enumerable is based.</typeparam>
        /// <param name="instance">The instance of the enumerable to process.</param>
        /// <param name="code">The code to execute for each item in the enumerable.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The original enumerable, for more processing.</returns>
        public static IEnumerable<TItem> Each<TItem>(this IEnumerable<TItem> instance, Action<TItem> code)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            if (code == null)
                throw new ArgumentNullException("code");

            foreach (TItem item in instance)
            {
                code.Invoke(item);
            }

            return instance;
        }
    }
}
