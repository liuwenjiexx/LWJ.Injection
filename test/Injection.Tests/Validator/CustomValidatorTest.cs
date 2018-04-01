using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using LWJ.Injection.Aop;
using LWJ.Injection.Aop.ParameterValidator; 
using System.Reflection;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{

    public partial class ValidatorTest
    {

        [TestMethod]
        public void Custom_Validator()
        {
            using (IInjector injector = Injector.Create())
            {
                injector.RegisterType<ICustomTest, CustomTest>();

                var obj = injector.CreateInstance<ICustomTest>();
                obj.SetString("hello world");
                try
                {
                    obj.SetString("world");
                    Assert.Fail();
                }
                catch (FailedParameterException ex)
                {
                    Console.WriteLine(ex.Message);
                } 
            }
        }


        interface ICustomTest
        {
            void SetString(string text);
        }

        [AopServer(typeof(ProxyCustom))]
        class CustomTest : ICustomTest
        {
            public void SetString([CustomValidate] string text)
            {
            }
        }

        class ProxyCustom : ICustomTest
        {
            [Inject]
            private ProxyServer proxy;

            public void SetString(string text)
            {
                proxy.Invoke("SetString", text);
            }
        }


        class CustomValidateAttribute : ParameterValidatorAttribute
        {
            public CustomValidateAttribute()
            {

            }

            public override IParameterValidator CreateValidator(ParameterInfo parameter)
            {
                return new StartsWithHelloValidator();
            }

            class StartsWithHelloValidator : IParameterValidator
            {

                public StartsWithHelloValidator()
                {

                }
                public FailedParameterException GetException(ParameterInfo parameterInfo, object value)
                {
                    return new FailedParameterException("starts with hello", parameterInfo, value);
                }

                public bool Validate(object value)
                {
                    string str = value as string;
                    if (str != null && str.StartsWith("hello"))
                        return true;
                    return false;
                }
            }

        }



    }
}
