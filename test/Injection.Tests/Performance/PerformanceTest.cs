using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LWJ.Injection;
using System.Diagnostics;
using LWJ.Injection.Aop;
using LWJ.Injection.Aop.Performance;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{
    public class TestClassTarget<TInterface, TTarget>
        where TTarget : TInterface
    {
        private TInterface target;


        public TInterface Target { get => target; }
        IInjector injector;
        [TestInitialize]
        public virtual void OnInit()
        {
            OnCreate();
        }

        [TestCleanup]
        public void Unload()
        {
            if (injector != null)
            {
                injector.Dispose();
                injector = null;
            }
        }

        protected virtual void OnCreate()
        {
            injector = Injector.Create();
            Type type = typeof(TInterface);
            injector.RegisterType<TInterface, TTarget>();
            //PerformanceBehaviour.Register(injector);
            //target = (T)Activator.CreateInstance(typeof(T), injector);
            target = injector.CreateInstance<TInterface>();

        }
    }

    [TestClass]
    public class PerformanceTest : TestClassTarget<PerformanceTest.IPerformanceTimeClass, PerformanceTest.PerformanceTimeClass>
    {

        [TestMethod]
        public void Sleep100()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Target.Sleep(100);
            sw.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds > 100);
            Assert.IsTrue(context.Elapsed.TotalMilliseconds > 100);
        }

        [TestMethod]
        public void EmptyMethodRun1000()
        {
            for (int i = 0; i < 1000; i++)
            {
                Target.EmptyMethod();
            }
        }


        static PerformanceTimeContext context;

        public interface IPerformanceTimeClass
        {
            void Sleep(int ms);
            void EmptyMethod();
        }

        [AopServer(typeof(ProxyPerformanceTimeClass))]
        public class PerformanceTimeClass : IPerformanceTimeClass
        {

            [PerformanceTime(Method = "PerformanceLog")]
            public void Sleep(int ms)
            {
                Thread.Sleep(ms);
            }



            public void EmptyMethod()
            {

            }


            //[CompileHoldName]
            static void PerformanceLog(PerformanceTimeContext context)
            {
                PerformanceTest.context = context;
                Console.WriteLine(context.Method + "  " + context.Elapsed.TotalMilliseconds);
            }
        }


        [TransparentProxy]
        public class ProxyPerformanceTimeClass : ContextBoundObject, IPerformanceTimeClass
        {
            public void EmptyMethod()
            {
                throw new NotImplementedException();
            }

            public void Sleep(int ms)
            {
                throw new NotImplementedException();
            }
        }
    }
}
