#if DEBUG
/**************************************************************
 *  Filename:    PerformanceBehaviour.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace LWJ.Injection.Aop.Performance
{
    public class PerformanceBehaviour : IAopBehaviour
    {
        public PerformanceBehaviour()
        {

        }
        public PerformanceBehaviour(IDictionary<string, object> validatorParameters)
        {

        }
        public void AddAopHandler(MethodBase methodBase, IList handlers)
        {
            handlers.Add(new TimeBeforeCallHandler());
        }

      //  [HandlerScope(Scope = HandlerScope.Method)]
        class TimeBeforeCallHandler : IBeforeCall, IAfterCall
        {
            static readonly object key = new object();
            MethodInfo callbackMethod;
            bool isInit;
            static int i = 0;
            public TimeBeforeCallHandler()
            {
                i++;

            }


            public void BeforeInvoke(ICallInvocation invocation)
            {
                if (!isInit)
                {
                    isInit = true;
                    var m = invocation.MethodBase;
                    var type = m.DeclaringType;
                    var timeAttr = m.GetCustomAttribute<PerformanceTimeAttribute>(true);
                    if (!string.IsNullOrEmpty(timeAttr.Method))
                    {
                        var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                        var types = new Type[] { typeof(PerformanceTimeContext) };
                        callbackMethod = type.GetMethod(timeAttr.Method, types, bindingFlags);

                    }

                }

                PerformanceTimeContext ctx = new PerformanceTimeContext()
                {
                    Target = invocation.Target,
                    Method = invocation.MethodBase,
                    StartTime = DateTime.Now,
                };
                invocation.Data[key] = ctx;
            }
            public void AfterInvoke(ICallInvocation invocation)
            {
                var ctx = (PerformanceTimeContext)invocation.Data[key];
                ctx.EndTime = DateTime.Now;
                if (callbackMethod != null)
                {
                    callbackMethod.Invoke(callbackMethod.IsStatic ? null : invocation.Target, new object[] { ctx });
                }
            }

        }



    }
}
#endif