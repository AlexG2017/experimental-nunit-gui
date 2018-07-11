using System;
using System.Collections.Generic;

namespace Nunit.Gui
{
    public static class ArrayExtensions
    {
        public static bool IsIntersect<T>(T[] array1, T[] array2, IEqualityComparer<T> comparer = null)
        {
            if (array1 == null)
            {
                throw new ArgumentNullException("array1");
            }
            if (array2 == null)
            {
                throw new ArgumentNullException("array2");
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            foreach (T item in array1)
            {
                if (Array.Exists(array2, x => comparer.Equals(x, item)))
                {
                    return true;
                }
            }

            return false;
        }

        public static T[] Intersect<T>(T[] array1, T[] array2, IEqualityComparer<T> comparer = null)
        {
            if (array1 == null)
            {
                throw new ArgumentNullException("array1");
            }
            if (array2 == null)
            {
                throw new ArgumentNullException("array2");
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            var result = new List<T>();
            foreach (T item in array1)
            {
                if (Array.Exists(array2, x => comparer.Equals(x, item)))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        public static T[] Except<T>(T[] array1, T[] array2, IEqualityComparer<T> comparer = null)
        {
            if (array1 == null)
            {
                throw new ArgumentNullException("array1");
            }
            if (array2 == null)
            {
                throw new ArgumentNullException("array2");
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            var result = new List<T>();
            foreach (T item in array1)
            {
                if (!Array.Exists(array2, x => comparer.Equals(x, item)))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }
    }
}
