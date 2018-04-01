/**************************************************************
 *  Filename:    InjectMethod.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LWJ.Injection
{


    public class InjectMethod : IInjectMember
    {
        private string methodName;
        private object[] args;

        public InjectMethod(string methodName, params object[] args)
        {
            this.methodName = methodName;
            this.args = args; 
        }

        public string MethodName
        {
            get { return methodName; }
        }

        public object[] Args
        {
            get { return args; }
        }

 
        public void AddBuilderMember(Type targetType, List<IBuilderMember> members)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;

            var injectParams = BuilderParameterInfo.FromArgs(args);

            var methodBase = InjectorUtils.FindMethod(targetType, methodName, args, bindingFlags);
            if (methodBase == null)
                throw new InjectionException(string.Format("type <{0}> not found  method <{1}>", targetType, methodName));
            members.Add(new MethodBuilder(methodBase, injectParams));
        }

    }


}
