using LWJ.Injection.Aop;
using LWJ.Injection.Aop.Caching;
using LWJ.Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LWJ.Injection.Test
{

    [TestClass]
    public class CachingTest
    {

        /// <summary>
        /// 测试单个方法缓存
        /// </summary>
        [TestMethod]
        public void Get_Value()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ICachingClass, CachingClass>();
                var obj = injector.CreateInstance<ICachingClass>();
                object result;

                obj.Value = "one";
                result = obj.Get_Value();
                Assert.AreEqual("one", result);

                obj.Value = "two";
                result = obj.Get_Value();
                Assert.AreEqual("one", result);
            }
        }

        /// <summary>
        /// 测试一个数据的两个方法缓存
        /// </summary>
        [TestMethod]
        public void Get_Value2()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ICachingClass, CachingClass>();
                var obj = injector.CreateInstance<ICachingClass>();
                object result;

                obj.Value = "one";
                result = obj.Get_Value();
                Assert.AreEqual("one", result);
                result = obj.Get_Value2();
                Assert.AreEqual("one2", result);

                obj.Value = "two";
                result = obj.Get_Value();
                Assert.AreEqual("one", result);
                result = obj.Get_Value2();
                Assert.AreEqual("one2", result);
            }
        }

        /// <summary>
        /// 测试不同方法是否分别做了缓存
        /// </summary>
        [TestMethod]
        public void Get_Value_2()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ICachingClass, CachingClass>();
                var obj = injector.CreateInstance<ICachingClass>();
                object result;

                obj.Value = "one";
                result = obj.Get_Value();
                Assert.AreEqual("one", result);
                result = obj.Get_Value2();
                Assert.AreEqual("one2", result);

                obj.Value = "two";
                result = obj.Get_Value();
                Assert.AreEqual("one", result);
                result = obj.Get_Value2();
                Assert.AreEqual("one2", result);
            }
        }
        /// <summary>
        /// 测试引用类型生存期
        /// </summary>
        [TestMethod]
        public void TimeoutRefValue()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ICachingClass, CachingClass>();
                var obj = injector.CreateInstance<ICachingClass>();
                string value = obj.Lifetime_RefType_100();
                Thread.Sleep(10);
                Assert.AreEqual(obj.Lifetime_RefType_100(), value);
                Thread.Sleep(Timeout);
                Assert.AreNotEqual(obj.Lifetime_RefType_100(), value);
            }
        }
        /// <summary>
        /// 测试值类型生存期
        /// </summary>
        [TestMethod]
        public void TimeoutValue()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ICachingClass, CachingClass>();
                var obj = injector.CreateInstance<ICachingClass>();
                int value = obj.Lifetime_ValueType_100();
                Thread.Sleep(10);
                Assert.AreEqual(obj.Lifetime_ValueType_100(), value);
                Thread.Sleep(Timeout);
                Assert.AreNotEqual(obj.Lifetime_ValueType_100(), value);
            }
        }

        [TestMethod]
        public void Two_Instance()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<ICachingClass, CachingClass>();
                var obj1 = injector.CreateInstance<ICachingClass>();
                var obj2 = injector.CreateInstance<ICachingClass>();
                object result;

                obj1.Value = "one";
                result = obj1.Get_Value();
                Assert.AreEqual("one", result);

                obj2.Value = "abc";
                result = obj2.Get_Value();
                Assert.AreEqual("abc", result);

                obj1.Value = "two";
                result = obj1.Get_Value();
                Assert.AreEqual("one", result);

                obj2.Value = "edf";
                result = obj2.Get_Value();
                Assert.AreEqual("abc", result);

            }
        }


        const int Timeout = 100;

        public interface ICachingClass
        {
            string Value { get; set; }
            string Value2 { get; set; }

            object Get_Value();

            object Get_Value2();

            object Get_Value_2();

            string Lifetime_RefType_100();

            int Lifetime_ValueType_100();

        }

        [AopServer(typeof(ProxyCachingClass))]
        public class CachingClass : ICachingClass
        {
            private string value;
            private string value2;

            public string Value { get => value; set => this.value = value; }
            public string Value2 { get => value2; set => value2 = value; }


            [CachingCall]
            public object Get_Value()
            {
                return Value;
            }

            [CachingCall]
            public object Get_Value2()
            {
                return Value + "2";
            }


            [CachingCall]
            public object Get_Value_2()
            {
                return Value2;
            }


            ILifetime LifetimeTimeout_100(string result)
            {
                var lifetime = new TimeoutLifetime(Timeout);
                lifetime.SetValue(result);
                return lifetime;
            }


            ILifetime LifetimeTimeout_100(int result)
            {
                var lifetime = new TimeoutLifetime(Timeout);
                lifetime.SetValue(result);
                return lifetime;
            }


            [CachingCall]
            [CachingLifetime(Method = "LifetimeTimeout_100")]
            public string Lifetime_RefType_100()
            {
                return DateTime.Now.Millisecond.ToString();
            }

            [CachingCall]
            [CachingLifetime(Method = "LifetimeTimeout_100")]
            public int Lifetime_ValueType_100()
            {
                return DateTime.Now.Millisecond;
            }
        }


        [TransparentProxy]
        public class ProxyCachingClass : ContextBoundObject, ICachingClass
        {
            public string Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string Value2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public object Get_Value()
            {
                throw new NotImplementedException();
            }

            public object Get_Value2()
            {
                throw new NotImplementedException();
            }

            public object Get_Value_2()
            {
                throw new NotImplementedException();
            }

            public string Lifetime_RefType_100()
            {
                throw new NotImplementedException();
            }

            public int Lifetime_ValueType_100()
            {
                throw new NotImplementedException();
            }
        }

    }
}
