using LWJ.Injection.Aop;
using LWJ.Injection.Aop.ParameterValidator;
using LWJ.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LWJ.Injection.Test
{
    public partial class ValidatorTest
    {

        void Add(Type type, string methodName)
        {

        }
        class AEndPoint
        {
            public Type Type;
            public MethodInfo Method;

        }

        class MethodNameMatchRule : ICallMatchRule
        {
            private Type type;
            private string methodName;
            public MethodNameMatchRule(Type type, string methodName)
            {
                this.type = type ?? throw new ArgumentNullException(nameof(type));
                this.methodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            }
            public bool IsCallMatch(MethodBase method)
            {
                if (type != null && !type.IsAssignableFrom(method.DeclaringType))
                    return false;
                if (methodName != method.Name)
                    return false;
                return true;
            }
        }


        [TestMethod]
        public void Add_Behaviour_Range_Int32_1_100()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRangeClass, RangeManualClass>();


                //string uri = "LWJ.Injection.Test.ValidatorTest.IRangeClass/Int32Range1_100/n";
                //string s1 = Uri.EscapeUriString(uri);
                //string s2 = Uri.EscapeDataString(uri);
                //uri: type full name/method name/parameter name

                Type interfaceType = typeof(IRangeClass);

                string parameterName;
                ParameterValidatorType validatorType;
                parameterName = "n";
                validatorType = ParameterValidatorType.Range;
                Dictionary<string, object> validatorProperties = new Dictionary<string, object>()
                {
                    {"min",1 },
                    {"max",100 }
                };
                injector.AddCallPolicy()
                    .AddMachRule<MethodNameMatchRule>(new InjectConstructor(interfaceType, "Int32Range1_100"))
                    .AddBehaviour<ParameterValidatorBehaviour>(
                    null,
                    new TypedValue(ParameterValidatorBehaviour.ParameterNameProperty, parameterName),
                    new TypedValue(ParameterValidatorBehaviour.ValidatorTypeProperty, validatorType),
                    new TypedValue(ParameterValidatorBehaviour.ValidatorParametersProperty, validatorProperties));

                var obj = injector.CreateInstance<IRangeClass>();

                try
                {
                    obj.Int32Range1_100(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                obj.Int32Range1_100(1);
                obj.Int32Range1_100(50);
                obj.Int32Range1_100(100);

                try
                {
                    obj.Int32Range1_100(101);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

            }
        }

        [TestMethod]
        public void Add_Behaviour_Range_Int32_Multi()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterType<IRangeClass, RangeManualClass>();

                Type interfaceType = typeof(IRangeClass);
                string parameterName;
                ParameterValidatorType validatorType;
                parameterName = "n";
                validatorType = ParameterValidatorType.Range;
                Dictionary<string, object> validatorProperties = new Dictionary<string, object>()
                {
                    {"min",1 },
                    {"max",100 }
                };
                injector.AddCallPolicy()
                    .AddMachRule<MethodNameMatchRule>(new InjectConstructor(interfaceType, "Int32Range1_100"))
                    .AddBehaviour<ParameterValidatorBehaviour>(
                    null,
                    new TypedValue(ParameterValidatorBehaviour.ParameterNameProperty, parameterName),
                    new TypedValue(ParameterValidatorBehaviour.ValidatorTypeProperty, validatorType),
                    new TypedValue(ParameterValidatorBehaviour.ValidatorParametersProperty, validatorProperties));
                parameterName = "n";
                validatorType = ParameterValidatorType.Range;
                validatorProperties = new Dictionary<string, object>()
                {
                    {"min",1 },
                    {"max",int.MaxValue }
                };
                injector.AddCallPolicy()
                    .AddMachRule<MethodNameMatchRule>(new InjectConstructor(interfaceType, "Int32Range1_Max"))
                    .AddBehaviour<ParameterValidatorBehaviour>(
                    null,
                    new TypedValue(ParameterValidatorBehaviour.ParameterNameProperty, parameterName),
                    new TypedValue(ParameterValidatorBehaviour.ValidatorTypeProperty, validatorType),
                    new TypedValue(ParameterValidatorBehaviour.ValidatorParametersProperty, validatorProperties));

                var obj = injector.CreateInstance<IRangeClass>();

                try
                {
                    obj.Int32Range1_100(0);
                    Assert.Fail();
                }
                catch (FailedRangeException ex) { }
                catch { throw; }

                obj.Int32Range1_100(1);
                obj.Int32Range1_100(50);
                obj.Int32Range1_100(100);

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

        [AopServer(typeof(ProxyRangeClass))]
        class RangeManualClass : IRangeClass
        {
            public void Int32Range1_Max(int n)
            {
                Assert.IsTrue(1 <= n && n <= int.MaxValue);
            }

            public void Int64Range1_Max(long n)
            {
                Assert.IsTrue(1 <= n && n <= long.MaxValue);
            }

            public void Float32Range1_Max(float n)
            {
                Assert.IsTrue(1f <= n && n <= float.MaxValue);
            }

            public void Float64Range1_Max(double n)
            {
                Assert.IsTrue(1d <= n && n <= double.MaxValue);
            }
            public void Int32Range1_100(int n)
            {
                Assert.IsTrue(1 <= n && n <= 100);
            }

            public void Int64Range1_100(long n)
            {
                Assert.IsTrue(1 <= n && n <= 100);
            }

            public void Float32Range1_100(float n)
            {
                Assert.IsTrue(1f <= n && n <= 100);
            }

            public void Float64Range1_100(double n)
            {
                Assert.IsTrue(1d <= n && n <= 100);
            }
        }

    }


}
