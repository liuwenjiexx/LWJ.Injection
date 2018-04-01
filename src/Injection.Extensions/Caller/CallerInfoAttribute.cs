/**************************************************************
 *  Filename:    CallerInfoAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System; 

namespace LWJ.Injection.Aop.Caller
{

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class CallerInfoAttribute : Attribute
    {
    }

}
