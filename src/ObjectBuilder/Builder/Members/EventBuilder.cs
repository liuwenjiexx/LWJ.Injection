/**************************************************************
 *  Filename:    EventBuilder.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.ObjectBuilder
{

    public class EventBuilder : IBuilderMember
    {
        private EventInfo eventInfo;
        private BuilderParameterInfo parameterInfo;
          
        public EventBuilder(EventInfo eventInfo, BuilderParameterInfo parameterInfo)
        {
            this.eventInfo = eventInfo ?? throw new ArgumentNullException(nameof(eventInfo));
            this.parameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));
        }

        public EventInfo EventInfo { get => eventInfo; }


        public object Invoke(IBuilderContext context,   object target, IBuilderValue[] values)
        {
            var method = eventInfo.GetAddMethod(true);
            var value = context.GetValue(parameterInfo, values);
            object ret;
            ret = method.Invoke(method.IsStatic ? null : target, new object[] { value });
            return ret;
        }
    }
}
