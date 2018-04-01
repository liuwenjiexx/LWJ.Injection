/**************************************************************
 *  Filename:    NotNullAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Injection.Aop.ParameterValidator
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class NotNullAttribute : ParameterValidatorAttribute
    {
        public NotNullAttribute()
        {
        }

        public override IParameterValidator CreateValidator(ParameterInfo parameter)
        {
            return NotNullValidator.instance;
        }


        internal class NotNullValidator : IParameterValidator
        {
            public static NotNullValidator instance = new NotNullValidator();

            public bool Validate(object value)
            {
                return value != null;
            }
            public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
            {
                return new FailedNotNullException(parameterInfo);
            }
        }
    }


    public class FailedNotNullException : FailedParameterException
    {

        public FailedNotNullException(ParameterInfo parameterInfo)
            : base(Resource1.ParamValid_NotNull_Type, parameterInfo, null)
        {
        }

    }

}
