/**************************************************************
 *  Filename:    Extensions.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.Injection.Aop;
using LWJ.ObjectBuilder; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection
{
    public static partial class Extensions
    {


        public static IInjector RegisterType(this IInjector source, Type interfaceType, string name, Type targetType, params IInjectMember[] injectMembers)
        {
            return source.RegisterType(interfaceType, name, targetType, null, injectMembers);
        }
        public static IInjector RegisterType(this IInjector source, Type interfaceType, Type targetType, params IInjectMember[] injectMembers)
        {
            return source.RegisterType(interfaceType, null, targetType, null, injectMembers);
        }


        public static IInjector RegisterType<TInterface, TTarget>(this IInjector source, string name, ILifetime lifetime = null, params IInjectMember[] injectMembers)
            where TTarget : TInterface
        {
            return source.RegisterType(typeof(TInterface), name, typeof(TTarget), null, injectMembers);
        }

        public static IInjector RegisterType<TInterface, TTarget>(this IInjector source, params IInjectMember[] injectMembers)
            where TTarget : TInterface
        {
            return source.RegisterType(typeof(TInterface), null, typeof(TTarget), null, injectMembers);
        }

        public static IInjector RegisterProxy<TInterface, TProxy>(this IInjector source, string name = null, Type proxyServerType = null)
        {
            return source.RegisterProxy(typeof(TInterface), name, typeof(TProxy), proxyServerType);
        }

        public static IInjector RegisterProxy(this IInjector source, Type interfaceType, string name, Type proxyType)
        {
            return source.RegisterProxy(interfaceType, name, proxyType, null);
        }


        public static void RegisterValue(this IInjector source, Type interfaceType, object value, ILifetime lifetime = null)
        {
            source.RegisterValue(interfaceType, null, value, lifetime);
        }

        public static void RegisterValue(this IInjector source, Type interfaceType, string name, object value)
        {
            source.RegisterValue(interfaceType, name, value, null);
        }

        public static IInjector RegisterValue<TInterface>(this IInjector source, TInterface value, ILifetime lifetime = null)
        {
            return source.RegisterValue(typeof(TInterface), null, value, lifetime);
        }
        public static IInjector RegisterValue<TInterface>(this IInjector source, string name, TInterface value, ILifetime lifetime = null)
        {
            return source.RegisterValue(typeof(TInterface), name, value, lifetime);
        }


        public static bool IsTypeRegistered(this IInjector source, Type interfaceType)
        {
            return source.IsTypeRegistered(interfaceType, null);
        }

        public static bool IsTypeRegistered<TInterface>(this IInjector source, string name = null)
        {
            return source.IsTypeRegistered(typeof(TInterface), name);
        }

        public static bool IsValueRegistered(this IInjector source, Type interfaceType)
        {
            return source.IsValueRegistered(interfaceType, null);
        }

        public static bool IsValueRegistered<TInterface>(this IInjector source, string name = null)
        {
            return source.IsValueRegistered(typeof(TInterface), name);
        }

        public static T CreateInstance<T>(this IInjector source, string name, params IBuilderValue[] values)
        {
            return (T)source.CreateInstance(typeof(T), name, values);
        }
        public static T CreateInstance<T>(this IInjector source, params IBuilderValue[] values)
        {
            return (T)source.CreateInstance(typeof(T), null, values);
        }

        public static object CreateInstance(this IInjector source, Type interfaceType, string name = null)
        {
            return source.CreateInstance(interfaceType, name, null);
        }
        public static object CreateInstance(this IInjector source, Type interfaceType, params IBuilderValue[] values)
        {
            return source.CreateInstance(interfaceType, null, values);
        }


        public static bool TryGetValue(this IInjector source, Type type, string name, out object value)
        {
            return source.TryGetValue(type, name, null, out value);
        }
        public static bool TryGetValue<T>(this IInjector source, out T value)
        {
            return source.TryGetValue<T>(null, out value);
        }
        public static bool TryGetValue<T>(this IInjector source, string name, out T value)
        {
            object tmp;
            if (source.TryGetValue(typeof(T), name, out tmp))
            {
                value = (T)tmp;
                return true;
            }
            value = default(T);
            return false;
        }


        public static object Resolve(this IInjector source, Type targetType, string name = null)
        {
            return source.Resolve(targetType, name, null);
        }

        internal static object GetValue(this IInjector source, BuilderParameterInfo parameterInfo, IEnumerable<IBuilderValue> values)
        {
            if (parameterInfo == null)
                throw new ArgumentNullException(nameof(parameterInfo));
            if (parameterInfo.HasValue)
                return parameterInfo.Value;
            Type valueType = parameterInfo.ParameterType;
            string name = parameterInfo.Name;

            if (values != null)
            {
                foreach (var item in values)
                {
                    if (item.IsMatchValue(valueType, name))
                        return item.GetValue(valueType, name);
                }
            }

            if (valueType != null)
            {
                object value;
                if (source.TryGetValue(valueType, name, out value))
                    return value;

                if (source.IsTypeRegistered(valueType, name))
                    return source.CreateInstance(valueType, name, values.ToArray());
            }

            if (parameterInfo.HasDefaultValue)
                return parameterInfo.DefaultValue;

            throw new BuilderValueNotFoundException(valueType, name);
        }

        public static object[] GetValue(this IInjector source, BuilderParameterInfo[] parameterInfos, IEnumerable<IBuilderValue> values)
        {
            object[] args;
            if (parameterInfos != null)
            {
                if (parameterInfos.Length > 0)
                {
                    args = new object[parameterInfos.Length];
                    BuilderParameterInfo p;
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        p = parameterInfos[i];

                        args[i] = source.GetValue(p, values);
                    }
                }
                else
                {
                    args = InternalExtensions.EmptyObjects;
                }
            }
            else
            {
                args = null;
            }
            return args;
        }

        public static TInterface Resolve<TInterface>(this IInjector source, string name = null, IBuilderValue[] values = null)
        {
            return (TInterface)source.Resolve(typeof(TInterface), name, values);
        }
        public static TInterface Resolve<TInterface>(this IInjector source, IBuilderValue[] values)
        {
            return (TInterface)source.Resolve(typeof(TInterface), null, values);
        }


        public static void Inject(this IInjector source, object target)
        {
            source.Inject(target, null);
        }
        public static void Inject(this IInjector source, object target, IInjectMember[] injectMembers)
        {
            source.InjectMembers(target, injectMembers, null);
        }

        public static void InjectMember(this IInjector source, object target, IInjectMember injectMember, IBuilderValue[] values)
        {
            source.InjectMembers(target, new IInjectMember[] { injectMember }, values);
        }



        public static IInjector CreateChild(this IInjector source)
        {
            var o = source.CreateChild(null);

            return o;
        }



    }
}
