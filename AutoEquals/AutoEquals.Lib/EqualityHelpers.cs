﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualityHelpers.cs" company="AutoEquals">
//   AutoEquals Library. Licence: GNU GPL 2.0. No warranty granted, use at your own risk.
// </copyright>
// <summary>
//   Defines the EqualityHelpers type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AutoEquals.Lib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using AutoEquals.Lib.Extensions;

    /// <summary>
    /// The equality helpers.
    /// </summary>
    internal static class EqualityHelpers
    {
        /// <summary>
        /// The get property value.
        /// </summary>
        /// <param name="obj">
        /// The object to get a property from.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// Value of a given property in a given object.
        /// </returns>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        /// <summary>
        /// Performs the equality check between the two objects.
        /// </summary>
        /// <param name="obj1">First object to check.</param>
        /// <param name="obj2">Second object to compare with.</param>
        /// <param name="type">Definition of the full namespace type defining the type of both objects.</param>
        /// <returns>True if objects are equal, using a check most suitable for a given type.</returns>
        public static bool TypeEqual(object obj1, object obj2, Type type)
        {
            var typeName = type.ToString();

            switch (typeName)
            {
                case "System.String":
                    return string.Equals(obj1, obj2);
                case "System.Int32":
                case "System.Int64":
                case "System.Double":
                case "System.Single":
                case "System.Decimal":
                case "System.Boolean":
                case "System.DateTime":
                case "System.TimeSpan":
                    return obj1.Equals(obj2);
                case "System.Collections.IEnumerable":
                    return EnumerableEquals(obj1, obj2);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return EnumerableEquals(obj1, obj2);
            }

            return obj1.Equals(obj2);
        }

        /// <summary>
        /// The type hash code.
        /// </summary>
        /// <param name="obj">
        /// The object to get hash code for.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int TypeHashCode(object obj, Type type)
        {
            if (obj == null)
            {
                return 0;
            }

            var typeName = type.ToString();

            switch (typeName)
            {
                case "System.String":
                case "System.Int32":
                case "System.Int64":
                case "System.Double":
                case "System.Single":
                case "System.Decimal":
                case "System.Boolean":
                case "System.DateTime":
                case "System.TimeSpan":
                    return obj.GetHashCode();
                case "System.Collections.IEnumerable":
                    return ((IEnumerable)obj).GetCollectionHashCode();
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return ((IEnumerable)obj).GetCollectionHashCode();
            }

            return obj.GetHashCode();
        }

        /// <summary>
        /// The enumerable equals.
        /// </summary>
        /// <param name="obj1">
        /// The object 1.
        /// </param>
        /// <param name="obj2">
        /// The object 2.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool EnumerableEquals(object obj1, object obj2)
        {
            var dynamic1 = (IEnumerable)obj1;
            var dynamic2 = (IEnumerable)obj2;

            return dynamic1.UnsortedSequencesEqual(dynamic2);
        }
    }
}
