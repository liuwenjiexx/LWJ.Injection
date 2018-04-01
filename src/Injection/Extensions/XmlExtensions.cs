/**************************************************************
 *  Filename:    InternalExtensions.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LWJ
{
    internal static partial class InternalExtensions
    {/*
        public static string GetAttributeValue(this XmlNode node, string name, string defaultValue)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var attr = node.Attributes.GetNamedItem(name);
            if (attr == null)
                return defaultValue;
            return attr.Value;
        }
        public static T GetAttributeValue<T>(this XmlNode node, string name, T defaultValue)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var attr = node.Attributes.GetNamedItem(name);
            if (attr == null)
                return defaultValue;
            return (T)ChangeType(attr.Value, typeof(T));
        }
        public static T GetAttributeValue<T>(this XmlNode node, string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var attr = node.Attributes.GetNamedItem(name);
            if (attr == null) throw new Exception("xml node not contains attribute, name:<{0}>".FormatArgs(name));

            return (T)ChangeType(attr.Value, typeof(T));
        }
        public static string GetAttributeValue(this XmlNode node, string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var attr = node.Attributes.GetNamedItem(name);
            if (attr == null) throw new Exception("xml node not contains attribute, name:<{0}>".FormatArgs(name));

            return attr.Value;
        }
        */
        public static object ChangeType(object value, Type targetType)
        {
            if (targetType == typeof(Type))
            {
                if (value is string)
                    return Type.GetType(value as string, true);
            }

            return Convert.ChangeType(value, targetType);
        }

    }
}
