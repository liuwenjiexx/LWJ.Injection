/**************************************************************
 *  Filename:    AopInvocation.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;
using System.Collections.Generic; 
using System.Reflection;

namespace LWJ.Injection.Aop
{

    internal class AopInvocation : ICallInvocation
    {

        private object proxy;
        private object target;
        private MethodBase methodBase;
        private IParameterCollection arguments;
        private IDictionary extraData;
        private IDictionary data;
        private IInjector injector;
        public AopInvocation(IInjector injector, object proxy, object target, MethodBase method, IParameterCollection arguments, IDictionary extraData)
        {
            this.proxy = proxy;
            this.arguments = arguments;
            this.methodBase = method;
            this.target = target;

            this.extraData = extraData;
            this.injector = injector;
        }


        public object Proxy
        {
            get { return proxy; }
        }

        public object Target
        {
            get { return target; }
        }
        public MethodBase MethodBase
        {
            get { return methodBase; }
        }

        public IParameterCollection Arguments
        {
            get { return arguments; }
        }

        public IDictionary ExtraData
        {
            get
            {
                if (extraData == null)
                    extraData = new Dictionary<object, object>();
                return extraData;
            }
        }


        public IDictionary Data
        {
            get
            {
                if (data == null)
                    data = new Dictionary<object, object>();
                return data;
            }
        }

        public ICallReturn ReturnException(Exception exception)
        {
            ICallReturn ret = new CallReturn(exception);
            return ret;
        }

        public ICallReturn Return(object returnValue, params object[] outputs)
        {
            ICallReturn ret = new CallReturn(returnValue);
            return ret;
        }


        public bool TryGetValue(Type valueType, string name, out object value)
        {
            if (valueType == null)
                throw new ArgumentNullException(nameof(valueType));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var dic = extraData;
            if (dic != null)
            {
                if (dic.Contains(name))
                {
                    value = dic[name];
                    if (value != null && valueType.IsAssignableFrom(value.GetType()))
                        return true;
                    value = null;
                }
            }

            if (injector.TryGetValue(valueType, name, out value))
                return true;

            value = null;
            return false;
        }


    }

}
