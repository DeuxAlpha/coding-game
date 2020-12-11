using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodinGame.Utilities.Extensions
{
    public static class ListExtensions
    {
        public static T FromEnd<T>(this List<T> enumerable, int count)
        {
            return enumerable.ElementAt(enumerable.Count - 1 - count);
        }

        public static T FromEndOrDefault<T>(this List<T> enumerable, int count)
        {
            return enumerable.ElementAtOrDefault(enumerable.Count - 1 - count);
        }
    }
}