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
        public void Build_Method_String()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IMethodClass, MethodClass>(new IInjectMember[] {
                    new InjectMethod("SetString", "a") });

                IMethodClass o;
                o = injector.Resolve<IMethodClass>();

                Assert.IsNotNull(o);
                Assert.AreEqual("a", o.GetString());
            }
        }
        interface IMethodClass
        {
            string GetString();
            void SetString(string value);
        }

        class MethodClass : IMethodClass
        {
            private string string1;

            public string GetString()
            {
                return string1;
            }


            public void SetString(string value)
            {
                string1 = value;
            }

        }
    }
}
