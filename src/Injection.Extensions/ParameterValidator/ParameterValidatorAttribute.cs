/**************************************************************
 *  Filename:    ParameterValidatorAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Injection.Aop.ParameterValidator
{

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public abstract class ParameterValidatorAttribute : Attribute
    {
        protected ParameterValidatorAttribute()
        {
        }

        public abstract IParameterValidator CreateValidator(ParameterInfo parameter);

    }


}
