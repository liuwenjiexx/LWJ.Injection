#if DEBUG
/**************************************************************
 * Filename:    PerformanceTimeAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Aop.Performance
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PerformanceTimeAttribute : Attribute
    {
        public string Method { get; set; }
    }
}
#endif