/**************************************************************
 *  Filename:    IAopBehaviour.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.Injection.Aop
{
    public interface IAopBehaviour
    {
    //    void OnAopInit();

        /// <summary>
        /// handler implement any interfaces: IBeforeCall, ICallHandler, IAfterCallReturn, IAfterCall, IThrowsCall
        /// </summary>
        /// <param name="methodBase"></param>
        /// <param name="methodHandlers"></param>
        void AddAopHandler(MethodBase methodBase, IList methodHandlers);
    }

}
