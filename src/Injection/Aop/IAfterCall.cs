﻿/**************************************************************
 *  Filename:    IAfterCall.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/


namespace LWJ.Injection.Aop
{
    public interface IAfterCall
    {
        void AfterInvoke(ICallInvocation invocation);
    }

}
