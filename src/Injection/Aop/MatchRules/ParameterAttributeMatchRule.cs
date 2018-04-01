/**************************************************************
 *  Filename:    ParameterAttributeMatchRule.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.Injection.Aop
{
    /// <summary>
    /// match method parameter define attributeType
    /// </summary>
    public class ParameterAttributeMatchRule : ICallMatchRule
    {
        private Type attributeType;
        private bool inherit;
        public ParameterAttributeMatchRule(Type attributeType, bool inherit)
        {
            this.attributeType = attributeType;
            this.inherit = inherit;
        }

        public Type AttributeType
        {
            get { return attributeType; }
        }

        public bool Inherit
        {
            get { return inherit; }
        }

        public bool IsCallMatch(MethodBase method)
        {
            foreach (var p in method.GetParameters())
            {
                if (p.IsDefined(attributeType, inherit))
                    return true;
            }
            return false;
        }
    }


}
