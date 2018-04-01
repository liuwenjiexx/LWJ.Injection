//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using LWJ.Object.Proxy;

//namespace LWJ.Injection.Test
//{
//    [TestClass]
//    public  class ProxyTest2
//    {

//        [TestMethod]
//        public void Custom_Proxy()
//        {
//            var obj = new TestClass();
//            Assert.AreEqual("Hello World", obj.Hello());
//        }



//        interface ITest
//        {

//            string Hello();
//        }

//        [CustomProxy(typeof(ProxyTestClass))]
//        class TestClass : ITest
//        {
//            public string Hello()
//            {
//                return "Hello";
//            }
//        }

//        class MyProxyServer : ProxyServer
//        {
//            public override object Invoke(string methodName, params object[] args)
//            {
//                object result = base.Invoke(methodName, args);
//                if (methodName == "Hello")
//                    return result + " World";
//                return result;
//            }
//        }
//        class ProxyTestClass : ITest
//        {
//            ProxyServer proxy;
//            public ProxyTestClass(ProxyServer proxy)
//            {
//                this.proxy = proxy;
//            }
//            public string Hello()
//            {
//                return proxy.Invoke<string>("Hello");
//            }
//        }
//    }
//}
