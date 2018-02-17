using System;
using System.Collections.Generic;
using System.Linq;

namespace facebook_comment_utils
{
    internal static class ListExtensions
    {
        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }
    }
}
