/**************************************************************
 *  Filename:    ProxyMethodNotFoundException.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.Object ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System; 

namespace LWJ.Proxies
{
    public class ProxyMethodNotFoundException : ProxyException
    {
        private Type classType;
        private string methodName;

        public ProxyMethodNotFoundException(Type classType, string methodName)
            : base(Resource1.Proxy_Method_NotFound)
        {
            this.classType = classType;
            this.methodName = methodName;
        }

        public Type ClassType { get => classType; }
        public string MethodName { get => methodName; }

        public override string Message
        {
            get
            {
                string message = base.Message;

                if (classType != null)
                    message += Environment.NewLine + Resource1.Proxy_Method_ClassType.FormatArgs(classType);

                if (!string.IsNullOrEmpty(methodName))
                    message += Environment.NewLine + Resource1.Proxy_Method_MethodName.FormatArgs(methodName);

                return message;
            }
        }

    }
}
