/**************************************************************
 *  Filename:    IParameterCollection.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;
using System.Reflection;

namespace LWJ.Injection
{
    public interface IParameterCollection : IList
    {
        object this[string parameterName] { get; set; }

        string GetParameterName(int index);

        ParameterInfo GetParameterInfo(int index);

        ParameterInfo GetParameterInfo(string parameterName);

        bool ContainsParameter(string parameterName);

        object[] ToValueArray();
    }

}
