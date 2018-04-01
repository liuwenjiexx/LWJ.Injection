/**************************************************************
 *  Filename:    AopException.cs
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
    public class AopException : Exception
    {
        public AopException()
            : base()
        { }

        public AopException(string message)
            : base(message)
        { }

        public AopException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }

}
