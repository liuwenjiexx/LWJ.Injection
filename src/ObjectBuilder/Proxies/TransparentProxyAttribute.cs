/**************************************************************
 *  Filename:    TransparentProxyAttribute.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.Object ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Proxies;

namespace LWJ.Proxies
{
    /// <summary>
    /// class need inhert ContextBoundObject
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TransparentProxyAttribute : ProxyAttribute
    {

        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            ProxyServer proxyInvoker;
            TransparentProxy proxy;
            lock (TransparentProxyAttribute.lockProxyServerField)
            {
                proxyInvoker = TransparentProxyAttribute.proxyServer;
                if (proxyInvoker != null)
                {
                    proxy = new TransparentProxy(serverType, proxyInvoker);
                }
                else
                {
                    proxy = new TransparentProxy(serverType);
                }
            }
            var transparentProxy = proxy.GetTransparentProxy() as MarshalByRefObject;
            return transparentProxy;
        }

        public override RealProxy CreateProxy(ObjRef objRef, Type serverType, object serverObject, Context serverContext)
        {
            TransparentProxy proxy = new TransparentProxy(serverType);
            if (serverContext != null)
                RealProxy.SetStubData(proxy, serverContext);
            return proxy;
        }
        public static readonly object lockProxyServerField = new object();
        public static ProxyServer proxyServer;
         

    }

}
