/**************************************************************
 *  Filename:    FieldBuilder.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.ObjectBuilder
{

    public class FieldBuilder : IBuilderMember
    {
        private FieldInfo field;
        private BuilderParameterInfo parameterInfo;
         
        public FieldBuilder(FieldInfo field, BuilderParameterInfo parameterInfo)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
            this.parameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));
        }


        public FieldInfo Field
        {
            get { return field; }
        }

        public object Invoke(IBuilderContext context, object target, IBuilderValue[] values)
        {
            object value = context.GetValue(parameterInfo, values);
            field.SetValue(field.IsStatic ? null : target, value);
            return null;
        }
    }
}
