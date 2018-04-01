/**************************************************************
 *  Filename:    CustomProxyBuilder.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic; 
using LWJ.ObjectBuilder;

namespace LWJ.Proxies
{
    public class CustomProxyBuilder : IBuilder 
    {
        private Type proxyType;
        private Type proxyServerType;

        private bool isTransparentProxy;

        public CustomProxyBuilder(Type proxyType, Type proxyServerType)
        {
            this.proxyType = proxyType;
            if (proxyServerType == null)
                proxyServerType = typeof(ProxyServer);
            this.proxyServerType = proxyServerType;
            isTransparentProxy = IsTransparentProxy(proxyType);
        }

        public object Build(IBuilderContext context, GetNextBuilderDelegate next)
        {
            object target = next()(context, next);

            var proxyServer = context.CreateInstance(proxyServerType, null,
                new IBuilderValue[] { new TypedValue(typeof(object), null, target) }) as ProxyServer;
             
            if (isTransparentProxy)
            {
                lock (TransparentProxyAttribute.lockProxyServerField)
                {   
                    TransparentProxyAttribute.proxyServer = proxyServer;
                    try
                    {
                        //error
                        //return injector.CreateInstance(proxyType, null,
                        //    new IInjectValue[] {
                        //new InjectValue(typeof(InjectProxy), null, proxy)
                        //    });
                        target = Activator.CreateInstance(proxyType);
                    }
                    finally
                    {
                        TransparentProxyAttribute.proxyServer = null;
                    }
                }
            }
            else
            {
                List<IBuilderValue> values = new List<IBuilderValue>(context.Values);
                values.Add(new TypedValue(typeof(ProxyServer), null, proxyServer));
                values.Add(new TypedValue(proxyServerType, null, proxyServer));
                values.Add(new TypedValue(context.TargetType, null, target));

                target = context.CreateInstance(proxyType, null, values.ToArray());
            }

            return target;
        }

        bool IsTransparentProxy(Type proxyType)
        {
            if (proxyType == null) throw new ArgumentNullException(nameof(proxyType));

            if (proxyType.IsDefined(typeof(TransparentProxyAttribute), true) &&
              proxyType.IsSubclassOf(typeof(ContextBoundObject)))
            {
                return true;
            }

            return false;
        }
    }
}
