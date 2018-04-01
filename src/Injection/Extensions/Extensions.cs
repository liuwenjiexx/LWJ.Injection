/**************************************************************
 *  Filename:    Extensions.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.Injection.Aop;
using LWJ.ObjectBuilder;
using System;

namespace LWJ.Injection
{
    public static partial class Extensions
    {




        public static bool TryGetValue<T>(this ICallInvocation source, string name, out T value)
        {
            object o;
            if (source.TryGetValue(typeof(T), name, out o))
            {
                value = (T)o;
                return true;
            }
            value = default(T);
            return false;
        }


        public static ICallPolicy AddBehaviour(this ICallPolicy source, Type behaviourType, InjectConstructor constructor = null)
        {
            return source.AddBehaviour(behaviourType, constructor, null);
        }

        public static ICallPolicy AddBehaviour<TAopBehaviour>(this ICallPolicy source, InjectConstructor constructor = null, params IBuilderValue[] values)
            where TAopBehaviour : IAopBehaviour
        {
            return source.AddBehaviour(typeof(TAopBehaviour), constructor, values);
        }

        public static ICallPolicy AddMachRule(this ICallPolicy source, Type matchRuleType, InjectConstructor constructor = null)
        {
            return source.AddMachRule(matchRuleType, constructor, null);
        }

        public static ICallPolicy AddMachRule<TMatchRule>(this ICallPolicy source, InjectConstructor constructor = null, params IBuilderValue[] values)
            where TMatchRule : ICallMatchRule
        {
            return source.AddMachRule(typeof(TMatchRule), constructor, values);
        }

    }
}
