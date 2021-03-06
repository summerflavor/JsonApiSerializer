﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonApiSerializer.Util
{
     public static class TypeInfoShim
    {
        public static IEnumerable<Type> GetInterfaces(TypeInfo info)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4
            foreach (var i in info.ImplementedInterfaces)
            {
                yield return i;
            }
            if (info.BaseType != null)
            {
                foreach (var i in GetInterfaces(info.BaseType.GetTypeInfo()))
                {
                    yield return i;
                }
            }
#else
            return info.GetInterfaces();
#endif
        }

        public static PropertyInfo GetProperty(TypeInfo info, string property)
        {
#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4
            return info.AsType().GetRuntimeProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.OrdinalIgnoreCase));
#else
            return info.GetProperty(property, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
#endif
        }
    }
}
