using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;
using LWJ.Injection.Aop;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{
    [TestClass]
    public partial class ProxyTest
    {
       
        [TestMethod]
        public void Aop_Transparent_Proxy()
        {
            IInjector injector = Injector.Create();

            injector.RegisterType<ISay, AopTransparentSayHello>();

            ISay obj;
       
            obj = injector.CreateInstance<ISay>();
            Assert.AreEqual("Hello", obj.Say());

        }
        [TestMethod]
        public void Aop_Transparent_Proxy_Named()
        {
            IInjector injector = Injector.Create();

            injector.RegisterType<ISay, AopTransparentSayHello>("hello");
            injector.RegisterType<ISay, AopTransparentSayWorld>("world");


            ISay obj;

            obj = injector.CreateInstance<ISay>("hello");
            Assert.AreEqual("Hello", obj.Say());

            obj = injector.CreateInstance<ISay>("world");
            Assert.AreEqual("World", obj.Say());
        }

        [TestMethod]
        public void Aop_Transparent_Proxy_Auto()
        {
            IInjector injector = Injector.Create();

            injector.RegisterType<ISay, AopTransparentSayHello>("hello");
          
            ISay obj; 
            obj = injector.CreateInstance<ISay>("hello");
            Assert.AreEqual("Hello", obj.Say());
   

        }
        interface ISay
        {
            string Say();
        }

        [AopServer(typeof(AopTransparentProxySay))]
        class AopTransparentSayHello : ISay
        {
            public string Say()
            {
                return "Hello";
            }
        }

        [AopServer(typeof(AopTransparentProxySay))]
        class AopTransparentSayWorld : ISay
        {
            public string Say()
            {
                return "World";
            }
        }

        //[Aop]
        //class AttributeAutoProxySayHello : ISay
        //{
        //    public string Say()
        //    {
        //        return "Hello";
        //    }
        //}


    


        [TransparentProxy]
        class AopTransparentProxySay : ContextBoundObject, ISay
        { 
            public string Say() { throw new NotImplementedException(); }

        }

    }
}
