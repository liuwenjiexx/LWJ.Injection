/**************************************************************
 *  Filename:    AopServerAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Aop
{

    /// <summary>
    /// Aop server class usage, proxy class usage [TransparentProxyAttribute] or inject [InjectAttribute] ProxyServer member;
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AopServerAttribute : CustomProxyAttribute
    {
        public AopServerAttribute(Type proxyType)
            : base(proxyType, typeof(AopServer))
        {
            
        }



    }
}
