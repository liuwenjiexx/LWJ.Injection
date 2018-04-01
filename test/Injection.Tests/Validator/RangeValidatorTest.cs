using LWJ.Injection.Aop;
using LWJ.Injection.Aop.ParameterValidator;
using LWJ.Proxies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Test
{
    public partial class ValidatorTest
    {
        [TestMethod]
        public void Range_Int32()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRangeClass, RangeClass>();
                
                var obj = injector.CreateInstance<IRangeClass>();

                obj.Int32Range1_100(1);
                obj.Int32Range1_100(2);
                obj.Int32Range1_100(50);
                obj.Int32Range1_100(9);
                obj.Int32Range1_100(100);
                try
                {
                    obj.Int32Range1_100(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }
                try
                {
                    obj.Int32Range1_100(101);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                try
                {
                    obj.Int32Range1_Max(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }
                obj.Int32Range1_Max(int.MaxValue);
            }
        }
        [TestMethod]
        public void Range_Int64()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRangeClass, RangeClass>();

                var obj = injector.CreateInstance<IRangeClass>();
                obj.Int64Range1_100(1);
                obj.Int64Range1_100(2);
                obj.Int64Range1_100(50);
                obj.Int64Range1_100(9);
                obj.Int64Range1_100(100);
                try
                {
                    obj.Int64Range1_100(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }
                try
                {
                    obj.Int64Range1_100(101);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }
                try
                {
                    obj.Int64Range1_Max(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }
                obj.Int64Range1_Max(long.MaxValue);
            }
        }
        [TestMethod]
        public void Range_Float32()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRangeClass, RangeClass>();

                var obj = injector.CreateInstance<IRangeClass>();
                obj.Float32Range1_100(1); 
                obj.Float32Range1_100(50); 
                obj.Float32Range1_100(100);
                try
                {
                    obj.Float32Range1_100(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                try
                {
                    obj.Float32Range1_100(101);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                try
                {
                    obj.Float32Range1_Max(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                obj.Float32Range1_Max(float.MaxValue);
            }
        }
        [TestMethod]
        public void Range_Float64()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRangeClass, RangeClass>();

                var obj = injector.CreateInstance<IRangeClass>();
                obj.Float64Range1_100(1);
                obj.Float64Range1_100(2);
                obj.Float64Range1_100(50);
                obj.Float64Range1_100(9);
                obj.Float64Range1_100(100);
                try
                {
                    obj.Float64Range1_100(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                try
                {
                    obj.Float64Range1_100(101);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                try
                {
                    obj.Float64Range1_Max(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                obj.Float64Range1_Max(double.MaxValue);
            }
        }
        interface IRangeClass
        {
            
            void Int32Range1_Max(int n);
            void Int64Range1_Max(long n);
            void Float32Range1_Max(float n);
            void Float64Range1_Max(double n);
            void Int32Range1_100(int n);
            void Int64Range1_100(long n);
            void Float32Range1_100(float n);
            void Float64Range1_100(double n);

        }

        [AopServer(typeof(ProxyRangeClass))]
        class RangeClass : IRangeClass
        {
   
            public void Int32Range1_Max([Range(1, int.MaxValue)] int n)
            {
                Assert.IsTrue(1 <= n && n <= int.MaxValue);
            }

            public void Int64Range1_Max([Range(1, long.MaxValue)] long n)
            {
                Assert.IsTrue(1 <= n && n <= long.MaxValue);
            }

            public void Float32Range1_Max([Range(1, float.MaxValue)] float n)
            {
                Assert.IsTrue(1f <= n && n <= float.MaxValue);
            }

            public void Float64Range1_Max([Range(1, double.MaxValue)] double n)
            {
                Assert.IsTrue(1d <= n && n <= double.MaxValue);
            }
            public void Int32Range1_100([Range(1, 100)] int n)
            {
                Assert.IsTrue(1 <= n && n <= 100);
            }

            public void Int64Range1_100([Range(1, 100l)] long n)
            {
                Assert.IsTrue(1 <= n && n <= 100);
            }

            public void Float32Range1_100([Range(1, 100f)] float n)
            {
                Assert.IsTrue(1f <= n && n <= 100);
            }

            public void Float64Range1_100([Range(1, 100d)] double n)
            {
                Assert.IsTrue(1d <= n && n <= 100);
            }

        }

        [TransparentProxy]
        class ProxyRangeClass : ContextBoundObject, IRangeClass
        {
            public void Float32Range1_100(float n)
            {
                throw new NotImplementedException();
            }

            public void Float32Range1_Max(float n)
            {
                throw new NotImplementedException();
            }

            public void Float64Range1_100(double n)
            {
                throw new NotImplementedException();
            }

            public void Float64Range1_Max(double n)
            {
                throw new NotImplementedException();
            }

            public void Int32Range1_100(int n)
            {
                throw new NotImplementedException();
            }

            public void Int32Range1_Max(int n)
            {
                throw new NotImplementedException();
            }

            public void Int64Range1_100(long n)
            {
                throw new NotImplementedException();
            }

            public void Int64Range1_Max(long n)
            {
                throw new NotImplementedException();
            }
        }

    }
}
