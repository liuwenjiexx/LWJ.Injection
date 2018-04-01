/**************************************************************
 *  Filename:    BuilderValueInvalidCastException.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.ObjectBuilder
{

    public class BuilderValueInvalidCastException : BuilderValueException
    {
        private Type fromType;


        public BuilderValueInvalidCastException(Type valueType, string valueName, Type fromType)
            : this(valueType, valueName, fromType, Resource1.BuilderValue_InvalidCast)
        {
        }

        public BuilderValueInvalidCastException(Type valueType, string valueName, Type fromType, string message)
            : base(message, valueType, valueName)
        {
            this.fromType = fromType;
        }



        public Type FromType { get => fromType; }

        public override string Message
        {
            get
            {
                string message = base.Message;

                if (fromType != null)
                    message += Environment.NewLine + Resource1.BuilderValue_InvalidCast_FromType.FormatArgs(fromType);
                return message;
            }
        }

    }
}
