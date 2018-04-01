/**************************************************************
 *  Filename:    ConstructorBuilder.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.ObjectBuilder
{

    public class ConstructorBuilder : IBuilderMember
    {
        private ConstructorInfo constructor;
        private BuilderParameterInfo[] parameterInfos;

        public ConstructorBuilder(ConstructorInfo constructor, BuilderParameterInfo[] parameterInfos)
        {

            this.constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
            this.parameterInfos = parameterInfos;
        }


        public ConstructorInfo Constructor
        {
            get { return constructor; }
        }

        public object Invoke(IBuilderContext context, object target, IBuilderValue[] values)
        {
            var args = context.GetValues(parameterInfos, values);
            object ret;

            ret = constructor.Invoke(args);

            return ret;
        }


    }

}
