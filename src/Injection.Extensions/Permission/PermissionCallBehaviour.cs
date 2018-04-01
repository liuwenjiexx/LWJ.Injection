/**************************************************************
 *  Filename:    PermissionCallBehaviour.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace LWJ.Injection.Aop.Permission
{
    /// <summary>
    /// use {Injector.RegisterValue} register {IPermissionProvider} 
    /// </summary>
    public class PermissionCallBehaviour : IAopBehaviour
    {
        private int order;
        private static Dictionary<MethodBase, PermissionHandler> cachedHandlers;

        private PermissionHandler handler;


        public PermissionCallBehaviour(int order)
        {
            this.Order = order;
        }
        public PermissionCallBehaviour()
        {

        }

        public PermissionCallBehaviour(string operationName, string providerName)
        {
            if (operationName == null)
                throw new ArgumentNullException(operationName);

            handler = new PermissionHandler(new string[] { operationName }, new string[] { providerName });
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        private void AddAopHandlerByUsageAttribute(MethodBase methodBase, IList handlers)
        {
            PermissionHandler handler;
            if (cachedHandlers == null)
                cachedHandlers = new Dictionary<MethodBase, PermissionHandler>();

            if (!cachedHandlers.TryGetValue(methodBase, out handler))
            {

                List<string> operationNames = null;
                List<string> authorizationProviderNames = null;

                foreach (var attr in methodBase.GetCustomAttributes<PermissionAttribute>(true))
                {
                    string providerName = attr.GetProviderName();
                    if (operationNames == null)
                    {
                        operationNames = new List<string>();
                        authorizationProviderNames = new List<string>();
                    }
                    operationNames.Add(attr.OperationName);

                    authorizationProviderNames.Add(providerName);
                }

                if (operationNames != null)
                    handler = new PermissionHandler(operationNames.ToArray(), authorizationProviderNames.ToArray());
                cachedHandlers[methodBase] = handler;
            }

            if (handler != null)
                handlers.Add(handler);
        }

        public void AddAopHandler(MethodBase methodBase, IList handlers)
        {
            PermissionHandler handler = this.handler;

            if (handler == null)
            {
                AddAopHandlerByUsageAttribute(methodBase, handlers);
            }
            else
            {
                handlers.Add(handler);
            }

        }


        class PermissionHandler : IBeforeCall
        {
            public string[] operationNames;
            public string[] permissionProviderNames;

            public PermissionHandler(string[] operNames, string[] providerNames)
            {
                this.operationNames = operNames;
                this.permissionProviderNames = providerNames;
            }

            public void BeforeInvoke(ICallInvocation invocation)
            {

                var operNames = operationNames;
                var providerNames = this.permissionProviderNames;
                IPermissionProvider permissionProvider;

                if (operNames.Length > 0)
                {
                    for (int i = 0, len = operNames.Length; i < len; i++)
                    {
                        var operName = operNames[i];
                        var providerName = providerNames[i];

                        if (!invocation.TryGetValue<IPermissionProvider>(providerName, out permissionProvider))
                            throw new PermissionException(operName, "not found <IPermissionProvider>, provider  name: {0}".FormatArgs(providerName));

                        if (!permissionProvider.HasPermission(operName))
                            throw new PermissionInvalidOperationException(operName);
                    }
                }

            }
        }





    }



}
