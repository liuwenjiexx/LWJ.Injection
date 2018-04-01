/**************************************************************
 *  Filename:    IBuilderMember.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
 
namespace LWJ.ObjectBuilder
{
    public interface IBuilderMember
    {
        object Invoke(IBuilderContext context, object target, IBuilderValue[] values);
    }

}
