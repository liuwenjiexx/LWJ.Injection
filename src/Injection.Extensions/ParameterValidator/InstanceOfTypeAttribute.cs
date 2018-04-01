/**************************************************************
 *  Filename:    InstanceOfTypeAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LWJ.Injection.Aop.ParameterValidator
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class InstanceOfTypeAttribute : ParameterValidatorAttribute
    {
        public InstanceOfTypeAttribute(Type type)
        {
            this.InstanceOfType = type;
        }

        public Type InstanceOfType { get; set; }

        public override IParameterValidator CreateValidator(ParameterInfo parameter)
        {
            return new InstanceOfTypeValidator(InstanceOfType);
        }

        const string InstanceTypeParameterName = "instanceType";

        static internal IParameterValidator CreateValidator(IDictionary<string, object> parameters)
        {

            Type instanceType;

            instanceType = Utils.GetParameter<Type>(parameters, InstanceTypeParameterName);
            return new InstanceOfTypeValidator(instanceType);
        }

        internal class InstanceOfTypeValidator : IParameterValidator
        {

            public Type type;
            public InstanceOfTypeValidator(Type type)
            {
                this.type = type;
            }
            public bool Validate(object value)
            {
                if (value == null)
                    return false;

                return type.IsInstanceOfType(value);
            }
            public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
            {
                Type valueType;
                if (value == null)
                    valueType = null;
                else
                    valueType = value.GetType();
                return new FailedInstanceOfTypeException(parameterInfo, type, valueType);
            }
        }
    }


    public class FailedInstanceOfTypeException : FailedParameterException
    {
        public FailedInstanceOfTypeException(ParameterInfo parameterInfo, object value, Type type)
            : base(Resource1.ParamValid_InstanceOfType_Type, parameterInfo, value)
        {
            this.InstanceOfType = type;
        }

        public Type InstanceOfType { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (InstanceOfType != null)
                    message += Environment.NewLine + Resource1.ParamValid_InstanceOfType_OfType.FormatArgs(InstanceOfType);
                return message;
            }
        }

    }


}
