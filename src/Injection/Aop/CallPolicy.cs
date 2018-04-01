/**************************************************************
 *  Filename:    AopPolicy.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.Injection.Aop;
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LWJ.Injection
{
    internal class AopPolicy : ICallPolicy
    {
        private string name;
        private List<BuildInfo> machRules;
        private List<BuildInfo> behaviourDatas;
        private IInjector injector;

        public AopPolicy(IInjector injector, string name)
        {
            if (injector == null)
                throw new ArgumentNullException(nameof(injector));
            this.name = name;
            behaviourDatas = new List<BuildInfo>();
            machRules = new List<BuildInfo>();
            this.injector = injector;
        }

        public string Name
        {
            get { return name; }
        }


        public ICallPolicy AddBehaviour(Type behaviourType, InjectConstructor constructor, IBuilderValue[] values)
        {
            if (behaviourType == null)
                throw new ArgumentNullException(nameof(behaviourType));

            if (!typeof(IAopBehaviour).IsAssignableFrom(behaviourType))
                throw new ArgumentException("<{0}> not implement <{1}>, type:<{2}>".FormatArgs(nameof(behaviourType), nameof(IAopBehaviour), behaviourType.FullName), nameof(behaviourType));

            var data = new BuildInfo(behaviourType, constructor, values);
            behaviourDatas.Add(data);
            return this;
        }


        public ICallPolicy AddMachRule(Type machRuleType, InjectConstructor constructor, IBuilderValue[] values)
        {
            if (machRuleType == null)
                return this;
            if (!typeof(ICallMatchRule).IsAssignableFrom(machRuleType))
                throw new ArgumentException("machRuleType not AssignableFrom:" + typeof(ICallPolicy));

            BuildInfo rule = new BuildInfo(machRuleType, constructor, values);
            machRules.Add(rule);
            return this;
        }


        public IEnumerable<IAopBehaviour> AllBehaviours()
        {
            if (behaviourDatas != null)
            {
                IAopBehaviour behaviour;
                var injector = this.injector;
                foreach (var it in behaviourDatas)
                {
                    behaviour = (IAopBehaviour)it.GetOrCacheTarget(injector);
                    yield return behaviour;
                }
            }
        }
        public bool IsMach(MethodBase method)
        {
            if (machRules.Count == 0)
                return true;
            var injector = this.injector;
            ICallMatchRule rule;
            foreach (var item in machRules)
            {
                rule = (ICallMatchRule)item.GetOrCacheTarget(injector);

                if (rule.IsCallMatch(method))
                    return true;
            }

            return false;
        }




        class BuildInfo
        {
            private Type buildType;
            private InjectConstructor constructor;
            private IBuilderValue[] values;
            private object target;

            public BuildInfo(Type type, InjectConstructor constructor, IBuilderValue[] values)
            {
                this.buildType = type;
                this.constructor = constructor;
                this.values = values;
            }

            public Type BuildType { get => buildType; }
            public InjectConstructor Constructor { get => constructor; }
            public IBuilderValue[] Values { get => values; }


            public object GetOrCacheTarget(IInjector injector)
            {
                object target = this.target;
                if (target != null)
                    return target;

                if (constructor == null)
                {
                    target = injector.CreateInstance(this.buildType, null, values);
                }
                else
                {
                    var builder = constructor.GetConstructorBuilder(this.buildType);
                    var ctx = new Builder.InjectionBuilderContext(injector, buildType, null, values);
                    target = builder.Invoke(ctx, null, values);
                    injector.Inject(target, values);
                    if (target is IAfterBuilder)
                        ((IAfterBuilder)target).AfterBuilder();
                }

                this.target = target;
                return target;
            }


        }
    }

}