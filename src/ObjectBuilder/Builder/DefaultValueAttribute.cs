/**************************************************************
 *  Filename:    DefaultValueAttribute.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.ObjectBuilder
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class DefaultValueAttribute:Attribute
    {
        private object defaultValue;

        public DefaultValueAttribute(object defaultValue)
        {
            this.DefaultValue = defaultValue;
        }

        public object DefaultValue { get => defaultValue; set => this.defaultValue = value; }
    }
}
