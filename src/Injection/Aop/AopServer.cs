/**************************************************************
 *  Filename:    AopServer.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.Proxies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LWJ.Injection.Aop
{
    /// <summary>
    /// AOP (Aspect Oriented Programming)
    /// </summary>
    internal class AopServer : ProxyServer
    {

        private IInjector injector;

        private Type serverType;

        private object target;

        private bool isAttachServer;

        private Dictionary<string, AopMemberMetadata[]> members;

        private static Dictionary<Type, Dictionary<string, AopMemberMetadata[]>> cachedMethods;

        private static readonly ICallHandler defaultCallHandler = new DefaultCallHandler();

        private Dictionary<MethodBase, MethodData> methodDatas;
        [Inject]
        public AopServer(IInjector injector, object target)
            : this(injector, target.GetType(), target, true)
        {
        }
        public AopServer(IInjector injector, Type serverType)
            : this(injector, serverType, null, false)
        { }

        public AopServer(IInjector injector, Type serverType, object target)
            : this(injector, serverType, target, true)
        { }

        private AopServer(IInjector injector, Type serverType, object target, bool hasTarget)
        {

            this.injector = injector ?? throw new ArgumentNullException(nameof(injector));
            this.serverType = serverType ?? throw new ArgumentNullException(nameof(serverType));

            methodDatas = new Dictionary<MethodBase, MethodData>();


            if (hasTarget)
            {
                if (target == null)
                    throw new ArgumentNullException(nameof(target));
                AttachTarget(target);
            }
            else
            {
                target = injector.CreateInstance(serverType);

                AttachTarget(target);
            }

        }



        public IInjector Injector { get => injector; }


        private void CacheAllMethods()
        {
            MethodData methodData;


            Dictionary<Type, object> maps = null;
            var policies = injector.GetAllCallPolicies();

            //var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type type = Target.GetType();
            members = GetCachedProxyMethods(type);
            List<object> handlers = new List<object>();
            maps = new Dictionary<Type, object>();
            foreach (var metadata in members.Values.SelectMany(o => o))
            {
                var method = metadata.Method;
                if (maps != null)
                    maps.Clear();
                handlers.Clear();
                methodData = null;
                foreach (var policy in policies)
                {
                    if (policy.IsMach(method))
                    {
                        foreach (var item in policy.AllBehaviours())
                        {
                            IAopBehaviour behaviour = item as IAopBehaviour;
                            if (behaviour != null)
                            {
                                behaviour.AddAopHandler(method, handlers);
                            }
                            else
                            {
                                if (!maps.ContainsKey(item.GetType()))
                                {

                                    maps[item.GetType()] = item;
                                }
                            }

                        }



                    }
                }


                if (maps != null && maps.Count > 0 || handlers.Count > 0)
                {
                    methodData = new MethodData();

                    methodData.BeforeCallHanders = maps.Values.Union(handlers).Where(o => o is IBeforeCall).Cast<IBeforeCall>().ToArray();
                    methodData.CallHanderPipline = new CallHandlerPipeline(maps.Values.Union(handlers).Where(o => o is ICallHandler).Cast<ICallHandler>().ToArray());
                    methodData.AfterReturnCallHanders = maps.Values.Union(handlers).Where(o => o is IAfterCallReturn).Cast<IAfterCallReturn>().ToArray();
                    methodData.AfterCallHanders = maps.Values.Union(handlers).Where(o => o is IAfterCall).Cast<IAfterCall>().ToArray();
                    methodData.ThrowsCallHanders = maps.Values.Union(handlers).Where(o => o is IThrowsCall).Cast<IThrowsCall>().ToArray();

                    if (methodData.BeforeCallHanders.Length == 0)
                        methodData.BeforeCallHanders = null;
                    if (methodData.AfterReturnCallHanders.Length == 0)
                        methodData.AfterReturnCallHanders = null;
                    if (methodData.AfterCallHanders.Length == 0)
                        methodData.AfterCallHanders = null;
                    if (methodData.ThrowsCallHanders.Length == 0)
                        methodData.ThrowsCallHanders = null;

                }
                methodDatas[method] = methodData;
            }

        }


        T[] GetHandlers<T>(Dictionary<Type, object> maps, IEnumerable<Type> handlerTypes)
        {
            List<object> handlers = new List<object>();
            HandlerScope scope;
            foreach (var handlerType in handlerTypes)
            {
                var handlerAttr = handlerType.GetCustomAttribute<HandlerScopeAttribute>(true);
                if (handlerAttr != null)
                {
                    scope = handlerAttr.Scope;
                }
                else
                {
                    scope = HandlerScope.Class;
                }

                object obj = null;
                if (scope == HandlerScope.Class)
                {
                    // allCallHandlers.TryGetValue(handlerType, out obj);
                }

                if (obj == null)
                    maps.TryGetValue(handlerType, out obj);

                if (obj == null)
                {
                    obj = injector.CreateInstance(handlerType);
                    //  allCallHandlers[handlerType] = obj;
                    maps[handlerType] = obj;
                }
                //    handler = (ICallHandler)obj;
                handlers.Add(obj);

            }


            //handlers.Sort(SortByDescending);


            return handlers.Cast<T>().ToArray();
        }
        public static IComparer<ICallHandler> SortByDescending = new IHandlerSort();
         

        //public object Invoke(string methodName)
        //{
        //    return Invoke(methodName, null);
        //}

        public override object Invoke(string methodName, object[] args)
        {
            args = args ?? InternalExtensions.EmptyObjects;
            //BindingFlags invokeMethodBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod;
            //var method = InjectorUtils.FindMethod(targetType, methodName, args, invokeMethodBindingFlags);
            AopMemberMetadata member = null;
            AopMemberMetadata[] tmp;
            if (members.TryGetValue(methodName, out tmp))
            {
                foreach (var item in tmp)
                {
                    if (item.Method.IsMatch(args.ToTypes()))
                    {
                        member = item;
                        break;
                    }
                }
            }

            if (member == null)
                throw new ProxyMethodNotFoundException(serverType, methodName);

            return Invoke(member.Method, args);
        }


        public object InvokeArgs(MethodBase method, params object[] args)
        {
            return Invoke(method, args);
        }

        public object Invoke(MethodBase method, object[] args)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            args = args ?? InternalExtensions.EmptyObjects;
            MethodData info = null;

            if (!methodDatas.TryGetValue(method, out info))
                throw new AopException(string.Format("aop server type [{0}] not contains method name [{1}]", serverType.FullName, method.Name));

            var target = this.Target;

            if (info == null)
            {
                try
                {
                    return method.Invoke(method.IsStatic ? null : target, args);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }


            IParameterCollection arguments = new ParameterCollection(method.AliginArguments(args), method.GetParameters());

            IDictionary extraData = info.extraData;

            AopInvocation invocation = new AopInvocation(injector, this, target, method, arguments, extraData);

            ICallReturn result = null;

            try
            {
                if (info.BeforeCallHanders != null)
                {
                    foreach (var item in info.BeforeCallHanders)
                        item.BeforeInvoke(invocation);
                }

                result = info.CallHanderPipline.Invoke(invocation, defaultCallHandler.Invoke);

                info.extraData = invocation.ExtraData;

                if (result == null)
                    invocation.ReturnException(new AopException("invoke method not return value,method:" + method.DeclaringType.FullName + "." + method.Name));

                if (result.Exception != null)
                {
                    ThrowsHandler(info.ThrowsCallHanders, invocation, result.Exception);
                }
                else
                {
                    if (info.AfterReturnCallHanders != null)
                    {
                        foreach (var item in info.AfterReturnCallHanders)
                            item.AfterReturn(invocation, result);
                    }

                    return result.ReturnValue;
                }

            }
            catch (Exception ex)
            {
                ThrowsHandler(info.ThrowsCallHanders, invocation, ex);
                throw ex;
            }
            finally
            {
                try
                {

                    if (info.AfterCallHanders != null)
                    {
                        foreach (var item in info.AfterCallHanders)
                            item.AfterInvoke(invocation);
                    }
                }
                catch (Exception ex)
                {
                    ThrowsHandler(info.ThrowsCallHanders, invocation, ex);
                    throw ex;
                }
            }

            throw result.Exception;
        }

        void ThrowsHandler(IThrowsCall[] handlers, ICallInvocation invocation, Exception ex)
        {
            if (handlers != null)
            {
                foreach (var item in handlers)
                    item.ThrowsInvoke(invocation, ex);
            }
        }

        GetNextCallHandlerDelegate Combine4(GetNextCallHandlerDelegate previous, NextHandlerDelegate next)
        {
            return () =>
            {
                return (invocation, getNext) =>
                {
                    if (previous == null)
                        return next(invocation, null);

                    return next(invocation, previous);
                };
            };
        }
        protected override void AttachTarget(object target)
        {
            base.AttachTarget(target );

            CacheAllMethods();
        }

        protected override object DetachTarget()
        {
            this.members = null;
            this.methodDatas = null;
            return base.DetachTarget();
        }

        private class IHandlerSort : IComparer<ICallHandler>
        {
            public int Compare(ICallHandler x, ICallHandler y)
            {
                return y.Order - x.Order;
            }
        }




        private class MethodData
        {
            public IBeforeCall[] BeforeCallHanders;
            public CallHandlerPipeline CallHanderPipline;
            public IAfterCall[] AfterCallHanders;
            public IAfterCallReturn[] AfterReturnCallHanders;
            public IThrowsCall[] ThrowsCallHanders;
            public IDictionary extraData;


        }



        class AopMemberMetadata
        {
            public MethodInfo Method;
            public string Name;
            public ParameterInfo[] ParameterInfos;
             
            public AopMemberMetadata(MethodInfo method)
            {
                this.Method = method;
                this.ParameterInfos = method.GetParameters();
             
                this.Name = method.Name;
            }
        }



        static Dictionary<string, AopMemberMetadata[]> GetCachedProxyMethods(Type type)
        {
            if (cachedMethods == null)
                cachedMethods = new Dictionary<Type, Dictionary<string, AopMemberMetadata[]>>();

            Dictionary<string, AopMemberMetadata[]> result;
            if (!cachedMethods.TryGetValue(type, out result))
            {
                var tmp = new Dictionary<string, List<AopMemberMetadata>>();
                List<AopMemberMetadata> tmp2;
                string key;
                
                foreach (var member in GetProxyMembers(type))
                {
                    key = member.Name;
                    if (!tmp.TryGetValue(member.Name, out tmp2))
                    {
                        tmp2 = new List<AopMemberMetadata>();
                        tmp.Add(member.Name, tmp2);
                    }
                    tmp2.Add(member);
                }
                result = new Dictionary<string, AopMemberMetadata[]>();

                foreach (var item in tmp)
                    result.Add(item.Key, item.Value.ToArray());

                cachedMethods[type] = result;
            }
            return result;
        }

        static bool IsMatchMethod(MethodInfo member)
        {
            if (member.IsDefined(typeof(IgnoreProxyMemberAttribute), false))
                return false;
            if (!member.IsPublic)
            {
                var memberAttr = member.GetCustomAttribute<ProxyMemberAttribute>(true);
                if (memberAttr == null)
                    return false;
            }
            return true;
        }

        static IEnumerable<AopMemberMetadata> GetProxyMembers(Type type)
        {
            
            AopMemberMetadata metadata;
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

            foreach (var method in methods)
            {
                if (method.IsGenericMethod)
                    continue;

                metadata = new AopMemberMetadata(method);
                yield return metadata;
                
            }
            
        }
        


    }









}
