//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using LWJ.Object.Proxy;

//namespace LWJ.Injection.Test
//{ 
//    public partial class ProxyTest
//    {

//        [TestMethod]
//        public void Transparent_Proxy()
//        {
//           var obj= new TestClass();
//            Assert.AreEqual("Hello World", obj.Hello());
//        }



//        interface ITest
//        {

//            string Hello();
//        }

//       // [CustomProxy(typeof(ProxyTestClass),typeof(MyProxyServer))]
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
//        [TransparentProxy]
//        class ProxyTestClass : ContextBoundObject, ITest
//        {
//            public string Hello()
//            {
//                throw new NotImplementedException();
//            }
//        }


//    }
//}
