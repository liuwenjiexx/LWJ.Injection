/**************************************************************
 *  Filename:    DefaultCallHandler.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System.Reflection;

namespace LWJ.Injection.Aop
{

    internal class DefaultCallHandler : ICallHandler
    {

        private int order = -1;

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public ICallReturn Invoke(ICallInvocation invocation, GetNextCallHandlerDelegate getNext)
        {
            object result;
            MethodBase method = invocation.MethodBase;
            object[] args = invocation.Arguments.ToValueArray();
            try
            {
                result = method.Invoke(invocation.Target, args);
            }
            catch (TargetInvocationException ex)
            {
                return invocation.ReturnException(ex.InnerException);
            }


            return invocation.Return(result);
        }




    }


}
