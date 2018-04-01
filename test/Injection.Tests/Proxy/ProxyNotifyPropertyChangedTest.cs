using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using LWJ.Injection.Aop;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{
    [TestClass]
    public class ProxyNotifyPropertyChangedTest
    {

        interface IPropertyData : INotifyPropertyChanged
        {

            int IntProperty { get; set; }

            bool BoolProperty { get; set; }

            string StringProperty { get; set; }

        }

        class PropertyData : IPropertyData
        {

            public int IntProperty { get; set; }

            public bool BoolProperty { get; set; }

            public string StringProperty { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        class ProxyPropertyData : IPropertyData
        {
            private ProxyServer proxy;
            private IPropertyData target;

            private static readonly MethodInfo PropertyChangedAddMethod = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged").GetAddMethod();
            private static readonly MethodInfo PropertyChangedRemoveMethod = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged").GetRemoveMethod();


            public event PropertyChangedEventHandler PropertyChanged
            {
                add
                {
                    proxy.Invoke(PropertyChangedAddMethod.Name, value);
                }
                remove
                {
                    proxy.Invoke(PropertyChangedRemoveMethod.Name, value);
                }
            }


            private static readonly MethodInfo IntPropertySetMethod = typeof(IPropertyData).GetProperty("IntProperty").GetSetMethod();
            private static readonly MethodInfo IntPropertyGetMethod = typeof(IPropertyData).GetProperty("IntProperty").GetGetMethod();

            public int IntProperty
            {
                get { return (int)proxy.Invoke(IntPropertyGetMethod.Name); }
                set { proxy.Invoke(IntPropertySetMethod.Name, value); }
            }

            private static readonly MethodInfo BoolPropertySetMethod = typeof(IPropertyData).GetProperty("BoolProperty").GetSetMethod();
            private static readonly MethodInfo BoolPropertyGetMethod = typeof(IPropertyData).GetProperty("BoolProperty").GetGetMethod();
            public bool BoolProperty
            {
                get { return (bool)proxy.Invoke(BoolPropertyGetMethod.Name); }
                set { proxy.Invoke(BoolPropertySetMethod.Name, value); }
            }

            private static readonly MethodInfo StringPropertySetMethod = typeof(IPropertyData).GetProperty("StringProperty").GetSetMethod();
            private static readonly MethodInfo StringPropertyGetMethod = typeof(IPropertyData).GetProperty("StringProperty").GetGetMethod();
            public string StringProperty
            {
                get { return (string)proxy.Invoke(StringPropertyGetMethod.Name); }
                set { proxy.Invoke(StringPropertySetMethod.Name, value); }
            }

            public ProxyPropertyData(IInjector injector, IPropertyData target)
            {
                this.target = target;
              //  proxy = InjectProxy.Create(injector, this, target);

            }

            [Inject]
            private void SetInjector(Injector injector)
            {
                injector.Inject(proxy);
            }
        }
         
        //[TestMethod]
        public void NotifyPropertyChangedTest()
        {

            IInjector injector = Injector.Create();

            //injector.AddBuilder(new ProxyBuilderPipeline()
            //    .Add<IPropertyData, PropertyData>());
            //injector.AddCallPolicy(null)
            //    /*.AddCallHander<NotifyPropertyChangedBehavior>();*/
            //    .AddBehaviour<NotifyPropertyChangedBehavior>();
            injector.RegisterType<IPropertyData, PropertyData>();


            var obj = injector.CreateInstance<IPropertyData>();
            Assert.IsInstanceOfType(obj, typeof(ProxyPropertyData));

            sb = new StringBuilder();

            obj.PropertyChanged += Obj_PropertyChanged;


            obj.IntProperty = 123;
            StringAssert.EndsWith(sb.ToString(), "IntProperty=123");

            obj.BoolProperty = true;
            StringAssert.EndsWith(sb.ToString(), "BoolProperty=" + true.ToString());

            obj.StringProperty = "hello world";
            StringAssert.EndsWith(sb.ToString(), "StringProperty=hello world");

        }
        StringBuilder sb;
        private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var data = sender as IPropertyData;
            sb.Append(e.PropertyName + "=" + data.GetType().GetProperty(e.PropertyName).GetValue(data, null));
        }
    }
}
