/**************************************************************
 *  Filename:    InjectProperty.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic; 

namespace LWJ.Injection
{

    public class InjectProperty : IInjectMember
    {
        private string propertyName;
        private object value;
        private bool hasValue;
        public InjectProperty(string propertyName)
        {
            this.propertyName = propertyName;
            this.hasValue = false;
        }
        public InjectProperty(string propertyName, object value)
        {
            this.propertyName = propertyName;
            this.value = value;
            this.hasValue = true;
        }


        public void AddBuilderMember(Type targetType, List<IBuilderMember> members)
        {
            var property = targetType.GetProperty(propertyName);
            if (property == null)
                throw new InjectionException(string.Format("type <{0}> not found  property <{1}>", targetType.FullName, propertyName));
            if (!property.CanWrite)
                throw new InjectionException(string.Format("type <{0}> property <{1}> can't write", targetType.FullName, propertyName));
            BuilderParameterInfo paramInfo;

            if (hasValue)
                paramInfo =  BuilderParameterInfo.MakeValue(property.PropertyType, null, value);
            else
                paramInfo = InjectorUtils.FromAttributeProvider(property, property.PropertyType);
            

            members.Add(new PropertyBuilder(property, paramInfo));
        }
    }



}
