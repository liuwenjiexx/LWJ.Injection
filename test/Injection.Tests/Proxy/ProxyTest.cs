using Microsoft.VisualStudio.TestTools.UnitTesting;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{
    /// <summary>
    /// ProxyTest 的摘要说明
    /// </summary>

    public partial class ProxyTest
    {
        [TestMethod]
        public void Register_Proxy_Inject_Proxy_Invoker()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ISay, SayHello>();
                injector.RegisterProxy<ISay, InjectProxyInvokerSayWorld>();

                ISay o = injector.CreateInstance<ISay>();

                Assert.IsInstanceOfType(o, typeof(InjectProxyInvokerSayWorld));
                Assert.AreEqual("Hello World", o.Say());
            }
        }

        [TestMethod]
        public void Register_Proxy_Inject_Target()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ISay, SayHello>();
                injector.RegisterProxy<ISay, InjectTargetSayWorld>();

                ISay o = injector.CreateInstance<ISay>();

                Assert.IsInstanceOfType(o, typeof(InjectTargetSayWorld));
                Assert.AreEqual("Hello World", o.Say());
            }
        }

        [TestMethod]
        public void Attribute_Proxy_Inject_Proxy_Invoker()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ISay, CustomProxyAttributeSayHello>();

                ISay o = injector.CreateInstance<ISay>();

                Assert.IsInstanceOfType(o, typeof(InjectProxyInvokerSayWorld));
                Assert.AreEqual("Hello World", o.Say());
            }
        }


        class SayHello : ISay
        {
            public string Say()
            {
                return "Hello";
            }
        }

        [CustomProxy(typeof(InjectProxyInvokerSayWorld))]
        class CustomProxyAttributeSayHello : ISay
        {
            public string Say()
            {
                return "Hello";
            }
        }


        class InjectProxyInvokerSayWorld : ISay
        {
            [Inject]
            private ProxyServer proxy;

            public string Say()
            {
                return proxy.Invoke("Say") + " World";
            }
        }


        class InjectTargetSayWorld : ISay
        {
            [Inject]
            private ISay target;

            public string Say()
            {
                return target.Say() + " World";
            }
        }


        /*

        [TestMethod]
        public void Caller()
        {
            IInjector injector = Injector.Create();

            injector.AddBehaviour(new ProxyBuilderStrategry());
            //injector.AddPolicy(null).AddMachRule<>

            injector.RegisterType<ILogger, TraceLog>(LogNames.Trace);

            var log = injector.CreateInstance<ILogger>(LogNames.Trace);
            Assert.IsInstanceOfType(log, typeof(TraceLog));
            var o = log.Log("gggg");

            StringAssert.StartsWith(o, "member name: Proxy_Trace_Message");
            StringAssert.EndsWith(o, "message: " + "gggg");
        }


        [TestMethod]
        public void Proxy_Caching_Call()
        {
            IInjector injector = Injector.Create();
            injector.AddBehaviour(new ProxyBuilderStrategry());

            CachingCallBehaviour.Register(injector);

            injector.RegisterType<ILogger, TimeLog>(LogNames.Time);


            var log = injector.CreateInstance<ILogger>(LogNames.Time);

            Assert.AreNotEqual(log.Log("1"), log.Log("2"));

            Assert.AreEqual(log.Log("1"), log.Log("1"));


        }*/

    }
}
