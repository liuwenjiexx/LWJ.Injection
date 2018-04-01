using LWJ.Injection.Aop;
using LWJ.Injection.Aop.Caller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Test.Caller
{
    [TestClass]
    public class CallerTest
    {

        [TestMethod]
        public void MethodInvoke()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ICallerClass, CallerClass>();
                var obj = injector.CreateInstance<ICallerClass>();
                var o = obj.MethodCall("hello");

                Assert.AreEqual("MethodInvoke", o.memberName);
            }
        }

        object Property
        {
            get
            {

                using (var injector = Injector.Create())
                {
                    injector.RegisterType<ICallerClass, CallerClass>();
                    var obj = injector.CreateInstance<ICallerClass>();
                    var o = obj.MethodCall("hello");

                    Assert.AreEqual("Property", o.memberName);
                    return o;
                }
            }
        }
        [TestMethod]
        public void Property_Invoke()
        {
            var s = Property;
        }

        class CallerInfo
        {
            public string message;
            public string memberName = "";
            public string sourceFilePath = "";
            public int sourceLineNumber = 0;
            public CallerInfo(string memberName, string sourceFilePath, int sourceLineNumber)
            {
                this.memberName = memberName;
                this.sourceFilePath = sourceFilePath;
                this.sourceLineNumber = sourceLineNumber;
            }
        }

        interface ICallerClass
        {

            CallerInfo MethodCall(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);


        }

        [AopServer(typeof(ProxyCallerClass))]
        class CallerClass : ICallerClass
        {


            public CallerInfo MethodCall(string message,
                 [CallerMemberName] string memberName = "",
                 [CallerFilePath] string sourceFilePath = "",
                 [CallerLineNumber] int sourceLineNumber = 0)
            {
                StringBuilder trace = new StringBuilder();

                trace.AppendLine("member name: " + memberName);
                trace.AppendLine("source file path: " + sourceFilePath);
                trace.AppendLine("source line number: " + sourceLineNumber);
                trace.Append("message: " + message);
                Console.WriteLine(trace.ToString());
                return new CallerInfo(memberName, sourceFilePath, sourceLineNumber);

            }



        }

        [LWJ.Proxies.TransparentProxy]
        class ProxyCallerClass : ContextBoundObject, ICallerClass
        {
            public CallerInfo MethodCall(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
            {
                throw new NotImplementedException();
            }
        }

    }

}
