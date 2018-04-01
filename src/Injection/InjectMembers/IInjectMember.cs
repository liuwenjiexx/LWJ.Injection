/**************************************************************
 *  Filename:    IInjectMember.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections.Generic;

namespace LWJ.Injection
{

    public interface IInjectMember
    {
        void AddBuilderMember(Type targetType, List<IBuilderMember> members);

    }

}
