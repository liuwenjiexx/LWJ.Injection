/**************************************************************
 *  Filename:    ProxyServer.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.Object ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Proxies
{

    public class ProxyServer
    {
        private object target;
        private Type targetType;
        private bool isAttached;

        protected ProxyServer()
        { }

        public ProxyServer(object target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            AttachTarget(target);
        }

        protected object Target
        {
            get
            {
                if (!isAttached)
                    throw new ProxyException(string.Format("not {0} target object", nameof(AttachTarget)));

                var target = this.target;
                if (target == null)
                    throw new ProxyException("targetobject is null");

                return target;
            }
        }
        protected Type TargetType { get => targetType; }
        protected bool IsAttached { get => isAttached; }

         
        public TReturn Invoke<TReturn>(string methodName, params object[] args)
        {
            return (TReturn)Invoke(methodName, args);
        }

        public virtual object Invoke(string methodName, params object[] args)
        {
            if (!isAttached)
                throw new ProxyException("proxy target not attach");

            var target = this.target;
            var type = this.targetType;

            Type[] argTypes;
            if (args == null || args.Length == 0)
                argTypes = Type.EmptyTypes;
            else
                argTypes = Type.GetTypeArray(args);
            var method = type.GetMethod(methodName, argTypes);
            if (method == null)
                throw new ProxyException("not found method, name:<{0}>".FormatArgs(methodName));
            object result;
            try
            {
                result = method.Invoke(target, args);
            }
            catch (TargetInvocationException ex)
            {
                throw new ProxyException("target invocation exception", ex.InnerException);
            }
            return result;
        }

        protected virtual void AttachTarget(object target)
        {
            if (IsAttached)
                throw new ProxyException("target already attached");

            this.target = target ?? throw new ArgumentNullException(nameof(target));
            this.targetType = target.GetType();
            //his.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));

            //if (!targetType.IsAssignableFrom(target.GetType()))
            //    throw new ArgumentException(string.Format("target type <{0}> not assignable from <{1}>", targetType, target.GetType()));

            isAttached = true;
        }

        protected virtual object DetachTarget()
        {
            if (!isAttached)
                throw new InvalidOperationException("target not attached");

            object target = this.target;
            this.target = null;
            this.targetType = null;
            return target;
        }

    }

}
