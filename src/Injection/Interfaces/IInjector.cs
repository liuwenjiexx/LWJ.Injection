/**************************************************************
 *  Filename:    IInjector.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;

namespace LWJ.Injection
{
    public interface IInjector : IDisposable
    {

        /// <summary>
        /// injector name
        /// </summary>
        string Name { get;  }

        //no public
        //IInjector Parent { get; }

        /// <summary>
        /// mapping interface
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name">interface name</param>
        /// <param name="targetType">implement type</param>
        /// <param name="lifetime"></param>
        /// <param name="injectMembers"></param>
        /// <returns></returns>
        IInjector RegisterType(Type interfaceType, string name, Type targetType, ILifetime lifetime, IInjectMember[] injectMembers);

        /// <summary>
        /// register proxy
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        /// <param name="proxyType">implement IProxyInvoker interface</param>
        /// <returns></returns>
        IInjector RegisterProxy(Type interfaceType, string name, Type proxyType, Type proxyServerType);

        /// <summary>
        /// register value
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IInjector RegisterValue(Type interfaceType, string name, object value, ILifetime lifetime);

        /// <summary>
        /// if invoked {RegisterType} return true else return false
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        /// <returns>return is self or parents registered</returns>
        bool IsTypeRegistered(Type interfaceType, string name);

        /// <summary>
        /// if invoked {RegisterProxy} return true else return false
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsProxyRegistered(Type interfaceType, string name);

        /// <summary>
        /// if invoked {RegisterValue} return true else return false
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsValueRegistered(Type interfaceType, string name);

        /// <summary>
        /// find registered value
        /// </summary>
        /// <param name="valueType"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(Type type, string name, IBuilderValue[] values, out object value);


        /// <summary>
        /// create new object
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        object CreateInstance(Type targetType, string name, IBuilderValue[] values);


        /// <summary>
        /// find registered value, not found create instance
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        object GetOrCreateInstance(Type interfaceType, string name, IBuilderValue[] values);


        /// <summary>
        /// find {RegisterValue} value,if not find the invoke {CreateInstance} and {RegisterValue}
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        object Resolve(Type targetType, string name, IBuilderValue[] values);


        /// <summary>
        /// create child Injector
        /// </summary>
        /// <returns></returns>
        IInjector CreateChild(string name);


        /// <summary>
        /// assign have  attribute {InjectValueAttribute} members(field, property, method) value。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="values"></param>
        void Inject(object target, IBuilderValue[] values);

        /// <summary>
        /// assign more  special members value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="members"></param>
        /// <param name="values"></param>
        void InjectMembers(object target, IEnumerable<IInjectMember> members, IBuilderValue[] values);

        /// <summary>
        /// assign special member value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="member"></param>
        /// <param name="values"></param>
        void InjectMember(object target, IInjectMember member, IBuilderValue[] values);

        /// <summary>
        /// extension object build pipeline
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IInjector AddBuilder(IBuilder builder);

        /// <summary>
        /// extension aop behaviour policy
        /// </summary>
        /// <param name="name">policy name</param>
        /// <returns></returns>
        ICallPolicy AddCallPolicy(string name);

        ICallPolicy AddCallPolicy();

        /// <summary>
        /// get all call policies in injector parents
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICallPolicy> GetAllCallPolicies();

        /// <summary>
        /// load injector config xml file
        /// </summary>
        /// <param name="xml"></param>
        void Config(string xml);
    }

}
