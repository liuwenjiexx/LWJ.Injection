/**************************************************************
 *  Filename:    InjectionException.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic; 

namespace LWJ.Injection
{
    public class InjectionException : Exception
    {
        public InjectionException(string message)
            : base(message)
        { }

        public InjectionException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}
