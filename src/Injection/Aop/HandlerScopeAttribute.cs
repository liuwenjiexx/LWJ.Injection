/**************************************************************
 *  Filename:    HandlerScopeAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System; 

namespace LWJ.Injection.Aop
{
    internal class HandlerScopeAttribute : Attribute
    {

        public HandlerScope Scope { get; set; }



    }

    internal enum HandlerScope
    {
        Method = 0,
        Class,
        Assembly,
    }


}
