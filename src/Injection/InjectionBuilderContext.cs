/**************************************************************
 *  Filename:    InjectionBuilderContext.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Builder
{

    internal class InjectionBuilderContext : IBuilderContext
    {
        private IInjector injector;
        private Type interfaceType;
        private string interfaceName;
        private IList<IBuilderValue> values;

        internal InjectionBuilderContext(IInjector injector, Type interfaceType, string interfaceName, IBuilderValue[] values)
        {
            this.injector = injector;
            this.interfaceType = interfaceType;
            this.interfaceName = interfaceName;

            if (values != null)
                this.values = new List<IBuilderValue>(values);
        }

         
        public Type TargetType
        {
            get { return interfaceType; }
        }


        public string TargetName
        {
            get { return interfaceName; }
        }


        public IList<IBuilderValue> Values
        {
            get
            {
                if (values == null)
                    values = new List<IBuilderValue>();
                return values;
            }
        }
        private IEnumerable<IBuilderValue> CombinValues(params IEnumerable<IBuilderValue>[] values)
        {
            if (values == null || values.Length == 0)
                yield break;

            foreach (var item in values)
            {
                if (item != null)
                {
                    foreach (var item2 in item)
                        yield return item2;
                }
            }
        }

        public object CreateInstance(Type targetType, string name, IBuilderValue[] values)
        {
            return injector.CreateInstance(targetType, name, CombinValues(values, this.values).ToArray());
        }

        public object[] GetValues(BuilderParameterInfo[] parameterInfos, IBuilderValue[] values)
        {
            return injector.GetValue(parameterInfos, CombinValues(values, this.values));
        }

        public object GetValue(BuilderParameterInfo parameterInfo, IBuilderValue[] values)
        {
            return injector.GetValue(parameterInfo, CombinValues(values, this.values));
        }

    }
}
