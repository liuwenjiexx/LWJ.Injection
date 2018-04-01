/**************************************************************
 *  Filename:    MethodBuilder.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.ObjectBuilder
{

    public class MethodBuilder : IBuilderMember
    {
        private MethodBase methodBase;

        protected BuilderParameterInfo[] parameterInfos;
          
        public MethodBuilder(MethodBase methodBase, BuilderParameterInfo[] args)
        {
            this.methodBase = methodBase ?? throw new ArgumentNullException(nameof(methodBase));
            this.parameterInfos = args;
        }

        public MethodBase MethodBase
        {
            get { return methodBase; }
        }

        public virtual object Invoke(IBuilderContext context, object target, IBuilderValue[] values)
        {
            var args = context.GetValues(parameterInfos, values);
            object ret;
            ret = methodBase.Invoke(methodBase.IsStatic ? null : target, args);

            return ret;
        }
    }

}
