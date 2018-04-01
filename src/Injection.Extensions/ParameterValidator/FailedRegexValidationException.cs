/**************************************************************
 *  Filename:    FailedRegexException.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System.Reflection;
using System;

namespace LWJ.Injection.Aop.ParameterValidator
{

    public class FailedRegexException : FailedParameterException
    {

        public FailedRegexException(ParameterInfo parameterInfo, object value, string regex, bool ignoreCase)
            : base(Resource1.ParamValid_Regex_Type, parameterInfo, value)
        {
            this.Regex = regex;
            this.IgnoreCase = ignoreCase;
        }

        public string Regex { get; private set; }
        public bool IgnoreCase { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;

                message += Environment.NewLine + Resource1.ParamValid_Regex_RegexString.FormatArgs(Regex);
                message += Environment.NewLine + Resource1.ParamValid_Regex_IgnoreCase.FormatArgs(IgnoreCase);

                return message;
            }
        }

    }

}
