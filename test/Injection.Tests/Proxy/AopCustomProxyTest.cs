using LWJ.Injection.Aop;
using LWJ.Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LWJ.Injection.Test
{

    public partial class ProxyTest
    { 
        [TestMethod]
        public void Aop_Custom_Proxy()
        {
            using (IInjector injector = Injector.Create())
            {
                injector.RegisterType<ISay, AopCustomSayHello>();

                var o = injector.CreateInstance<ISay>();
                Assert.IsInstanceOfType(o, typeof(AopCustomProxySay));
                Assert.AreEqual("Hello", o.Say());
            }

        }


        [TestMethod]
        public void Aop_Custom_Proxy_Named()
        {

            using (IInjector injector = Injector.Create())
            {

                injector.RegisterType<ISay, AopCustomSayHello>("hello");
                injector.RegisterType<ISay, AopCustomSayWorld>("world");


                var o = injector.CreateInstance<ISay>("hello");
                Assert.IsInstanceOfType(o, typeof(AopCustomProxySay));
                Assert.AreEqual("Hello", o.Say());

                o = injector.CreateInstance<ISay>("world");
                Assert.IsInstanceOfType(o, typeof(AopCustomProxySay));
                Assert.AreEqual("World", o.Say());
            }
        }

         



          
        [AopServer(typeof(AopCustomProxySay))]
        class AopCustomSayHello : ISay
        {
            public string Say()
            {
                return "Hello";
            }
        }
        [AopServer(typeof(AopCustomProxySay))]
        class AopCustomSayWorld : ISay
        {
            public string Say()
            {
                return "World";
            }
        }

        class AopCustomProxySay : ISay
        {
            [Inject]
            private ProxyServer proxy;
             
            public string Say()
            {
                return proxy.Invoke<string>("Say");
            }
        }



    }



}
