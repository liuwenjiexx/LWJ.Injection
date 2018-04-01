/**************************************************************
 *  Filename:    InternalExtensions.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace LWJ
{
    internal static partial class InternalExtensions
    {
        public static readonly object[] EmptyObjects = new object[0];
           
        public static string FormatArgs(this string source,params object[] args)
        {
            return string.Format(source, args);
        }
        public static MethodInfo GetMethod(this Type source, string name, Type[] types, BindingFlags bindingFlags)
        {

            if (types == null)
                types = Type.EmptyTypes;
            int len = types.Length;
            MethodInfo result = null;
            foreach (var m in source.GetMember(name, MemberTypes.Method, bindingFlags).Cast<MethodInfo>())
            {
                var argInfos = m.GetParameters();
                if (argInfos.Length == len)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (argInfos[i].ParameterType.IsAssignableFrom(types[i]))
                        {
                            result = m;
                            break;
                        }
                    }
                    if (result != null)
                        break;
                }
            }
            return result; 
        }
 

    }




}
