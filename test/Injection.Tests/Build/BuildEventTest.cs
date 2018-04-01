using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Test
{
    public partial class BuildMemberTest
    {
        [TestMethod]
        public void Build_Event()
        {
            using (var injector = Injector.Create())
            {
                string text = null;
                EventHandler handler = (sender, e) =>
                {
                    text = "a";
                };
                injector.RegisterType<IEventClass, EventClass>(new IInjectMember[] {
                new InjectEvent("Event1", handler) });

                var o = injector.Resolve<IEventClass>();

                Assert.IsNotNull(o);
                Assert.IsNull(text);
                o.OnEvent1();
                Assert.AreEqual("a", text);

                o.Event1 -= handler;
                text = null;
                o.OnEvent1();
                Assert.IsNull(text);
            }
        }


     

        interface IEventClass
        {
            event EventHandler Event1;
            void OnEvent1();
        }

        class StringEventArgs : EventArgs
        {
            public string Text { get; set; }
        }
        delegate void StringEventHandler(object sender, StringEventArgs e);

        class EventClass : IEventClass
        {
            public event EventHandler Event1;
            public event StringEventHandler StringEvent;

            public void OnEvent1()
            {
                if (Event1 != null)
                {
                    Event1(this, EventArgs.Empty);
                }
            }

            public string OnStringEvent()
            {
                string text = "empty";
                if (StringEvent != null)
                {
                    StringEventArgs args = new StringEventArgs();
                    args.Text = text;
                    StringEvent(this, args);
                    text = args.Text;
                }
                return text;
            }
        }

    }
}
