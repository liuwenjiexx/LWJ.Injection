/**************************************************************
 *  Filename:    IBuilderContext.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.ObjectBuilder
{

    public interface IBuilderContext
    {
         

        Type TargetType { get; }


        string TargetName { get; }


        IList<IBuilderValue> Values { get; }

        object CreateInstance(Type targetType, string name, IBuilderValue[] values);
         
        object GetValue(BuilderParameterInfo parameterInfo, IBuilderValue[] values);

        object[] GetValues(BuilderParameterInfo[] parameterInfos, IBuilderValue[] values);
             
    }


}
