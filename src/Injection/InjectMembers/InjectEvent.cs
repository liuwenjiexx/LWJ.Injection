/**************************************************************
 *  Filename:    InjectEvent.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;

namespace LWJ.Injection
{

    public class InjectEvent : IInjectMember
    {
        private string eventName;
        private object value;
        private bool hasValue;
        public InjectEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentException(Resource1.Argument_NullOrEmpty, nameof(eventName));

            this.eventName = eventName;
            this.hasValue = false;
        }
        public InjectEvent(string eventName, object value)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentException(Resource1.Argument_NullOrEmpty, nameof(eventName));

            this.eventName = eventName;
            this.value = value;
            hasValue = true;
        }

        public void AddBuilderMember(Type targetType, List<IBuilderMember> members)
        {
            var eventInfo = targetType.GetEvent(eventName);
            if (eventInfo == null)
                throw new InjectionException(string.Format("type <{0}> not found  event <{1}>", targetType.FullName, eventName));
            if (eventInfo.GetAddMethod(true) == null)
                throw new InjectionException(string.Format("type <{0}> event <{1}> can't add", targetType.FullName, eventName));

            BuilderParameterInfo paramInfo;
            if (hasValue)
                paramInfo =  BuilderParameterInfo.MakeValue(eventInfo.EventHandlerType, null, value);
            else
                paramInfo = InjectorUtils.FromAttributeProvider(eventInfo, eventInfo.EventHandlerType);
             
            members.Add(new EventBuilder(eventInfo, paramInfo));
        }
    }

}
