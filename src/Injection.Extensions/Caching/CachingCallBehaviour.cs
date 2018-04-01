/**************************************************************
 *  Filename:    CachingCallBehaviour.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LWJ.ObjectBuilder;

namespace LWJ.Injection.Aop.Caching
{


    public class CachingCallBehaviour : IAopBehaviour
    {

        private static Dictionary<MethodBase, CachingData> cachedStaticHandlers;
        private int order = 1000;

        private bool usageAttribute;

        [DefaultValue(-1)]
        [InjectValue(MaxResultCachingProperty)]
        private int maxResultCaching;

        [DefaultValue(null)]
        [InjectValue(LifetimeTypeProperty)]
        private Type lifetimeType;

        [DefaultValue(null)]
        [InjectValue(LifetimeMethodProperty)]
        private string lifetimeMethod;

        /// <summary>
        /// limit caching result
        /// </summary>
        public const string MaxResultCachingProperty = "maxResultCaching";

        public const string LifetimeTypeProperty = "lifetimeType";

        public const string LifetimeMethodProperty = "lifetimeMethod";

        public CachingCallBehaviour()
        {
        }

        public CachingCallBehaviour(int order)
        {
            this.order = order;
        }

        private CachingCallBehaviour(bool usageAttribute)
        {
            this.usageAttribute = usageAttribute;
        }


        public int Order
        {
            get { return order; }
            set { order = value; }
        }


         

        private void AddAopHandlerByUsageAttribute(MethodBase methodBase, IList methodHandlers)
        {

            CachingData data;

            if (cachedStaticHandlers == null)
                cachedStaticHandlers = new Dictionary<MethodBase, CachingData>();

            if (!cachedStaticHandlers.TryGetValue(methodBase, out data))
            {
                var cachingAttr = methodBase.GetCustomAttribute<CachingCallAttribute>(true);

                if (cachingAttr != null)
                {
                    int maxCachingResult = cachingAttr.MaxCaching;
                    Type lifetimeType = null;
                    string lifetimeMethod = null;
                    var resultLifetimeAttr = methodBase.GetCustomAttribute<CachingLifetimeAttribute>(false);

                    if (resultLifetimeAttr != null)
                    {
                        lifetimeType = resultLifetimeAttr.LifetimeType;
                        lifetimeMethod = resultLifetimeAttr.Method;
                    }

                    data = new CachingData(maxCachingResult, lifetimeType, lifetimeMethod);

                    cachedStaticHandlers[methodBase] = data;
                }
            }

            if (data != null)
            {
                methodHandlers.Add(new CachingCallHandler(data));
            }

        }

        public void AddAopHandler(MethodBase methodBase, IList methodHandlers)
        {
            if (usageAttribute)
            {
                AddAopHandlerByUsageAttribute(methodBase, methodHandlers);
            }
            else
            {
                var handler = new CachingCallHandler(new CachingData(maxResultCaching, lifetimeType, lifetimeMethod));
                methodHandlers.Add(handler);
            }
        }

        private class CachingData
        {
            public int maxResultCaching;
            public Type lifetimeType;
            public string lifetimeMethodName;

            public CachingData(int maxResultCaching, Type lifetimeType, string lifetimeMethod)
            {
                this.maxResultCaching = maxResultCaching;
                this.lifetimeType = lifetimeType;
                this.lifetimeMethodName = lifetimeMethod;
            }
        }


       // [HandlerScope(Scope = HandlerScope.Method)]
        private class CachingCallHandler : ICallHandler
        {


            private Dictionary<ElementCollection, CachingResult> cached;
            public CachingData cachingData;
              
            public int Order { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
             
            public CachingCallHandler(CachingData data)
            {
                this.cachingData = data;
            }
             

            public ICallReturn Invoke(ICallInvocation invocation, GetNextCallHandlerDelegate getNext)
            {

                if (cached == null)
                    cached = new Dictionary<ElementCollection, CachingResult>();

                var args = invocation.Arguments;
                var key = new ElementCollection(args);

                CachingResult cachingResult;
                ICallReturn result = null;

                if (cached.TryGetValue(key, out cachingResult))
                {
                    result = cachingResult.Result;
                    object value;
                    value = cachingResult.Lifetime.GetValue();
                    if (value != null)
                    //if (cachingResult.Lifetime.GetValue(out value))
                    {
                        result.ReturnValue = value;
                    }
                    else
                    {
                        cachingResult = null;
                        result = null;
                    }
                }

                if (cachingResult == null)
                {
                    result = getNext()(invocation, getNext);
                    ILifetime lifetime = GetLifetime((MethodInfo)invocation.MethodBase, invocation.Target, result.ReturnValue, cachingData.lifetimeType, cachingData.lifetimeMethodName);
                    cachingResult = new CachingResult(result, lifetime);
                    cached[key] = cachingResult;
                }

                return result;
            }



            public ILifetime GetLifetime(MethodInfo targetMethod, object obj, object result, Type lifetimeType, string lifetimeMethodName)
            {

                if (lifetimeType == null && string.IsNullOrEmpty(lifetimeMethodName))
                {
                    lifetimeType = typeof(PersistentLifetime);
                }

                ILifetime lifetime;

                if (lifetimeType != null)
                {
                    lifetime = Activator.CreateInstance(lifetimeType) as ILifetime;
                    if (lifetime == null)
                        throw new Exception("GetLifetime error,create lifetime instance null, type: " + lifetimeType.FullName);
                    lifetime.SetValue(result);
                }
                else
                {

                    var objType = targetMethod.DeclaringType;

                    var types = new Type[] { targetMethod.ReturnType };
                    var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

                    var lifetimeMethod = objType.GetMethod(lifetimeMethodName, types, bindingFlags);
                    if (lifetimeMethod == null)
                        throw new Exception(objType.FullName + " not contains method: " + lifetimeMethodName);

                    lifetime = lifetimeMethod.Invoke(lifetimeMethod.IsStatic ? null : obj, new object[] { result }) as ILifetime;
                    if (lifetime == null)
                        throw new Exception("GetLifetime error,create lifetime instance null, method: " + lifetimeMethod.Name);
                }

                return lifetime;
            }



        }




        class CachingResult
        {
            public ICallReturn Result;
            public ILifetime Lifetime;

            public CachingResult(ICallReturn result, ILifetime lifetime)
            {
                this.Result = result;
                this.Lifetime = lifetime;
            }
        }
    }



}
