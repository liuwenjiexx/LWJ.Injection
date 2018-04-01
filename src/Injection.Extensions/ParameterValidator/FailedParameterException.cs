/**************************************************************
 *  Filename:    FailedParameterException.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Injection.Aop.ParameterValidator
{

    public class FailedParameterException : Exception
    {

        public FailedParameterException(string validatorType, ParameterInfo parameterInfo, object value)
            : this(validatorType, parameterInfo, value, Resource1.ParamValid_FailedParam)
        {
        }
        public FailedParameterException(string validatorType, ParameterInfo parameterInfo, object value, string message)
            : base(message)
        {
            this.ValidatorType = validatorType;
            this.ParameterInfo = parameterInfo;
            this.Value = value;
        }


        public ParameterInfo ParameterInfo { get; private set; }

        public object Value { get; private set; }

        public string ValidatorType { get; private set; }


        public override string Message
        {
            get
            {
                string message = base.Message;

                if (!string.IsNullOrEmpty(ValidatorType))
                    message += Environment.NewLine + Resource1.ParamValid_Type.FormatArgs(ValidatorType);

                if (ParameterInfo != null)
                    message += Environment.NewLine + Resource1.ParamValid_ParamName.FormatArgs(ParameterInfo.Name);

                if (Value != null)
                    message += Environment.NewLine + Resource1.ParamValid_ParamValue.FormatArgs(Value);

                return message;
            }
        }

    }


}
