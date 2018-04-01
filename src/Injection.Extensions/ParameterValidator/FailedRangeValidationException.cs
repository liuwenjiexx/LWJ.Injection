/**************************************************************
 *  Filename:    FailedRangeException.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Injection.Aop.ParameterValidator
{

    public class FailedRangeException : FailedParameterException
    {

        public object Min { get; private set; }

        public object Max { get; private set; }


        public FailedRangeException(ParameterInfo parameterInfo, object value, object min, object max)
            : base(Resource1.ParamValid_Range_Type, parameterInfo, value)
        {
            this.Min = min;
            this.Max = max;
        }

        public override string Message
        {
            get
            {
                string message = base.Message;
                message += Environment.NewLine + Resource1.ParamValid_Range_Min.FormatArgs(Min);
                message += Environment.NewLine + Resource1.ParamValid_Range_Max.FormatArgs(Max);
                return message;
            }
        }


    }


}
