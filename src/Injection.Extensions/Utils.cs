using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection
{
    internal static class Utils
    {
        internal static T GetParameter<T>(IDictionary<string, object> parameters, string parameterName)
        {
            return (T)GetParameter(parameters, parameterName, typeof(T));
        }
        internal static T GetParameter<T>(IDictionary<string, object> parameters, string parameterName, T defaultValue)
        {
            return (T)GetParameter(parameters, parameterName, typeof(T), defaultValue);
        }
        internal static object GetParameter(IDictionary<string, object> parameters, string parameterName, Type parameterType)
        {
            object value;
            if (!parameters.TryGetValue(parameterName, out value))
                throw new KeyNotFoundException(Resource1.Aop_Parameter_NotFound.FormatArgs(parameterName));

            value = ParameterChangeType(parameterName, parameterType, value);
            return value;
        }

        internal static object GetParameter(IDictionary<string, object> parameters, string parameterName, Type parameterType, object defaultValue)
        {
            object value;
            if (!parameters.TryGetValue(parameterName, out value))
                return defaultValue;
            value = ParameterChangeType(parameterName, parameterType, value);
            return value;
        }
        static object ParameterChangeType(string parameterName, Type parameterType, object value)
        {

            Type valueType = null;
            if (value != null)
                valueType = value.GetType();
            if (valueType == parameterType)
                return value;
            if (parameterType == typeof(Type) && value is string)
            {
                value = Type.GetType((string)value, true);
                return value;
            }

            try
            {
                value = Convert.ChangeType(value, parameterType);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(Resource1.Aop_Parameter_InvalidCast.FormatArgs(parameterName, parameterType, value));
            }
            return value;
        }
    }
}
