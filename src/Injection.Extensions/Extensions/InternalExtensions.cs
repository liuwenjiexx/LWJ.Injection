/**************************************************************
 *  Filename:    InternalExtensions.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace LWJ
{
    internal static partial class InternalExtensions
    {
        public static readonly object[] EmptyObjects = new object[0];
        public static int CombineHashCodes(int h1, int h2)
        {
            return h1 * 31 + h2;
        }

        public static bool Contains<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value, IEqualityComparer<TValue> valueComparer)
        {
            List<TValue> values;

            if (!dictionary.TryGetValue(key, out values))
                return false;

            return values.Contains(value, valueComparer);
        }
        public static void Add<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
        {
            dictionary.GetOrAdd(key).Add(value);
        }
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");
            TValue value;
            if (dic.TryGetValue(key, out value))
                return value;
            value = factory(key);
            dic[key] = value;
            return value;
        }
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            return dictionary.GetOrAdd<TKey, TValue>(key, () => new TValue());
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> newValue)
        {
            TValue result;

            if (!dictionary.TryGetValue(key, out result))
            {
                result = newValue();
                dictionary[key] = result;
            }

            return result;
        }
        public static bool ItemsEquals(this IEnumerator source, IEnumerator compare)
        {
            while (source.MoveNext())
            {
                if (!compare.MoveNext())
                    return false;

                if (!object.Equals(source.Current, compare.Current))
                    return false;
            }
            return !compare.MoveNext();
        }
        public static int GetElementsHashCode(this IEnumerable array)
        {
            if (array == null)
                return 0;

            int hashCode = 0;

            foreach (var item in array)
            {
                int elementHash;
                if (item != null)
                    elementHash = item.GetHashCode();
                else
                    elementHash = 0;

                hashCode = CombineHashCodes(hashCode, elementHash);
            }

            return hashCode;
        }
        /*
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider member, bool inherit)
            where T : Attribute
        {
            var attrs = member.GetCustomAttributes(typeof(T), inherit);
            if (attrs.Length > 0)
                return (T)attrs[0];
            //for (int i = 0; i < attrs.Length; i++)
            //{
            //    if (attrs[i] is T)
            //        return (T)attrs[i];
            //}
            return null;
        }
        */

            /*
        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider member, bool inherit)
            where T : Attribute
        {
            var attrs = member.GetCustomAttributes(typeof(T), inherit);

            return (T[])attrs;
            //List<T> lst = new List<T>(attrs.Length);

            //for (int i = 0; i < attrs.Length; i++)
            //{
            //    if (attrs[i] is T)
            //        lst.Add((T)attrs[i]);
            //}
            //return lst.ToArray();
        }*/

        public static void SetValueUnity(this PropertyInfo source, object obj, object value, object[] index)
        {
            source.GetSetMethod(true).Invoke(obj, new object[] { value });
        }

        public static object GetValueUnity(this PropertyInfo source, object obj, object[] index)
        {
            object value;
            value = source.GetGetMethod(true).Invoke(obj, null);
            return value;
        }

        public static MethodInfo GetMethod(this Type source, string name, Type[] types, BindingFlags bindingFlags)
        {

            if (types == null)
                types = Type.EmptyTypes;
            int len = types.Length;
            MethodInfo result = null;
            foreach (var m in source.GetMember(name, MemberTypes.Method, bindingFlags).Cast<MethodInfo>())
            {
                var argInfos = m.GetParameters();
                if (argInfos.Length == len)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (argInfos[i].ParameterType.IsAssignableFrom(types[i]))
                        {
                            result = m;
                            break;
                        }
                    }
                    if (result != null)
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// [null value] to [null type]
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Type[] ToTypes(this object[] source)
        {
            if (source == null || source.Length == 0)
                return Type.EmptyTypes;

            int len;
            Type[] types;

            len = source.Length;
            types = new Type[len];
            for (int i = 0; i < len; i++)
            {
                if (source[i] == null)
                    types[i] = null;
                else
                    types[i] = source[i].GetType();
            }

            return types;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="argTypes">null arg type match any not value type</param>
        /// <returns></returns>
        public static bool IsMatch(this MethodBase method, Type[] argTypes)
        {

            ParameterInfo[] ps;
            int argLen = argTypes.Length;

            ps = method.GetParameters();

            if (ps.Length != argLen)
            {
                if (argLen > ps.Length || !ps[argLen].IsOptional)
                    return false;
            }
            else if (argLen == 0)
            {
                return true;
            }

            ParameterInfo p;
            Type paramType;
            Type argType;
            bool result = true;
            for (int i = 0; i < argLen; i++)
            {
                p = ps[i];
                paramType = p.ParameterType;
                argType = argTypes[i];
                if (argType == null)
                {
                    if (paramType.IsValueType)
                    {
                        result = false;
                        break;
                    }                     
                }
                else
                {
                    if (!paramType.IsAssignableFrom(argType))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// aligin parameter length, padding DefaultValue
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object[] AliginArguments(this MethodBase method, object[] args)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            args = args ?? EmptyObjects;
            int argLen = args.Length;
            ParameterInfo[] ps = method.GetParameters();
            if (argLen != ps.Length)
            {
                var tmp = new object[ps.Length];
                Array.Copy(args, tmp, argLen);
                for (int i = argLen; i < ps.Length; i++)
                {
                    var p = ps[i];
                    tmp[i] = p.DefaultValue;
                }
                args = tmp;
            }
            return args;
        }
        /*
        public static string FormatArgs(this string source,params object[] args)
        {
            return string.Format(source, args);
        }*/


        public static T GetCustomAttribute<T>(this ICustomAttributeProvider member, bool inherit)
            where T : Attribute
        {
            var attrs = member.GetCustomAttributes(typeof(T), inherit);
            if (attrs != null && attrs.Length > 0)
                return (T)attrs[0];
            return null;
        }
        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider member, bool inherit)
            where T : Attribute
        {
            var attrs = member.GetCustomAttributes(typeof(T), inherit);
            if (attrs != null && attrs.Length > 0)
            {
                T[] result = new T[attrs.Length];
                for (int i = 0; i < attrs.Length; i++)
                    result[i] = (T)attrs[i];
                return result;
            }
            return new T[0];
        }
        public static string FormatArgs(this string source, params object[] args)
        {
            return string.Format(source, args);
        }
    }




}
