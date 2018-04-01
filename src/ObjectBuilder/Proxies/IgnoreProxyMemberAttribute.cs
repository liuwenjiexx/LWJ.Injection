/**************************************************************
 *  Filename:    IgnoreProxyMemberAttribute.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.Object ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.Proxies
{

    /// <summary>
    /// transparent proxy usage
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event, AllowMultiple = false, Inherited = true)]
    public class IgnoreProxyMemberAttribute : Attribute
    {

    }

}
