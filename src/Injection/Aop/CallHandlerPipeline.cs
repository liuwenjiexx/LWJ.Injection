/**************************************************************
 *  Filename:    CallHandlerPipeline.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Aop
{

    internal class CallHandlerPipeline
    {
        private ICallHandler[] handlers;

        public CallHandlerPipeline()
        {

        }

        public CallHandlerPipeline(IEnumerable<ICallHandler> handlers)
        {
            if (handlers != null)
                this.handlers = handlers.ToArray();
        }


        public ICallReturn Invoke(ICallInvocation invocation, NextHandlerDelegate target)
        {
            int count = handlers == null ? 0 : handlers.Length;
            if (count <= 0)
                return target(invocation, null);


            int handleIndex = 0;

            return handlers[0].Invoke(invocation, delegate ()
            {
                handleIndex++;
                if (handleIndex < count)
                    return handlers[handleIndex].Invoke;
                return target;
            });
        }


    }
}
