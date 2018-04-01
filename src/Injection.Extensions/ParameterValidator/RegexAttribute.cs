/**************************************************************
 *  Filename:    RegexAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace LWJ.Injection.Aop.ParameterValidator
{

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class RegexAttribute : ParameterValidatorAttribute
    {
        public RegexAttribute(string regex)
           : this(regex, true)
        {
        }
        public RegexAttribute(string regex, bool ignoreCase)
        {
            this.RegexString = regex;
            this.IgnoreCase = ignoreCase;
        }

        public string RegexString { get; set; }

        public bool IgnoreCase { get; set; }


        //AllowNull
        //AllowEmpty


        public override IParameterValidator CreateValidator(ParameterInfo parameter)
        {
            return new RegexValidator(RegexString, IgnoreCase);
        }
        internal static IParameterValidator CreateRegexValidator(IDictionary<string, object> parameters)
        {
            string regexString;
            bool ignoreCase;

            regexString = Utils.GetParameter<string>(parameters, "regex");
            ignoreCase = Utils.GetParameter<bool>(parameters, "ignoreCase", false);

            return new RegexValidator(regexString, ignoreCase);
        }

        private class RegexValidator : IParameterValidator
        {
            private Regex regex;
            private string regexString;
            private bool ignoreCase;

            public RegexValidator(string regexString, bool ignoreCase)
            {
                if (regexString == null)
                    throw new ArgumentNullException(nameof(regexString));
                this.ignoreCase = ignoreCase;
                RegexOptions options = RegexOptions.None;
                if (ignoreCase)
                    options |= RegexOptions.IgnoreCase;
                regex = new Regex(regexString, options);
                this.regexString = regexString;
            }

            public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
            {
                return new FailedRegexException(parameterInfo, value, regexString, ignoreCase);
            }

            public bool Validate(object value)
            {
                string str = value as string;

                return regex.IsMatch(str);
            }

        }


    }













}
