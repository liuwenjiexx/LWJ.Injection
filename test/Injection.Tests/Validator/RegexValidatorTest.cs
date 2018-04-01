using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using LWJ.Injection.Aop;
using LWJ.Injection.Aop.ParameterValidator;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{
     
    [TestClass]
    public partial class ValidatorTest
    {
         
        [TestMethod]
        public void Regex_String_Url()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRegexClass, RegexClass>();

                 var obj = injector.CreateInstance<IRegexClass>();
                  
                obj.Regex_Http_Url("http://a.com");
                obj.Regex_Http_Url("HTTP://a.com");
                try
                {
                    obj.Regex_Http_Url("H://a.com");
                    Assert.Fail();
                }
                catch (FailedParameterException ex) { }
                catch { throw; }
            }
        }/*
        [TestMethod]
        public void Regex_Url_No_Register_Type()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRegexClass, RegexClass>();

                var obj = injector.CreateInstance<IRegexClass>();

                obj.Regex_Http_Url("http://a.com");
                obj.Regex_Http_Url("HTTP://a.com");
                try
                {
                    obj.Regex_Http_Url("H://a.com");
                    Assert.Fail();
                }
                catch (FailedParameterValidationException ex) { }
                catch { throw; }
            }
        }*/

        [TestMethod]
        public void Regex_String_Url_Child()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRegexClass, RegexClass>();
                  
                var child = injector.CreateChild();
                var obj = child.Resolve<IRegexClass>();

                obj.Regex_Http_Url("http://a.com");
                obj.Regex_Http_Url("HTTP://a.com");
                try
                {
                    obj.Regex_Http_Url("H://a.com");
                    Assert.Fail();
                }
                catch (FailedParameterException ex) { }
                catch { throw; }
            }
        }

        [TestMethod]
        public void Regex_String_Url_Child_Child()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRegexClass, RegexClass>();
                 
                var child = injector.CreateChild().CreateChild();
                var obj = child.Resolve<IRegexClass>();

                obj.Regex_Http_Url("http://a.com");
                obj.Regex_Http_Url("HTTP://a.com");
                try
                {
                    obj.Regex_Http_Url("H://a.com");
                    Assert.Fail();
                }
                catch (FailedParameterException ex) { }
                catch { throw; }
            }
        }

        [TestMethod]
        public void Regex_Int()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRegexClass, RegexClass>();
                  
                var o = injector.CreateInstance<IRegexClass>();
                 
                Assert.AreEqual(100, o.ToInteger("100"));
                 
                try
                {
                    o.ToInteger("a");
                    Assert.Fail();
                } 
                catch (FailedRegexException ex) { }
                catch { throw; }

            }

        }

        interface IRegexClass
        {
            void Regex_Http_Url(string url);
            int ToInteger(string integer);
        }




        [AopServer(typeof(ProxyRegexClass))]
        class RegexClass : IRegexClass
        {
            private const string httpUrlRegex = "\\s*http://.*";

            public RegexClass(IInjector injector)
            {

            }

            public void Regex_Http_Url([Regex(httpUrlRegex)] string url)
            {
                Regex regex = new Regex(httpUrlRegex, RegexOptions.IgnoreCase);
                Assert.IsTrue(regex.IsMatch(url));
            }
            public int ToInteger([Regex("\\d+")] string integer)
            {
                return int.Parse(integer);
            }
        }

        [TransparentProxy]
        class ProxyRegexClass : ContextBoundObject, IRegexClass
        {
            public void Regex_Http_Url(string url)
            {
                throw new NotImplementedException();
            }

            public int ToInteger(string integer)
            {
                throw new NotImplementedException();
            }
        }


    }



}
