/**************************************************************
 *  Filename:    ICallPolicy.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.Injection.Aop;
using LWJ.ObjectBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.Injection
{
    /// <summary>
    /// Behaviour Match Policy
    /// </summary> 
    public interface ICallPolicy
    {

        string Name { get; }

        /// <summary>
        /// 一个策略可以对应多个匹配规则，只需要其中一个规则匹配则策略匹配成功
        /// </summary>
        /// <param name="machType"></param>
        /// <param name="constructor"></param>
        /// <returns></returns>
        ICallPolicy AddMachRule(Type machType, InjectConstructor constructor, IBuilderValue[] values);


        bool IsMach(MethodBase method);

        ICallPolicy AddBehaviour(Type behaviourType, InjectConstructor constructor,  IBuilderValue[] values);


        //IEnumerable<Type> GetHandlerTypes(Type interfaceType);
        //IEnumerable<Type> AllHandlerTypes();
        IEnumerable<IAopBehaviour> AllBehaviours();

    }
}
