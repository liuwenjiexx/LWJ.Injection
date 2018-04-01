/**************************************************************
 *  Filename:    ICallHandler.cs
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

    public interface ICallHandler
    {
        int Order { get; set; }

        //  bool CanHandleType(Type type);

        ICallReturn Invoke(ICallInvocation invocation,GetNextCallHandlerDelegate getNext);

    }

    public delegate ICallReturn NextHandlerDelegate(ICallInvocation invocation,GetNextCallHandlerDelegate getNext);
     

    public delegate NextHandlerDelegate GetNextCallHandlerDelegate();

}
