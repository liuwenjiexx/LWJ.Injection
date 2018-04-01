/**************************************************************
 *  Filename:    AssignableFromTypeAttribute.cs
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
    public class AssignableFromTypeAttribute : ParameterValidatorAttribute
    {
        public AssignableFromTypeAttribute(Type type)
        {
            this.AssignableFromType = type;
        }

        public Type AssignableFromType { get; set; }

        public override IParameterValidator CreateValidator(ParameterInfo parameter)
        {
            return new AssignableFromTypeValidator(AssignableFromType);
        }
        const string AssignableTypeParameterName = "assignableType";

        static internal IParameterValidator CreateValidator(IDictionary<string, object> parameters)
        {
            Type assignableType;

            assignableType = Utils.GetParameter<Type>(parameters, AssignableTypeParameterName);
            return new AssignableFromTypeValidator(assignableType);
        }

        internal class AssignableFromTypeValidator : IParameterValidator
        {

            private Type type;

            public AssignableFromTypeValidator(Type type)
            {
                this.type = type;
            }

            public bool Validate(object value)
            {
                if (value == null)
                    return false;
                Type valueType;
                if (value is Type)
                    valueType = (Type)value;
                else
                    valueType = value.GetType();
                return type.IsAssignableFrom(valueType);
            }
            public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
            {
                return new FailedAssignableFromTypeException(parameterInfo, value, type);
            }
        }
    }

    public class FailedAssignableFromTypeException : FailedParameterException
    {
        public FailedAssignableFromTypeException(ParameterInfo parameterInfo, object value, Type type)
                        : base(Resource1.ParamValid_AssignFromType_Type, parameterInfo, value)
        {
            this.AssignableFromType = type;
        }

        public Type AssignableFromType { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (AssignableFromType != null)
                    message += Environment.NewLine + Resource1.ParamValid_AssignFromType_FromType.FormatArgs(AssignableFromType)  ;
                return message;
            }
        }

    }
}
