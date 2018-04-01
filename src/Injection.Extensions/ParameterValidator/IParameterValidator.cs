/**************************************************************
 *  Filename:    IParameterValidator.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Injection.Aop.ParameterValidator
{

    public interface IParameterValidator
    {
        bool Validate(object value);

        FailedParameterException GetException(ParameterInfo parameterInfo, object value);

    }
}
