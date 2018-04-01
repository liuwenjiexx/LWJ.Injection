using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using LWJ.Injection.Aop;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{

    public partial class AopMemberTest
    {

 
        [TestMethod]
        public void Aop_Property_Int()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IPropertyClass, PropertyClass>();
                var target = injector.CreateInstance<IPropertyClass>();

                Assert.AreEqual(0, target.IntProperty);
                target.IntProperty = 1;
                Assert.AreEqual(1, target.IntProperty);
            }
        }

        [TestMethod]
        public void Aop_Property_Float()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IPropertyClass, PropertyClass>();
                var target = injector.CreateInstance<IPropertyClass>();
                Assert.AreEqual(0f, target.FloatProperty);
                target.FloatProperty = 1.1f;
                Assert.AreEqual(1.1f, target.FloatProperty);
            }
        }

        [TestMethod]
        public void Aop_Property_String()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IPropertyClass, PropertyClass>();
                var target = injector.CreateInstance<IPropertyClass>();
                Assert.AreEqual(null, target.StringProperty);
                target.StringProperty = string.Empty;
                Assert.AreEqual(string.Empty, target.StringProperty);
                target.StringProperty = "abc";
                Assert.AreEqual("abc", target.StringProperty);
            }
        }

        [TestMethod]
        public void Aop_Property_Object()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IPropertyClass, PropertyClass>();
                var target = injector.CreateInstance<IPropertyClass>();
                Assert.AreEqual(null, target.ObjectProperty);
                target.ObjectProperty = 1;
                Assert.AreEqual(1, target.ObjectProperty);
                target.ObjectProperty = 1.1f;
                Assert.AreEqual(1.1f, target.ObjectProperty);
                target.ObjectProperty = "abc";
                Assert.AreEqual("abc", target.ObjectProperty);
                target.ObjectProperty = new object[] { 1, "abc" };

                Assert.IsInstanceOfType(target.ObjectProperty, typeof(ICollection));
                CollectionAssert.AreEqual(new object[] { 1, "abc" }, (ICollection)target.ObjectProperty);
            }
        }

        [TestMethod]
        public void Aop_Property_String__Only_Read()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IPropertyClass, PropertyClass>();
                var target = injector.CreateInstance<IPropertyClass>();
                Assert.AreEqual(null, target.OnlyReadStringProperty);

                target.SetOnlyReadStringProperty("abc");
                Assert.AreEqual("abc", target.OnlyReadStringProperty);
            }
        }


        [TestMethod]
        public void Aop_Property_String__Only_Write()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IPropertyClass, PropertyClass>();
                var target = injector.CreateInstance<IPropertyClass>();
                Assert.AreEqual(null, target.GetOnlyWriteStringProperty());

                target.OnlyWriteStringProperty = "abc";
                Assert.AreEqual("abc", target.GetOnlyWriteStringProperty());
            }
        }

        interface IPropertyClass
        {
            string StringProperty { get; set; }
            int IntProperty { get; set; }
            float FloatProperty { get; set; }
            object ObjectProperty { get; set; }
            string OnlyReadStringProperty { get; }
            string OnlyWriteStringProperty { set; }
            string GetOnlyWriteStringProperty();
            void SetOnlyReadStringProperty(string value);
        }

        [AopServer(typeof(ProxyPropertyClass))]
        class PropertyClass : IPropertyClass
        {

            public float FloatProperty { get; set; }
            public int IntProperty { get; set; }
            public string StringProperty { get; set; }
            public object ObjectProperty { get; set; }
            private string onlyReadStringProperty;
            private string onlyWriteStringProperty;

            public string OnlyReadStringProperty => onlyReadStringProperty;

            public string OnlyWriteStringProperty { set => onlyWriteStringProperty = value; }

            public void SetOnlyReadStringProperty(string value)
            {
                this.onlyReadStringProperty = value;
            }

            public string GetOnlyWriteStringProperty()
            {
                return onlyWriteStringProperty;
            }

        }

        [TransparentProxy]
        class ProxyPropertyClass : ContextBoundObject, IPropertyClass
        {
            public float FloatProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int IntProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public object ObjectProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public string OnlyReadStringProperty => throw new NotImplementedException();

            public string OnlyWriteStringProperty { set => throw new NotImplementedException(); }
            public string StringProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public string GetOnlyWriteStringProperty()
            {
                throw new NotImplementedException();
            }

            public void SetOnlyReadStringProperty(string value)
            {
                throw new NotImplementedException();
            }
        }

    }
}
