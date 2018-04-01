/**************************************************************
 *  Filename:    ICallMatchRule.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System.Reflection;

namespace LWJ.Injection.Aop
{
    /// <summary>
    /// IMethodMatchRule
    /// </summary>
    public interface ICallMatchRule
    {
        bool IsCallMatch(MethodBase method);
    }
}
