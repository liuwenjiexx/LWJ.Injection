/**************************************************************
 *  Filename:    MethodAttributeMatchRule.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Injection.Aop
{


    /// <summary>
    /// match method define attributeType
    /// </summary>
    public class MethodAttributeMatchRule : ICallMatchRule
    {
        private Type attributeType;
        private bool inherit;


        public MethodAttributeMatchRule(Type attributeType, bool inherit)
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
            return method.IsDefined(AttributeType, Inherit);
        }

    }

}
