/**************************************************************
 *  Filename:    IThrowsCall.cs
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
    public interface IThrowsCall
    {
        void ThrowsInvoke(ICallInvocation invocation, Exception exception);
    }

}
