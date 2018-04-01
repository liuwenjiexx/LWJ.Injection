using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LWJ.Injection.Test
{
    [TestClass]
    public partial class BuildMemberTest
    {


        [TestMethod]
        public void Build_Property_String()
        {
            using (var injector = Injector.Create())
            {

                injector.RegisterType<IPropertyClass, PropertyClass>(new IInjectMember[] {
                new InjectProperty("StringProperty", "a") });

                var o = injector.Resolve<IPropertyClass>();

                Assert.IsNotNull(o);
                Assert.AreEqual("a", o.StringProperty);
            }
        }




        interface IPropertyClass
        {
            string StringProperty { get; set; }
        }

        class PropertyClass : IPropertyClass
        {
            public string StringProperty { get; set; }

        }

    }
}
