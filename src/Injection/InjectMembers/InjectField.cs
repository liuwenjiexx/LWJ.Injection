/**************************************************************
 *  Filename:    InjectField.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;

namespace LWJ.Injection
{
    public class InjectField : IInjectMember
    {
        private string fieldName;
        private object value;
        private bool hasValue;

        public InjectField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException(Resource1.Argument_NullOrEmpty, nameof(fieldName));

            this.fieldName = fieldName;
            this.hasValue = false;
        }

        public InjectField(string fieldName, object value)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException(Resource1.Argument_NullOrEmpty, nameof(fieldName));

            this.fieldName = fieldName;
            this.value = value;
            this.hasValue = true;
        }


        public void AddBuilderMember(Type targetType, List<IBuilderMember> members)
        {
            var field = targetType.GetField(fieldName);
            //var field = targetType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField| BindingFlags.CreateInstance| BindingFlags.Static| BindingFlags.GetField);
            if (field == null)
                throw new InjectionException(string.Format("type <{0}> not found  field <{1}>", targetType.FullName, fieldName));
            if (field.IsInitOnly)
                throw new InjectionException(string.Format("type <{0}> field <{1}> is can't write", targetType.FullName, fieldName));
            BuilderParameterInfo paramInfo;
            if (hasValue)
                paramInfo =   BuilderParameterInfo.MakeValue(field.FieldType, null, value);
            else
                paramInfo = InjectorUtils.FromAttributeProvider(field, field.FieldType); 

            members.Add(new FieldBuilder(field, paramInfo));
        }
    }



}
