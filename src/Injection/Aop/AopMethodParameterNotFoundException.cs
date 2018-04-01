/**************************************************************
 *  Filename:    AopMethodParameterNotFoundException.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Aop
{
    public class AopMethodParameterNotFoundException : AopException
    {
        private Type classType;
        private string methodName;
        private string parameterName;

        public AopMethodParameterNotFoundException(Type classType, string methodName, string parameterName)
            : base(Resource1.Aop_Method_Parameter_NotFound)
        {
            this.classType = classType;
            this.methodName = methodName;
            this.parameterName = parameterName;
        }

        public Type ClassType { get => classType; }
        public string MethodName { get => methodName; }

        public string ParameterName { get => parameterName; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (classType != null)
                    message += Environment.NewLine + Resource1.Aop_Method_ClassType.FormatArgs(classType);

                if (!string.IsNullOrEmpty(methodName))
                    message += Environment.NewLine + Resource1.Aop_Method_MethodName.FormatArgs(methodName);

                if (!string.IsNullOrEmpty(parameterName))
                    message += Environment.NewLine + Resource1.Aop_Parameter_ParameterName.FormatArgs(parameterName);

                return message;
            }
        }
    }
}
