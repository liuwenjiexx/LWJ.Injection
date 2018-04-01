/**************************************************************
 *  Filename:    InjectionNotTypeRegsteredException.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.Injection
{
    public class InjectionNotTypeRegsteredException : InjectionException
    {
        private Type interfaceType;
        private string interfaceName;
        public InjectionNotTypeRegsteredException(Type interfaceType, string interfaceName)
            : this(interfaceType, interfaceName, Resource1.Injection_NotTypeRegistered)
        { }

        public InjectionNotTypeRegsteredException(Type interfaceType, string interfaceName, string message)
            : base(message)
        {
            this.interfaceType = interfaceType;
            this.interfaceName = interfaceName;
        }

        public Type InterfaceType { get => interfaceType; }
        public string InterfaceName { get => interfaceName; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (interfaceType != null)
                    message += Environment.NewLine + Resource1.Injection_Register_InterfaceType.FormatArgs(interfaceType);
                if (!string.IsNullOrEmpty(interfaceName))
                    message += Environment.NewLine + Resource1.Injection_Register_InterfaceName.FormatArgs(interfaceName);
                return message;
            }
        }

    }
}
