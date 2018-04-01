/**************************************************************
*  Filename:    CustomProxyAttribute.cs
*  Copyright:  © 2017 WenJie Liu. All rights reserved.
*  Description: LWJ.Object ClassFile
*  @author:     WenJie Liu
*  @version     2017/2/17
**************************************************************/
using System;

namespace LWJ.Proxies
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CustomProxyAttribute : Attribute
    {
        private Type proxyServerType;

        public CustomProxyAttribute(Type proxyType)
            : this(proxyType, typeof(ProxyServer))
        {
        }
        public CustomProxyAttribute(Type proxyType, Type proxyServerType)
        {

            this.ProxyType = proxyType ?? throw new ArgumentNullException(nameof(proxyType));
            this.proxyServerType = proxyServerType ?? throw new ArgumentNullException(nameof(proxyServerType));
            if (!typeof(ProxyServer).IsAssignableFrom(proxyServerType))
                throw new ArgumentException("not assignable type {0}".FormatArgs(nameof(ProxyServer)), nameof(proxyServerType));

        }

        public Type ProxyType { get; set; }

        public virtual Type GetProxyServerType()
        {
            return proxyServerType;
        }



    }
}
