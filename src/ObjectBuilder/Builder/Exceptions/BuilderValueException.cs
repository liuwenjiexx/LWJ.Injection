/**************************************************************
 *  Filename:    BuilderValueException.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.ObjectBuilder
{
    public class BuilderValueException : BuilderException
    {

        private Type valueType;
        private string valueName;
        public BuilderValueException(string message, Type valueType, string valueName)
            : base(message)
        {
            this.valueType = valueType;
            this.valueName = valueName;
        }

        public BuilderValueException(string message)
            : base(message)
        {
        }



        public Type ValueType { get => valueType; }

        public string ValueName { get => valueName; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (valueType != null)
                    message += Environment.NewLine + Resource1.BuilderValue_Type.FormatArgs(valueType);

                if (!string.IsNullOrEmpty(valueName))
                    message += Environment.NewLine + Resource1.BuilderValue_Name.FormatArgs(valueName);

                return message;
            }
        }
    }
}
