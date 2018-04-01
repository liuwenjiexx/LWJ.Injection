/**************************************************************
 *  Filename:    RangeAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LWJ.Injection.Aop.ParameterValidator
{

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class RangeAttribute : ParameterValidatorAttribute
    {

        public RangeAttribute(int min, int max)
        {
            this.Min = min;
            this.Max = max;
            this.typeCode = TypeCode.Int32;
        }
        public RangeAttribute(long min, long max)
        {
            this.Min = min;
            this.Max = max;
            this.typeCode = TypeCode.Int64;
        }

        public RangeAttribute(float min, float max)
        {
            this.Min = min;
            this.Max = max;
            this.typeCode = TypeCode.Single;
        }

        public RangeAttribute(double min, double max)
        {
            this.Min = min;
            this.Max = max;
            this.typeCode = TypeCode.Double;
        }

        public object Min { get; set; }

        public object Max { get; set; }

        private TypeCode typeCode;


        public override IParameterValidator CreateValidator(ParameterInfo parameter)
        { 
            object min = this.Min;
            object max = this.Max;
            return CreateRangeValidator(typeCode, min, max);
        }

        static IParameterValidator CreateRangeValidator(TypeCode valueTypeCode, object min, object max)
        {
            switch (valueTypeCode)
            {
                case TypeCode.Int32:
                    return new Int32RangeValidator((int)min, (int)max);
                case TypeCode.Int64:
                    return new Int64RangeValidator((long)min, (long)max);
                case TypeCode.Single:
                    return new Float32RangeValidator((float)min, (float)max);
                case TypeCode.Double:
                    return new Float64RangeValidator((double)min, (double)max);
                    /*
                case TypeCode.Int32:
                    return new Int32RangeValidator(Convert.ToInt32(min), Convert.ToInt32(max));
                case TypeCode.Int64:
                    return new Int64RangeValidator(Convert.ToInt64(min), Convert.ToInt64(max));
                case TypeCode.Single:
                    return new Float32RangeValidator(Convert.ToSingle(min), Convert.ToSingle(max));
                case TypeCode.Double:
                    return new Float64RangeValidator(Convert.ToDouble(min), Convert.ToDouble(max));*/
            }

            throw new NotImplementedException();
        }

        const string MinParameterName = "min";
        const string MaxParameterName = "max";

        internal static IParameterValidator CreateRangeValidator(Type valueType, IDictionary<string, object> parameters)
        {
            object min;
            object max;

            min = Utils.GetParameter(parameters, MinParameterName, valueType);
            max = Utils.GetParameter(parameters, MaxParameterName, valueType);

            return RangeAttribute.CreateRangeValidator(Type.GetTypeCode(valueType), min, max);
        }

        private class Int32RangeValidator : IParameterValidator
        {
            private int min;
            private int max;

            public Int32RangeValidator(int min, int max)
            {
                this.min = min;
                this.max = max;
            }

            public bool Validate(object value)
            {
                var val = Convert.ToInt32(value);
                return min <= val && val <= max;
            }
            public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
            {
                return new FailedRangeException(parameterInfo, value, min, max);
            }
        }


        private class Int64RangeValidator : IParameterValidator
        {
            private long min;
            private long max;

            public Int64RangeValidator(long min, long max)
            {
                this.min = min;
                this.max = max;
            }

            public bool Validate(object value)
            {
                var val = Convert.ToInt64(value);
                return min <= val && val <= max;
            }

            public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
            {
                return new FailedRangeException(parameterInfo, value, min, max);
            }
        }
        private class Float32RangeValidator : IParameterValidator
        {
            private float min;
            private float max;

            public Float32RangeValidator(float min, float max)
            {
                this.min = min;
                this.max = max;
            }

            public bool Validate(object value)
            {
                var val = Convert.ToSingle(value);
                return min <= val && val <= max;
            }
            public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
            {
                return new FailedRangeException(parameterInfo, value, min, max);
            }
        }
        private class Float64RangeValidator : IParameterValidator
        {
            private double min;
            private double max;

            public Float64RangeValidator(double min, double max)
            {
                this.min = min;
                this.max = max;
            }

            public bool Validate(object value)
            {
                var val = Convert.ToDouble(value);
                return min <= val && val <= max;
            }
            public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
            {
                return new FailedRangeException(parameterInfo, value, min, max);
            }
        }

    }



}
