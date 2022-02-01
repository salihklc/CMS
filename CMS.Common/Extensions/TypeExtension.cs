using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Extensions
{
    public static class TypeExtension
    {
        public static bool xIsList(this Type poType)
        {
            return (poType.IsGenericType && poType.GetGenericTypeDefinition() == typeof(List<>));
        }

        public static bool xIsArray(this Type poType)
        {
            return (poType.IsArray);
        }

        public static bool xIsSingleObject(this Type poType)
        {
            return (poType.IsClass && !poType.IsPrimitive && poType != typeof(string));
        }

        public static object xCreateList(this Type poType)
        {
            if (poType.xIsList())
                return Activator.CreateInstance(typeof(List<>).MakeGenericType(poType.GetGenericArguments()[0]));

            if (poType.xIsArray())
                return Activator.CreateInstance(typeof(List<>).MakeGenericType(poType.GetElementType()));

            return Activator.CreateInstance(typeof(List<>).MakeGenericType(poType));
        }

        public static object xCreateListItem(this Type poType)
        {
            if (poType.xIsList())
                return Activator.CreateInstance(poType.GetGenericArguments()[0]);

            if (poType.xIsArray())
                return Activator.CreateInstance(poType.GetElementType());

            return Activator.CreateInstance(poType.MakeGenericType());
        }

        public static Array xCreateArray(this Type poType, int piLength)
        {
            if (poType.xIsList())
                return Array.CreateInstance(poType.GetGenericArguments()[0], piLength);

            if (poType.xIsArray())
                return Array.CreateInstance(poType.GetElementType(), piLength);

            return Array.CreateInstance(poType, piLength);
        }

        public static object xCreateArrayItem(this Type poType)
        {
            if (poType.xIsArray())
                return Activator.CreateInstance(poType.GetElementType());

            return null;
        }
    }
}
