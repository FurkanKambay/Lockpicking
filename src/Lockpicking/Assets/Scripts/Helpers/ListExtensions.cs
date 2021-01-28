using System;
using System.Collections.Generic;

namespace Lockpicking.Helpers
{
    public static class ListExtensions
    {
        private static readonly Random random = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            for (int i = list.Count; i > 1; i--)
            {
                int j = random.Next(i);
                T temp = list[j];
                list[j] = list[i - 1];
                list[i - 1] = temp;
            }

            return list;
        }
    }
}
