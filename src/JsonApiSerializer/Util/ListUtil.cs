﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JsonApiSerializer.Util
{
    internal static class ListUtil
    {
        public static bool IsList(Type type)
        {
            Type output;
            return IsList(type, out output);
        }

        /// <summary>
        /// Determines whether the specified type is list.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="elementType">The element type of the list.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is list; otherwise, <c>false</c>.
        /// </returns>
            public static bool IsList(Type type, out Type elementType)
        {
            if (type.IsArray)
            {
                elementType = type.GetElementType();
                return true;
            }

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType)
            {
                var listType = typeInfo.GetGenericTypeDefinition();
                elementType = typeInfo.GenericTypeArguments[0];
                if (typeof(IEnumerable<>).MakeGenericType(elementType).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                    return true;
            }


            elementType = null;
            return false;
        }

        /// <summary>
        /// Creates a list of the specified type.
        /// </summary>
        /// <param name="listType">Type of the list.</param>
        /// <param name="elements">The elements to add to the list.</param>
        /// <returns>list that is assignable to listType</returns>
        /// <exception cref="System.ArgumentException">
        /// listType - when listType is not a list
        /// </exception>
        public static object CreateList(Type listType, IEnumerable<object> elements)
        {
            Type elementType;
            if (!IsList(listType, out elementType))
                throw new ArgumentException($"{nameof(listType)} must be a list. {nameof(listType)} was {listType}", nameof(listType));

            //if we are an array we need to create wtih Array.CreateInstance
            if (listType.IsArray)
            {
                var elementList = elements.ToArray();
                var array = Array.CreateInstance(elementType, elementList.Length);
                Array.Copy(elementList, array, elementList.Length);
                return array;
            }

            //if the type can be satisfied with a list we will create a list
            var concreteListType = typeof(List<>).MakeGenericType(elementType);
            if (listType.GetTypeInfo().IsAssignableFrom(concreteListType.GetTypeInfo()))
            {
                var list = (IList)Activator.CreateInstance(concreteListType);
                foreach (var element in elements)
                    list.Add(element);
                return list;
            }


            throw new ArgumentException($"Cannot create list of type {listType}", nameof(listType));
        }
    }
}
