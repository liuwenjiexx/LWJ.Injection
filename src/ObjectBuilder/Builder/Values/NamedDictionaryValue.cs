/**************************************************************
 *  Filename:    NamedDictionaryValue.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;

namespace LWJ.ObjectBuilder
{
    public class NamedDictionaryValue : IBuilderValue
    {
        private IDictionary<string, object> dic;
        public NamedDictionaryValue(IDictionary<string, object> dic)
        {
            if (dic == null || dic.Count == 0)
                this.dic = null;
            else
                this.dic = new Dictionary<string, object>(dic);
        }

        public object GetValue(Type type, string name)
        {
            var dic = this.dic;
            if (dic == null)
                throw new  InvalidOperationException("not name value, name:<0>".FormatArgs(name));
            object value;
            value = dic[name];
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
                        throw new BuilderValueInvalidCastException(type, name, value.GetType());
                    }
                }
            }
            return value;
        }

        public bool IsMatchValue(Type type, string name)
        {
            var dic = this.dic;
            if (dic.ContainsKey(name))
                return true;
            return false;
        }
    }
}
