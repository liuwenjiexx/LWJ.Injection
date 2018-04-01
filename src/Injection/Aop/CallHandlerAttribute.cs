/**************************************************************
 *  Filename:    CallHandlerAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Aop
{

    /// <summary>
    /// usage method have ICallHandler  behaviour
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class CallHandlerAttribute : Attribute
    {
        //public int Order
        //{
        //    get;
        //    set;
        //}

        //public abstract ICallHandler CreateCallHander();
    }
}
