/**************************************************************
 *  Filename:    PropertyBuilder.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.ObjectBuilder
{

    public class PropertyBuilder : IBuilderMember
    {
        private PropertyInfo property;
        private BuilderParameterInfo parameterInfo;
         

        public PropertyBuilder(PropertyInfo property, BuilderParameterInfo parameterInfo)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (property.GetSetMethod(true) == null)
                throw new ArgumentException(property.Name + " property not contains set method");
            if (parameterInfo == null)
                throw new ArgumentNullException(nameof(parameterInfo));
            this.property = property;
            this.parameterInfo = parameterInfo;
        }


        public PropertyInfo Property
        {
            get { return property; }
        }

        public object Invoke(IBuilderContext context, object target, IBuilderValue[] values)
        {
            var method = property.GetSetMethod(true);
            var value = context.GetValue(parameterInfo, values);
            object ret;
            ret = method.Invoke(method.IsStatic ? null : target, new object[] { value });
            return ret;
        }
    }
}
