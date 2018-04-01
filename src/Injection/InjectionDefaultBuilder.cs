/**************************************************************
 *  Filename:    InjectionDefaultBuilder.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/

using LWJ.ObjectBuilder;
using System;

namespace LWJ.Injection
{
    //DefualtBuilder
    internal class InjectionDefaultBuilder : IBuilder
    {

        private InjectTypeMetadata injectType;
        private ConstructorBuilder constructorBuilder;
        private IBuilderMember[] builderMembers;
         

        public InjectionDefaultBuilder( InjectTypeMetadata injectType, ConstructorBuilder constructorBuilder, IBuilderMember[] builderMembers)
        { 

            this.injectType = injectType ?? throw new ArgumentNullException(nameof(injectType));

            this.constructorBuilder = constructorBuilder ?? throw new ArgumentNullException(nameof(constructorBuilder));

            this.builderMembers = builderMembers;
        }

        public object Build(IBuilderContext context, GetNextBuilderDelegate next)
        {
            object target;
             
             

            var constructorBuilder = this.constructorBuilder;
            var buildMembers = this.builderMembers;

            target = constructorBuilder.Invoke(context, null, null);
             

            if (buildMembers != null && buildMembers.Length > 0)
            {
                for (int i = 0, len = buildMembers.Length; i < len; i++)
                {
                    var buildMember = buildMembers[i];
                    buildMember.Invoke(context, target, null);
                }
            }


            if (target is IAfterBuilder)
                ((IAfterBuilder)target).AfterBuilder();

            return target;
        }
    }
}
