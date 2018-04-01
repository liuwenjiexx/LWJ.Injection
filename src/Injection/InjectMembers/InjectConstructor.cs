/**************************************************************
 *  Filename:    InjectConstructor.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LWJ.Injection
{


    public class InjectConstructor : IInjectMember
    {
        private BuilderParameterInfo[] injectParams;
        public static readonly InjectConstructor Empty = new InjectConstructor(Type.EmptyTypes);

        public InjectConstructor(params object[] args)
            : this(BuilderParameterInfo.FromArgs(args))
        {
        }

        public InjectConstructor(params Type[] argTypes)
            : this(BuilderParameterInfo.FromTypes(argTypes))
        {
        }

        public InjectConstructor(params BuilderParameterInfo[] parameters)
        {
            this.injectParams = parameters;
        }



        public void AddBuilderMember(Type targetType, List<IBuilderMember> members)
        {
            members.Add(GetConstructorBuilder(targetType));
        }

        internal ConstructorBuilder GetConstructorBuilder(Type targetType)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;
            var argTypes = injectParams.Select(o => o.ParameterType).ToArray();
            var constructor = InjectorUtils.FindConstructor(targetType, argTypes, bindingFlags);
            if (constructor == null)
                throw new InjectionException(string.Format("type <{0}> not found  constructor, argument count:<{1}> ", targetType, argTypes.Length));
            return new ConstructorBuilder(constructor, injectParams);
        }

    }




}
