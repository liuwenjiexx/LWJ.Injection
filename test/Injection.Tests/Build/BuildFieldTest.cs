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
        public void Build_Field_String()
        {
            using (var injector = Injector.Create())
            {

                injector.RegisterType<IFieldClass, BuildFieldSay>(new IInjectMember[] { new InjectField("stringField", "a") });

                var o = injector.Resolve<IFieldClass>() as BuildFieldSay;

                Assert.IsNotNull(o);
                Assert.AreEqual("a", o.GetStringField());
            }
        }

        //  [TestMethod]
        public void Register_Type_Build_Field_No_Value()
        {
            using (var injector = Injector.Create())
            {

                injector.RegisterType<IFieldClass, BuildFieldSay>(new IInjectMember[] {
                    new InjectField("stringField") });

                var o = injector.Resolve<IFieldClass>() as BuildFieldSay;

                Assert.IsNotNull(o);
                Assert.AreEqual("a", o.GetStringField());
            }
        }

        interface IFieldClass
        {
            string GetStringField();
        }

        class BuildFieldSay : IFieldClass
        {

            public string stringField;

            public string GetStringField()
            {
                return stringField;
            }
        }


    }
}
