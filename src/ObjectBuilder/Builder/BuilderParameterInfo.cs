/**************************************************************
 *  Filename:    BuilderParameterInfo.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.ObjectBuilder
{
    public sealed class BuilderParameterInfo
    {
        internal Type parameterType; 
        internal string name;
        internal object value;
        private bool hasValue;
        private object defaultValue;
        private bool hasDefaultValue;

        public BuilderParameterInfo(Type parameterType)
            : this(parameterType, null)
        {
        }

        public BuilderParameterInfo(Type parameterType, string name)            
        {
            this.parameterType = parameterType;
            this.name = name;
        }
         


        public Type ParameterType
        {
            get { return parameterType; }
        }

        public string Name
        {
            get { return name; }
        }

        public object Value
        {
            get { return value; }
        }

        public bool HasValue
        {
            get { return hasValue; }
        }

        public object DefaultValue { get => defaultValue; }

        public bool HasDefaultValue { get => hasDefaultValue; }


        public static BuilderParameterInfo NullValue = new BuilderParameterInfo(null);
        public static readonly BuilderParameterInfo[] EmptyArray = new BuilderParameterInfo[0];

        public static BuilderParameterInfo[] FromArgs(object[] args)
        {
            if (args == null || args.Length == 0)
                return EmptyArray;
            int length = args.Length;
            var ps = new BuilderParameterInfo[length];
            object value;
            for (int i = 0; i < length; i++)
            {
                value = args[i];
                if (value == null)
                    ps[i] = NullValue;
                else
                    ps[i] =   BuilderParameterInfo.MakeValue(value.GetType(), null, value);
            }
            return ps;
        }

        
        public static BuilderParameterInfo[] FromTypes(params Type[] types)
        {
            if (types == null || types.Length == 0)
                return EmptyArray;
            int length = types.Length;
            var ps = new BuilderParameterInfo[length];
            Type type;
            for (int i = 0; i < length; i++)
            {
                type = types[i];
                if (type == null || type == Type.Missing)
                    ps[i] = NullValue;
                else
                    ps[i] = new BuilderParameterInfo(type);
            }
            return ps;
        }

        public static BuilderParameterInfo MakeDefault(Type parameterType, string name, object defaultValue)
        {
            return new BuilderParameterInfo(parameterType, name) { defaultValue = defaultValue, hasDefaultValue = true };
        }
        public static BuilderParameterInfo MakeValue(Type parameterType, string name, object value)
        {
            return new BuilderParameterInfo(parameterType, name) { value = value, hasValue = true };
        }

        public override string ToString()
        {
            return string.Format("parameter type:<{0}>, name:<{1}>", (parameterType == null ? "" : parameterType.FullName), name);
        }

    }

}
