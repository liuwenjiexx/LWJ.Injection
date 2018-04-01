using LWJ.Injection.Aop;
using LWJ.ObjectBuilder;
using LWJ.Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Test
{
    [TestClass]
    public class InjectValueTest
    {



        [TestMethod]
        public void Register_Type()
        {
            var injector = Injector.Create();
            injector.RegisterType<ISay, SayA>();

            Assert.IsTrue(injector.IsTypeRegistered<ISay>());
            Assert.IsFalse(injector.IsTypeRegistered<ISay>("A"));

            var o = injector.CreateInstance<ISay>();
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayA));
            Assert.AreEqual("A", o.Say());

            try
            {
                injector.CreateInstance<ISay>("A");
                Assert.Fail();
            }
            catch (AssertFailedException ex) { throw ex; }
            catch { }

        }

        [TestMethod]
        public void Register_Type_Name()
        {
            var injector = Injector.Create();
            injector.RegisterType<ISay, SayA>("A");

            Assert.IsFalse(injector.IsTypeRegistered<ISay>());
            Assert.IsTrue(injector.IsTypeRegistered<ISay>("A"));

            var o = injector.CreateInstance<ISay>("A");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayA));
            Assert.AreEqual("A", o.Say());

            try
            {
                injector.CreateInstance<ISay>();
                Assert.Fail();
            }
            catch (AssertFailedException ex) { throw ex; }
            catch { }
        }

        [TestMethod]
        public void Register_Type_Inject_Constructor()
        {

            var injector = Injector.Create();
            injector.RegisterType<ISay, CustomSay>(new InjectConstructor("Hello"));

            ISay o = injector.CreateInstance<ISay>();
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(CustomSay));
            Assert.AreEqual("Hello", o.Say());
        }

        [TestMethod]
        public void Register_Type_Inject_Field()
        {
            var injector = Injector.Create();
            injector.RegisterType<ISay, SayA>()
            .RegisterType<ISay, SayB>("B")
            .RegisterType<ISay, SayA>("C");


            var d = injector.CreateInstance<FieldInjectData>();
            Assert.IsNotNull(d);

            ISay o;

            o = d.fieldA;
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayA));
            Assert.AreEqual("A", o.Say());

            o = d.GetFieldB();
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayB));
            Assert.AreEqual("B", o.Say());

            o = d.GetFieldC();
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayA));
            Assert.AreEqual("A", o.Say());
        }

        [TestMethod]
        public void Register_Type_Inject_Property()
        {
            var injector = Injector.Create();
            injector.RegisterType<ISay, SayA>()
            .RegisterType<ISay, SayB>("B")
            .RegisterType<ISay, SayC>("C");
            //.AddBuilder(new ProxyBuilderPipeline()
            //   .Add<ISay, ProxySay>());

            var o = injector.CreateInstance<InjectPropertyTest>();
            Assert.IsNotNull(o);

            Assert.IsNotNull(o.PropertyA);
            Assert.AreEqual("A", o.PropertyA.Say());

            Assert.IsNotNull(o.GetPropertyB());
            Assert.AreEqual("B", o.GetPropertyB().Say());

            Assert.IsNotNull(o.GetPropertyC());
            Assert.AreEqual("C", o.GetPropertyC().Say());
        }

        [TestMethod]
        public void Register_Instance()
        {
            var injector = Injector.Create();

            injector.RegisterValue<ISay>(new SayA())
            .RegisterValue<ISay>("B", new SayB())
            .RegisterValue<ISay>("C", new CustomSay("Hello"));

            var d = injector.CreateInstance<FieldInjectData>();
            Assert.IsNotNull(d);

            ISay o;
            o = d.fieldA;
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayA));
            Assert.AreEqual("A", o.Say());

            o = d.GetFieldB();
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayB));
            Assert.AreEqual("B", o.Say());

            o = d.GetFieldC();
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(CustomSay));
            Assert.AreEqual("Hello", o.Say());

        }


        [TestMethod]
        public void Create_Instance_Inject_Value()
        {
            IBuilderValue[] values = new IBuilderValue[]
            {
                new TypedValue(typeof(ISay), null, new SayA()),
                new TypedValue(typeof(ISay), "B", new SayB()),
                new TypedValue(typeof(ISay), "C", new CustomSay("Hello"))
            };

            var d = Injector.Default.CreateInstance<FieldInjectData>(null, values);
            Assert.IsNotNull(d);

            ISay o;

            o = d.fieldA;
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayA));
            Assert.AreEqual("A", o.Say());

            o = d.GetFieldB();
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SayB));
            Assert.AreEqual("B", o.Say());

            o = d.GetFieldC();
            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(CustomSay));
            Assert.AreEqual("Hello", o.Say());

        }

        [TestMethod]
        public void Name_Value_Parent_Child()
        {

            var parent = Injector.Create();
            var child = parent.CreateChild();

            parent.RegisterValue<string>("parent");
            parent.RegisterValue<string>("1", "one");

            child.RegisterValue<string>("child");
            child.RegisterValue<string>("2", "two");

            Assert.AreEqual("parent", parent.Resolve<string>());
            Assert.AreEqual("one", parent.Resolve<string>("1"));

            try
            {
                parent.Resolve<string>("2");
                Assert.Fail();
            }
            catch (AssertFailedException ex) { throw ex; }
            catch { }

            Assert.AreEqual("child", child.Resolve<string>());
            Assert.AreEqual("one", child.Resolve<string>("1"));
            Assert.AreEqual("two", child.Resolve<string>("2"));

            //try
            //{
            //object o=    Injector.Default.Resolve<string>();           
            //    Assert.Fail();
            //}
            //catch (AssertFailedException ex) { throw ex; }
            //catch { }
            try
            {
                Injector.Default.Resolve<string>("1");
                Assert.Fail();
            }
            catch (AssertFailedException ex) { throw ex; }
            catch { }
            try
            {
                Injector.Default.Resolve<string>("2");
                Assert.Fail();
            }
            catch (AssertFailedException ex) { throw ex; }
            catch { }
        }

        [TestMethod]
        public void Find_By_Name()
        {
            IInjector a, b, aa;
            IInjector o;
            using (var root = Injector.Create())
            {
                a = root.CreateChild("a");
                b = root.CreateChild("b");
                aa = a.CreateChild("aa");


                o = Injector.FindByName("a");
                Assert.IsNotNull(o);
                Assert.AreEqual("a", o.Name);


                o = Injector.FindByName("b");
                Assert.IsNotNull(o);
                Assert.AreEqual("b", o.Name);


                o = Injector.FindByName("aa");
                Assert.IsNotNull(o);
                Assert.AreEqual("aa", o.Name);
            }
        }

        [TestMethod]
        public void Find_By_Name_Dispose()
        {
            IInjector a, b, aa;
            IInjector o;
            using (var root = Injector.Create())
            {
                a = root.CreateChild("a");
                b = root.CreateChild("b");
                aa = a.CreateChild("aa");


                a.Dispose();
                o = Injector.FindByName("a");
                Assert.IsNull(o, "a");

                o = Injector.FindByName("aa");
                Assert.IsNull(o, "aa");

                b.Dispose();
                o = Injector.FindByName("b");
                Assert.IsNull(o, "bb");

            }
        }
        [TestMethod]
        public void Find_By_Name_Using()
        {
            IInjector o;
            using (var root = Injector.Create())
            {

                o = Injector.FindByName("a");
                Assert.IsNull(o);

                o = Injector.FindByName("aa");
                Assert.IsNull(o);

                using (var a = root.CreateChild("a"))
                {
                    o = Injector.FindByName("a");
                    Assert.IsNotNull(o);
                    Assert.AreEqual("a", o.Name);

                    using (var aa = a.CreateChild("aa"))
                    { 
                        o = Injector.FindByName("a");
                        Assert.IsNotNull(o);
                        Assert.AreEqual("a", o.Name);

                        o = Injector.FindByName("aa");
                        Assert.IsNotNull(o);
                        Assert.AreEqual("aa", o.Name);
                    }

                    o = Injector.FindByName("a");
                    Assert.IsNotNull(o);
                    Assert.AreEqual("a", o.Name);

                    o = Injector.FindByName("aa");
                    Assert.IsNull(o);

                }
                o = Injector.FindByName("a");
                Assert.IsNull(o);

            }

        }

        class FieldInjectData
        {

            [Inject]
            public ISay fieldA;

            [Inject]
            [InjectValue("B")]
            private ISay fieldB;

            [Inject]
            [InjectValue("C")]
            protected ISay fieldC;

            public ISay GetFieldB() => fieldB;
            public ISay GetFieldC() => fieldC;
        }

        class InjectPropertyTest
        {
            [Inject]
            public ISay PropertyA { get; set; }

            [Inject]
            [InjectValue("B")]
            private ISay PropertyB { get; set; }

            [Inject]
            [InjectValue("C")]
            protected ISay PropertyC { get; set; }

            public ISay GetPropertyB() => PropertyB;
            public ISay GetPropertyC() => PropertyC;
        }



        interface ISay
        {
            string Say();
        }

        class SayA : ISay
        {
            public string Say() => "A";
        }
        class SayB : ISay
        {
            public string Say() => "B";

        }
        class SayC : ISay
        {
            public string Say() => "C";

        }
        class CustomSay : ISay
        {
            private string say;
            public CustomSay(string say)
            {
                this.say = say;
            }
            public string Say() => say;
        }



        class ProxySay : ISay
        {
            [Inject]
            private ProxyServer proxy;

            public string Say() => (string)proxy.Invoke("Say");

        }

    }
}
