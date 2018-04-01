using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LWJ.Injection;
using System.Reflection;
using LWJ.Injection.Aop;

namespace LWJ.Injection.Test
{
    class NotifyPropertyChangedBehavior : ICallHandler
    {

        private event PropertyChangedEventHandler propertyChanged;

        private static readonly MethodInfo addEventMethodInfo = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged").GetAddMethod();

        private static readonly MethodInfo removeEventMethodInfo = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged").GetRemoveMethod();




        public int Order
        {
            get;
            set;
        }

        public ICallReturn Invoke(ICallInvocation invocation, GetNextCallHandlerDelegate getNext)
        {
            if (invocation.MethodBase == addEventMethodInfo)
            {
                return AddEventSubscription(invocation);
            }

            if (invocation.MethodBase == removeEventMethodInfo)
            {
                return RemoveEventSubscription(invocation);
            }

            if (IsPropertySetter(invocation))
            {
                return InterceptPropertySet(invocation, getNext);
            }

            return getNext()(invocation, getNext);
        }




        ICallReturn AddEventSubscription(ICallInvocation invocation)
        {
            var subscriber = (PropertyChangedEventHandler)invocation.Arguments[0];
            propertyChanged += subscriber;
            return invocation.Return(null);
        }


        private ICallReturn RemoveEventSubscription(ICallInvocation invocation)
        {
            var subscriber = (PropertyChangedEventHandler)invocation.Arguments[0];
            propertyChanged -= subscriber;
            return invocation.Return(null);
        }

        private static bool IsPropertySetter(ICallInvocation invocation)
        {
            return invocation.MethodBase.IsSpecialName && invocation.MethodBase.Name.StartsWith("set_");
        }

        private ICallReturn InterceptPropertySet(ICallInvocation invocation, GetNextCallHandlerDelegate getNext)
        {
            var propertyName = invocation.MethodBase.Name.Substring(4);

            var returnValue = getNext()(invocation, getNext);

            var subscribers = propertyChanged;
            if (subscribers != null)
            {
                subscribers(invocation.Target, new PropertyChangedEventArgs(propertyName));
            }

            return returnValue;
        }

    }

    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

    public class PropertyChangedEventArgs : EventArgs
    {
        public string PropertyName { get; set; }

        public PropertyChangedEventArgs(string name)
        {
            this.PropertyName = name;
        }
    }
    public interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }


}
