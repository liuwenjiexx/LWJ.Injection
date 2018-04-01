/**************************************************************
 *  Filename:    ICallInvocation.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.Injection.Aop
{



    public interface ICallInvocation
    {
 

        IParameterCollection Arguments { get; }
        //IDictionary Properties
        //ArgumentLength
        IDictionary  ExtraData { get; }
        IDictionary  Data { get; }
        object Proxy { get; }

        object Target { get; }

        MethodBase MethodBase { get; }

        //MethodSignature
        //MethodName

        ICallReturn Return(object returnValue, params object[] outputs);

        ICallReturn ReturnException(Exception ex);

        bool TryGetValue(Type valueType, string name, out object value);

    }

}
