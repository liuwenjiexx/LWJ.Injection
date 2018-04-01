/**************************************************************
 *  Filename:    IBuilderValue.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
 
namespace LWJ.ObjectBuilder
{

    public interface IBuilderValue
    {
        bool IsMatchValue(Type type, string name);

        object GetValue(Type type, string name);


    }

}
