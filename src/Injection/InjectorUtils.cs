/**************************************************************
 *  Filename:    InjectorUtils.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.Injection
{

    internal static class InjectorUtils
    {
        
         

        internal static BuilderParameterInfo FromAttributeProvider(ICustomAttributeProvider provider, Type defaultType, string defaultName = null)
        {
            var valueAttr = provider.GetCustomAttribute<InjectValueAttribute>(true);
            var defaultAttr = provider.GetCustomAttribute<DefaultValueAttribute>(true);
            Type type = null;
            string name = null;
            bool hasDefaultValue = false;
            object defaultValue = null;

            if (valueAttr != null)
            {
                type = valueAttr.Type;
                name = valueAttr.Name;

            }
            if (defaultAttr != null)
            {
                defaultValue = defaultAttr.DefaultValue;
                hasDefaultValue = true;
            }
            if (type == null)
                type = defaultType;
            if (string.IsNullOrEmpty(name))
                name = defaultName;
            BuilderParameterInfo p;
            if (hasDefaultValue)
                p = BuilderParameterInfo.MakeDefault(type, name, defaultValue);
            else
                p = new BuilderParameterInfo(type, name);

            return p;
        }

        public static BuilderParameterInfo[] FromMethod(MethodBase methodBase)
        {

            ParameterInfo[] parameters = methodBase.GetParameters();
            if (parameters.Length > 0)
            {
                BuilderParameterInfo[] injectParams = new BuilderParameterInfo[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = parameters[i];
                    var injectParam = FromAttributeProvider(p, p.ParameterType);

                    injectParams[i] = injectParam;
                }
                return injectParams;
            }
            return BuilderParameterInfo.EmptyArray;
        }


        public static MethodInfo FindMethod(Type type, string methodName, object[] args, BindingFlags bindingFlags)
        {
            return FindMethod(type, methodName, args.ToTypes(), bindingFlags);
        }
        public static MethodInfo FindMethod(Type type, string methodName, Type[] argTypes, BindingFlags bindingFlags)
        {
            return FindMethod(type.GetMethods(bindingFlags).Select(o => (MethodBase)o).Where(o => o.Name == methodName), argTypes) as MethodInfo;
        }
        public static ConstructorInfo FindConstructor(Type type, object[] args)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance;
            return FindConstructor(type, args, bindingFlags);
        }
        public static ConstructorInfo FindConstructor(Type type, object[] args, BindingFlags bindingFlags)
        {
            return FindConstructor(type, args.ToTypes(), bindingFlags);
        }

        public static ConstructorInfo FindConstructor(Type type, Type[] argTypes, BindingFlags bindingFlags)
        {
            return FindMethod(type.GetConstructors(bindingFlags).Select(o => (MethodBase)o), argTypes) as ConstructorInfo;
        }

        public static MethodBase FindMethod(IEnumerable<MethodBase> methods, Type[] argTypes)
        {
            MethodBase method = null;
            foreach (var m in methods)
            {
                if (m.IsMatch(argTypes))
                {
                    method = m;
                    break;
                }
            }
            return method;
        }





    }
}
