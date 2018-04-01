using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
using LWJ.Injection.Aop;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{
     
    public partial class AopMemberTest
    { 
 

        [TestMethod]
        public void Aop_Event()
        {
            using (IInjector injector = Injector.Create())
            {
                injector.RegisterType<IEventClass, EventClass>();
                 
                var target = injector.CreateInstance<IEventClass>();

                int n = 0;
                EventHandler handler = new EventHandler((o, e) =>
                {
                    n++;
                });
                Assert.AreEqual(0, n);

                target.Event1 += handler;
                target.OnEvent1();
                Assert.AreEqual(1, n);

                target.Event1 -= handler;
                target.OnEvent1();
                Assert.AreEqual(1, n);
            }

        }


        interface IEventClass
        {
            event EventHandler Event1;
            void OnEvent1();
        }

        [AopServer(typeof(ProxyEventClass))]
        class EventClass : IEventClass
        {
            public event EventHandler Event1;
            public void OnEvent1()
            {
                if (Event1 != null)
                    Event1(this, EventArgs.Empty);
            }
        }

        [TransparentProxy]
        class ProxyEventClass : ContextBoundObject, IEventClass
        {

            public event EventHandler Event1;

            public void OnEvent1()
            {
                throw new NotImplementedException();
            }
        }

    }


}
