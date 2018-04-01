using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LWJ.Injection;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Runtime.CompilerServices;

namespace LWJ.Injection.Test
{

    //BeforeLog  AfterLog ThrowsLog
    //xml config, mamual config

    [TestClass]
    public class UnitTest1
    {
        class StaticInit
        {
            static StaticInit()
            {

            }
        }
        //[TestMethod]
        public void DllInit()
        {
            var t = typeof(StaticInit);
            //t.GetInterfaceMap() 
            RuntimeHelpers.RunClassConstructor(t.TypeHandle);
            RuntimeHelpers.RunClassConstructor(t.TypeHandle);
        }

        [TestMethod]
        public void TestKeyValuePair2()
        {
            var key1 = new KeyValuePair<int, string>(123, "abc");
            var key11 = new KeyValuePair<int, string>(123, "abc");
            var key2 = new KeyValuePair<int, string>(0, "abc");

            Assert.IsTrue(object.Equals(key1, key11));
            Assert.IsFalse(object.Equals(key1, key2));

            var dic1 = new Dictionary<KeyValuePair<int, string>, int>();
            dic1[key1] = 1;
            dic1[key11] = 2;
            dic1[key2] = 3;
            Assert.AreEqual(dic1[key1], 2);
            Assert.AreEqual(dic1[key2], 3);

            var dic2 = new Dictionary<object, int>();
            dic2[key1] = 1;
            dic2[key11] = 2;
            dic2[key2] = 3;
             
            Assert.AreEqual(dic2[key1], 2);
            Assert.AreEqual(dic2[key2], 3);

        }
        [TestMethod]
        public void TestKeyValuePair3()
        {
            var key1 = new KeyValuePair<int, KeyValuePair<int, string>>(123, new KeyValuePair<int, string>(1, "abc"));
            var key11 = new KeyValuePair<int, KeyValuePair<int, string>>(123, new KeyValuePair<int, string>(1, "abc"));
            var key2 = new KeyValuePair<int, KeyValuePair<int, string>>(0, new KeyValuePair<int, string>(1, "abc"));
            var key3 = new KeyValuePair<int, KeyValuePair<int, string>>(123, new KeyValuePair<int, string>(0, "abc"));

            Assert.IsTrue(object.Equals(key1, key11));
            Assert.IsFalse(object.Equals(key1, key2));
            Assert.IsFalse(object.Equals(key1, key3));

            var dic1 = new Dictionary<KeyValuePair<int, KeyValuePair<int, string>>, int>();
            dic1[key1] = 1;
            dic1[key11] = 2;
            dic1[key2] = 3;
            dic1[key3] = 4;
            Assert.AreEqual(dic1[key1], 2);
            Assert.AreEqual(dic1[key2], 3);
            Assert.AreEqual(dic1[key3], 4);
        }


        int count = 100000;
        bool isReflect = false;
        [TestMethod]
        public void Direct_Invoke_Multi_Argument()
        {
            int i = 1;
            string b = "fgsdfsd";
            object aa = new List<int>();
            object[] args = new object[] { i, b, aa, i, b, aa };
            InvokeTest o = new InvokeTest();
            for (int j = count; j > 0; j--)
            {
                o.Invoke_Multi_Argument_(i, b, aa, i, b, aa);
            }

        }

        [TestMethod]
        public void Interface_Direct_Invoke_Multi_Argument()
        {
            int i = 1;
            string b = "fgsdfsd";
            object aa = new List<int>();
            object[] args = new object[] { i, b, aa, i, b, aa };
            InvokeTest o = new InvokeTest();
            for (int j = count; j > 0; j--)
            {
                IInvokeTest o2 = (IInvokeTest)o;

                o2.Invoke_Multi_Argument_(i, b, aa, i, b, aa);
            }

        }

        [TestMethod]
        public void Reflect_Invoke_Multi_Argument()
        {
            var m = typeof(IInvokeTest).GetMethod("Invoke_Multi_Argument_");
            int i = 1;
            string b = "fgsdfsd";
            object aa = new List<int>();
            object[] args = new object[] { i, b, aa, i, b, aa };
            InvokeTest o = new InvokeTest();
            for (int j = count; j > 0; j--)
            {
                //var m1 = typeof(IInvokeTest).GetMethod("Invoke_Multi_Argument_");
                m.Invoke(o, args);
            }
        }

        [TestMethod]
        public void Direct_Invoke_Single_Argument()
        {
            int i = 1;
            string b = "fgsdfsd";
            object aa = new List<int>();
            var arg = new Single_Argument(i, b, aa);
            InvokeTest o = new InvokeTest();
            for (int j = count; j > 0; j--)
            {
                o.Invoke_Single_Argument_(arg);
            }

        }

        [TestMethod]
        public void Reflect_Invoke_Single_Argument()
        {
            int i = 1;
            string b = "fgsdfsd";
            object aa = new List<int>();
            var arg = new Single_Argument(i, b, aa);
            var args = new object[] { arg };
            var m = typeof(IInvokeTest).GetMethod("Invoke_Single_Argument_");

            InvokeTest o = new InvokeTest();
            for (int j = count; j > 0; j--)
            {
                m.Invoke(o, args);
            }

        }

        interface IInvokeTest
        {
            int Invoke_Multi_Argument_(int i, string b, object aa, int i1, string b1, object aa1);
            void Invoke_Single_Argument_(Single_Argument o);
        }

        class InvokeTest : IInvokeTest
        {
            public int Invoke_Multi_Argument_(int i, string b, object aa, int i1, string b1, object aa1)
            {
                return i + i1;
            }
            public void Invoke_Single_Argument_(Single_Argument o)
            {
                int i = o.i;
                string b = o.b;
                object aa = o.aa;
            }
        }

        public class Single_Argument
        {
            public Single_Argument(int i, string b, object aa)
            {
                this.i = i;
                this.b = b;
                this.aa = aa;
            }

            public int i
            {
                get; set;
            }
            public string b
            {
                get; set;
            }
            public object aa
            {
                get; set;
            }
        }

        public static void PrintTime(Action action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);
        }

        int dicCount = 1000;
        [TestMethod]
        public void Dictionary_Int_Key()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            PrintTime(() =>
            {
                for (int i = 0; i < dicCount; i++)
                {
                    dic[i] = 0;
                }
            });

            PrintTime(() =>
            {
                int value;
                for (int i = 0; i < dicCount; i++)
                {

                    dic.TryGetValue(i, out value);
                }
            });
        }

        [TestMethod]
        public void Sorted_Dictionary_Int_Key()
        {
            Type[] o;//= Type.GetTypeArray(null);  //error
                     //o = Type.GetTypeArray(new object[] { 1, "aa", null });
                     //string ss=null;
                     //o = Type.GetTypeArray(new object[] { 1, "aa", ss });//error


            SortedDictionary<int, int> dic = new SortedDictionary<int, int>();

            PrintTime(() =>
            {
                for (int i = 0; i < dicCount; i++)
                {
                    dic[i] = 0;
                }
            });

            PrintTime(() =>
            {
                int value;
                for (int i = 0; i < dicCount; i++)
                {
                    dic.TryGetValue(i, out value);
                }
            });
        }

        //[TestMethod]
        public void Locked_Test()
        {


            object lockObj = new object();

            Func<string, int, StringBuilder, ThreadStart> build = (text, sleep, sb1) =>
           {
               return () =>
               {
                   lock (lockObj)
                   //lock (new object())
                   {
                       Thread.Sleep(sleep);
                       sb1.Append(text);
                   }
               };
           };

            StringBuilder sb = new StringBuilder();


            new Thread(build("a", 30, sb)).Start();
            new Thread(build("b", 5, sb)).Start();
            new Thread(build("c", 20, sb)).Start();
            new Thread(build("d", 5, sb)).Start();
            new Thread(build("h", 5, sb)).Start();
            new Thread(build("e", 5, sb)).Start();
            new Thread(build("l", 5, sb)).Start();
            new Thread(build("l", 5, sb)).Start();
            new Thread(build("o", 0, sb)).Start();

            Thread.Sleep(100);
            Assert.AreEqual("abcdhello", sb.ToString());

        }

        [TestMethod]
        public void WeakReference_Test()
        {
            GCTest obj = new GCTest();
            WeakReference weak = new WeakReference(obj);
            obj = null;
            GC.Collect();
            bool alive = weak.IsAlive;
            Assert.IsFalse(alive);
            obj = weak.Target as GCTest;
            Assert.IsNull(obj);

            weak = new WeakReference(null);
            int n = 0;
            GC.Collect();
            weak = new WeakReference(n);
            n = 1;
            GC.Collect();
        }
        private void PrivateMethod(int n)
        {

        }
        // [TestMethod]
        public void AAA()
        {
            var type = this.GetType();
            var aa = type.GetMethod("WeakReference_Test", new Type[] { });
            aa = type.GetMethod("PrivateMethod", new Type[] { typeof(int) });
        }



        class GCTest
        {

            ~GCTest()
            {

            }
        }



        /*
        unity container: https://msdn.microsoft.com/en-us/library/microsoft.practices.unity(v=pandp.20).aspx
            BeforeBuild

            AfterBuild
            RealProxy

            管道+上下文（Pipeline+Context）


            http://www.cnblogs.com/artech/archive/2008/08/06/1261637.html

            aop: http://pandonix.iteye.com/blog/336873/
            Behaviour

            AOP应用
            Authentication 权限
        Caching缓存
        Context passing内容传递
        Error handling 错误处理
        Lazy loading 延时加载
        Debugging 调试
        logging, tracing, profiling and monitoring 记录跟踪 优化 校准
        Performance optimization性能优化
        Persistence 持久化
        Resource pooling资源池
        Synchronization 同步
        Transactions事务
        */

        /*
        [TestMethod]
        public void Try_Throw()
        {
            try
            {
                Console.WriteLine("try");
                throw new Exception("AA");
            }
            catch
            {
                Console.WriteLine("catch");
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }
        [TestMethod]
        public void Try()
        {
            try
            {
                Console.WriteLine("try");

                Console.WriteLine("return");
            }
            catch
            {
                Console.WriteLine("catch");
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }

        [TestMethod]
        public void Try_Not_Throw_Time()
        {
            int n = 10000;
            while (n-- > 0)
            {
                try
                {
                    try
                    {
                        try
                        {
                            int a;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception ex)
                {

                }


            }
        }

        [TestMethod]
        public void Try_Time()
        {
            int n = 10000;
            while (n-- > 0)
            {
                try
                {
                    throw new Exception("");
                }
                catch (Exception ex)
                {

                }
            }
        }

        [TestMethod]
        public void Try_Time2()
        {
            int n = 10000;
            while (n-- > 0)
            {

                try
                {
                    try
                    {
                        try
                        {
                            throw new Exception("");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        */
        /*
        [TestMethod]
        public void Multi_Dictionary()
        {
            int n = 100000;

            Dictionary<object, Dictionary<string, Dictionary<object, object>>> dic;
            dic = new Dictionary<object, Dictionary<string, Dictionary<object, object>>>();

            Dictionary<string, Dictionary<object, object>> dic1;
            Dictionary<object, object> dic2;



            int count = 50;


            for (int i = 0; i < count; i++)
            {

                dic1 = new Dictionary<string, Dictionary<object, object>>();
                for (int j = 0; j < count; j++)
                {
                    dic2 = new Dictionary<object, object>();
                    for (int k = 0; k < count; k++)
                    {
                        dic2[k.ToString()] = k.ToString();
                    }
                    dic1[j.ToString()] = dic2;
                }

                dic[i.ToString()] = dic1;
            }



            while (n-- > 0)
            {
                if (dic.TryGetValue("1", out dic1))
                {
                    if (dic1.TryGetValue("1", out dic2))
                    {
                        object val;
                        if (dic2.TryGetValue("1", out val))
                        {

                        }
                    }
                }

            }
        }

        [TestMethod]
        public void Multi_Tuple_Dictionary()
        {
            int n = 100000;


            Tuple<object, string, object> key;

            Dictionary<Tuple<object, string, object>, object> dic;
            dic = new Dictionary<Tuple<object, string, object>, object>();

            int count = 50;
            object j1;
            string j2;
            object j3;
            for (int i = 0; i < count; i++)
            {
                  j1 = i.ToString();
                for (int j = 0; j < count; j++)
                {
                      j2 = j.ToString();
                    for (int k = 0; k < count; k++)
                    {
                          j3 = k.ToString();
                        key = new Tuple<object, string, object>(j1, j2, j3);
                        dic[key] = j3;
                    }
                }


            }


            key = new Tuple<object, string, object>("1", "1", "1");

            while (n-- > 0)
            {
                object val;
                if (dic.TryGetValue(key, out val))
                {

                }

            }
        }*/

        //void Invoke<T1, T2>(Action<T1, T2> call, T1 arg1, T2 arg2)
        //{
        //     call(arg1,arg2);
        //    var m= call.Method;
        //    var d= m.DeclaringType;
        //}

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
        private class MyAttrAttribute : Attribute
        {
            public string name
            {
                get; set;
            }
        }

        [MyAttr]
        private class BaseClass
        {


        }

        [MyAttr]
        private class ChildClass : BaseClass
        {

        }

        [TestMethod]
        public void TestAttr()
        {
            Type attrType = typeof(MyAttrAttribute);
            Assert.IsTrue(typeof(BaseClass).IsDefined(attrType, true));
            Assert.IsTrue(typeof(BaseClass).IsDefined(attrType, false));
            Assert.IsTrue(typeof(ChildClass).IsDefined(attrType, true));
            Assert.IsTrue(typeof(ChildClass).IsDefined(attrType, false));

            Assert.AreEqual(2, typeof(ChildClass).GetCustomAttributes(true).Length);
            Assert.AreEqual(1, typeof(ChildClass).GetCustomAttributes(false).Length);


        }
        [TestMethod]
        public void TestRes()
        {
            System.Resources.ResourceManager rm;
            rm = new System.Resources.ResourceManager("", typeof(Injector).Assembly);
            //string xml = rm.GetString("injector.xml");

            var assembly = typeof(LWJ.Injection.Aop.Caching.CachingCallAttribute).Assembly;
            using (var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".injection.config"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);


            }

        }

        [TestMethod]
        public void TestXmlSer()
        {
            //XmlSerializer ser = new XmlSerializer();
            //XmlMapping mapping;
            // XmlSerializer.GenerateSerializer().

        }

        [TestMethod]
        public void TestAAA()
        {
            CreateArray();
            CreateList();
            CreateDictionary();
        }
        void CreateArray()
        {
            Type itemType = typeof(int);
            //var makeArrayType = intType.MakeArrayType();            
            //var array = Activator.CreateInstance(makeArrayType);
            var array = Array.CreateInstance(itemType, 3);
            Assert.IsTrue(typeof(Array).IsAssignableFrom(array.GetType()));
            int[] array2 = (int[])array;
            Array array3 = (Array)array;
            array3.SetValue(0, 1);
        }

        void CreateList()
        {
            Type itemType = typeof(int);
            Type listType = typeof(List<>);
            Type type = listType.MakeGenericType(itemType);
            var list = Activator.CreateInstance(type);
            List<int> list2 = (List<int>)list;
            IList list3 = (IList)list;
        }
        void CreateDictionary()
        {
            Type keyType = typeof(string);
            Type valueType = typeof(int);
            Type dicType = typeof(Dictionary<,>);
            Type type = dicType.MakeGenericType(keyType, valueType);
            var dic = Activator.CreateInstance(type);
            Dictionary<string, int> dic2 = (Dictionary<string, int>)dic;
            IDictionary dic3 = (IDictionary)dic;
        }

        [Serializable]
        class MyXmlData
        {


        }
        /*
        public void Write(XmlNode parent, object value)
        {
            string text = value == null ? "" : value.ToString();
            if (value == null)
                text = string.Empty;
            else if (value is string)
                text = (string)value;
            else
                text = Convert.ToString(value);
            var node = parent.OwnerDocument.CreateElement("string");
            node.InnerText = text;
            parent.AppendChild(node);
        }*/
    }
}
/*AuthorizationCallHandler(IAuthorizationProvider provider, 
                     string operationName, 
                     int order)
[ExceptionCallHandler("exception-policy-name")]

//[PerformanceCounterCallHandler(CategoryName="My Category", 
InstanceName="MyInstance", UseTotalCounter=false)]

    */
/*
    public static void Register(IInjector injector)
    {

        injector.AddCallPolicy(null)
            .AddMachRule<MethodAttributeMatchRule>(new InjectConstructor(typeof(PerformanceTimeAttribute), true))
            // .AddBeforeCall<TimeBeforeCallHandler>()
            .AddBehaviour<TimeBeforeCallHandler>();
    }
    *//*
internal static void Register(IInjector injector)
{

    injector.AddCallPolicy(null)
        .AddMachRule<MethodAttributeMatchRule>(new InjectConstructor(typeof(CachingCallAttribute), true))
        .AddBehaviour<CachingCallBehaviour>();
}
internal static void Register(IInjector injector)
{

    injector.AddCallPolicy(null)
        //  .AddMachRule<MethodAttributeMatchRule>(new InjectConstructor(typeof(CallerOffsetAttribute), true))
        .AddMachRule<ParameterAttributeMatchRule>(new InjectConstructor(typeof(CallerInfoAttribute), true))
        .AddBehaviour<CallerInfoBehavior>();
}

internal static void Register(IInjector injector)
{
    injector.AddCallPolicy()
      .AddMachRule<ParameterAttributeMatchRule>(new InjectConstructor(typeof(ParameterValidatorAttribute), true))
      .AddBehaviour<ParameterValidatorBehaviour>(new InjectConstructor(string.Empty, ParameterValidatorType.UsageAttribute, null));
}*/
      /*
              internal static void Register(IInjector injector)
              {

                  injector.AddCallPolicy(null)
                      .AddMachRule<MethodAttributeMatchRule>(new InjectConstructor(typeof(PermissionAttribute), true))
                      .AddBehaviour<BeforeCallHandler>();
              }*/

/*
    public static Type DynamicCreateType()
    {
        //动态创建程序集  
        AssemblyName DemoName = new AssemblyName("DynamicAssembly");
        AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(DemoName, AssemblyBuilderAccess.RunAndSave);
        //动态创建模块  
        ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(DemoName.Name, DemoName.Name + ".dll");
        //动态创建类MyClass  
        TypeBuilder tb = mb.DefineType("MyClass", TypeAttributes.Public);
        //动态创建字段  
        FieldBuilder fb = tb.DefineField("myField", typeof(System.String), FieldAttributes.Private);
        //动态创建构造函数  
        Type[] clorType = new Type[] { typeof(System.String) };
        ConstructorBuilder cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, clorType);
        //生成指令  
        ILGenerator ilg = cb1.GetILGenerator();//生成 Microsoft 中间语言 (MSIL) 指令  
        ilg.Emit(OpCodes.Ldarg_0);
        ilg.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
        ilg.Emit(OpCodes.Ldarg_0);
        ilg.Emit(OpCodes.Ldarg_1);
        ilg.Emit(OpCodes.Stfld, fb);
        ilg.Emit(OpCodes.Ret);
        //动态创建属性  
        PropertyBuilder pb = tb.DefineProperty("MyProperty", PropertyAttributes.HasDefault, typeof(string), null);
        //动态创建方法  
        MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName;
        MethodBuilder myMethod = tb.DefineMethod("get_Field", getSetAttr, typeof(string), Type.EmptyTypes);
        //生成指令  
        ILGenerator numberGetIL = myMethod.GetILGenerator();
        numberGetIL.Emit(OpCodes.Ldarg_0);
        numberGetIL.Emit(OpCodes.Ldfld, fb);
        numberGetIL.Emit(OpCodes.Ret);
        //使用动态类创建类型  
        Type classType = tb.CreateType();
        //保存动态创建的程序集 (程序集将保存在程序目录下调试时就在Debug下)  
        dynamicAssembly.Save(DemoName.Name + ".dll");
        //创建类  
        return classType;
    }*/
//Microsoft.CSharp.CSharpCodeProvider provider;
//public static Assembly NewAssembly()
//{
//    //创建编译器实例。  
//    provider = new CSharpCodeProvider();
//    //设置编译参数。  
//    paras = new CompilerParameters();
//    paras.GenerateExecutable = false;
//    paras.GenerateInMemory = true;
//    //创建动态代码。  
//    StringBuilder classSource = new StringBuilder();
//    classSource.Append("public   class   DynamicClass n");
//    classSource.Append("{n");
//    //创建属性。  
//    classSource.Append(propertyString("aaa"));
//    classSource.Append(propertyString("bbb"));
//    classSource.Append(propertyString("ccc"));
//    classSource.Append("}");
//    System.Diagnostics.Debug.WriteLine(classSource.ToString());
//    //编译代码。  
//    CompilerResults result = provider.CompileAssemblyFromSource(paras, classSource.ToString());
//    //获取编译后的程序集。  
//    Assembly assembly = result.CompiledAssembly;
//    return assembly;
//}

//static void AA()
//{
//    CodeCompileUnit unit = new CodeCompileUnit();
//    CompilerParameters comParam = new CompilerParameters();
//    comParam.GenerateInMemory = true;
//    comParam.GenerateExecutable = false;

//    CodeNamespace sampleNamespace = new CodeNamespace("LWJ.Inject.Proxy.Generate");
//    sampleNamespace.Imports.Add(new CodeNamespaceImport("System"));

//    CodeTypeDeclaration class1 = new CodeTypeDeclaration("Customer");
//    class1.IsClass = true;

//    sampleNamespace.Types.Add(class1);
//    unit.Namespaces.Add(sampleNamespace);
//    //class1.BaseTypes.Add(typeof(ISay));
//    var provider = new CSharpCodeProvider();
//    var result = provider.CompileAssemblyFromDom(comParam, unit);
//    Assembly assembly = result.CompiledAssembly;
//    Type type = assembly.GetType("LWJ.Inject.Proxy.Generate.Customer");
//    type = Type.GetType("LWJ.Inject.Proxy.Generate.Customer", false);
//}