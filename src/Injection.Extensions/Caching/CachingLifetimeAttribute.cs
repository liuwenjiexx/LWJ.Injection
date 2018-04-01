/**************************************************************
 *  Filename:    CachingLifetimeAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.Injection.Aop.Caching
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachingLifetimeAttribute : Attribute
    {
        public CachingLifetimeAttribute()
        {
        }

        public CachingLifetimeAttribute(Type lifetimeType)
        {
            this.LifetimeType = lifetimeType;
        }
        public CachingLifetimeAttribute(string method)
        {
            this.Method = method;
        }

        public Type LifetimeType { get; set; }

        public string Method { get; set; }

    }



}
