/**************************************************************
 *  Filename:    NamedValue.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.ObjectBuilder
{
    public class NamedValue : IBuilderValue
    {
        private string name;
        private object value;

        public NamedValue(string name, object value)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("parameter is empty", nameof(name));
           
            this.value = value;
        }

        public object GetValue(Type type, string name)
        {
            object value = this.value;

            if (value == null)
            {
                if (type.IsValueType)
                    value = Activator.CreateInstance(type);
            }
            else
            {
                if (!type.IsAssignableFrom(value.GetType()))
                {
                    try
                    {
                        if (type.IsEnum)
                            value = Enum.Parse(type, value as string);
                        else
                            value = Convert.ChangeType(value, type);
                    }
                    catch
                    {
                        throw new BuilderValueInvalidCastException(type, name, this.value.GetType());
                    }
                }
            }

            return value;
        }

        public bool IsMatchValue(Type type, string name)
        {
            if (name == this.name)
                return true;
            return false;
        }


    }
}
