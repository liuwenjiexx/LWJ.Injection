using LWJ.Injection.Aop;
using LWJ.Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Test
{

    /// <summary>
    /// 不能支持Field，仅支持Method作代理
    /// </summary>
    [TestClass]
    public partial class AopMemberTest
    {
       

        [TestMethod]
        public void Aop_Method()
        {
            using (IInjector injector = Injector.Create())
            {
                injector.RegisterType<IMethodClass, MethodClass>();

              var  target = injector.CreateInstance<IMethodClass>();

                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(ProxyMethodClass));
                Assert.AreEqual("Hello World", target.Method());
            }
        }





        interface IMethodClass
        {
            string Method();
            int AddMethod(int a, int b);

        }

        [AopServer(typeof(ProxyMethodClass))]
        class MethodClass : IMethodClass
        {
            public MethodClass()
            {
                IntProperty = 1;
            }

            public int IntProperty { get; set; }


            public int AddMethod(int a, int b)
            {
                return a + b;
            }

            public string Method()
            {
                return "Hello World";
            }

        }

        [TransparentProxy]
        class ProxyMethodClass : ContextBoundObject, IMethodClass
        {
            public int AddMethod(int a, int b)
            {
                throw new NotImplementedException();
            }

            public string Method()
            {
                throw new NotImplementedException();
            }
        }



    }
}
