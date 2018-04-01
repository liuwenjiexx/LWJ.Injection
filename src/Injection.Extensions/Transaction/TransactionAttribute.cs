/**************************************************************
 * Filename:    TransactionAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/


using LWJ.Injection.Aop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.Injection.Aop.Transaction
{
    
    //transactional
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class TransactionAttribute : CallHandlerAttribute
    {

        public TransactionAttribute(string providerName, TransactionUsage usage)
        {
            this.ProviderName = providerName;
            this.Usage = usage;
        }

        public string ProviderName { get; set; }

        public TransactionUsage Usage { get; set; }


        /*
        public override ICallHandler CreateCallHander()
        {
            throw new NotImplementedException();
        }*/
    }

    public enum TransactionUsage
    {
        Required,//用该属性标注的方法或组件总是在事务中运行。如果客户端已经在事务中，则在原事务中运行；如果没有事务，则开启一个新事务，在其中运行。
        New,//方法或组件总是在新开启的事务中运行。如果客户端已经在事务中，则首先将原事务挂起，然后新开启一个事务，在其中运行，新事务结束之后，原来事务从挂起点继续执行；如果没有事务，则开启一个新事务，在其中运行。
        Supports,//和 Required 属性的不同点是，在没有事务的环境中不会开启一个新事务；如果存在事务的话则加入其中运行，这点和 Reuqired 相同。
        NotSupported,//如果事务已经存在的情况下，则原来的事务要挂起，然后调用标注该属性的方法或组件，调用结束之后，继续原来的事务；无事务环境中调用的时候，不开启新事务，这点和 Supports 相同。
        Mandatory,//调用标注该属性的方法或组件的客户端，必须已经在事务中，如果不在事务中则会抛出异常；如果已经在事务中，则加入原来事务运行。和 Required 不同的是，该属性不会自动开启新的事务；
        Never,//用 Never 属性标注的方法或组件，不能在事务中运行。如果调用该方法或组件的客户端已经在事务中，则抛出异常。
    }


    public abstract class TransactionManmager
    {
        public abstract void BeginTransaction();

        public abstract void Commit();

        public abstract void Rollback();

    }

    public class TransactionCallBehaviour : IBeforeCall, IAfterCall, IThrowsCall
    {
        TransactionManmager trans;

        public static void Register(IInjector injector)
        {
            //injector.AddCallPolicy(null)
            //.AddMachRule<MethodAttributeMatchRule>(new InjectConstructor(typeof(TransactionAttribute), true))
            /*  .AddBeforeCall<TransactionCallBehaviour>()
              .AddAfterCall<TransactionCallBehaviour>()
              .AddThrowsCall<TransactionCallBehaviour>();*/
            //.AddBehaviour<TransactionCallBehaviour>();
        }

        public void BeforeInvoke(ICallInvocation invocation)
        {
            throw new NotImplementedException();
        }
        public void AfterInvoke(ICallInvocation invocation)
        {
            throw new NotImplementedException();
        }


        public void ThrowsInvoke(ICallInvocation invocation, Exception exception)
        {
            throw new NotImplementedException();
        }

        private class TransInfo
        {
            public TransactionUsage Usage;

            private static Dictionary<MethodBase, TransInfo> cached = new Dictionary<MethodBase, TransInfo>();

            public static TransInfo Get(MethodBase methodBase)
            {
                return cached.GetOrAdd(methodBase, (method) =>
                 {
                     TransInfo info;

                     var transAttr = method.GetCustomAttribute<TransactionAttribute>(true);
                     if (transAttr != null)
                     {
                         info = new TransInfo();
                         info.Usage = transAttr.Usage;
                     }
                     else
                     {
                         info = null;
                     }

                     return info;
                 });


            }




        }




    }

}
