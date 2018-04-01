/**************************************************************
 *  Filename:    TransparentProxy.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.Object ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace LWJ.Proxies
{
 

    internal class TransparentProxy : RealProxy
    {
        private ProxyServer proxyServer; 
         
        internal TransparentProxy(Type type)
            : base(type)
        {
        }

        internal TransparentProxy(Type type, ProxyServer proxyServer)
            : base(type)
        {
            this.proxyServer = proxyServer;
        }
        

        public override IMessage Invoke(IMessage msg)
        {
            if (msg is IConstructionCallMessage)
            {
                IConstructionCallMessage constructCallMsg = msg as IConstructionCallMessage;
                IConstructionReturnMessage constructionReturnMessage = this.InitializeServerObject((IConstructionCallMessage)msg);
                SetStubData(this, constructionReturnMessage.ReturnValue);
                 
                return constructionReturnMessage; 
            }
            else
            {
                IMethodCallMessage callMsg = msg as IMethodCallMessage;
                IMessage message;

                try
                {
                    object[] args = callMsg.Args;                     
                    object returnValue;
                     
                    var method = callMsg.MethodBase;
                    string name;
                     
                    name = method.Name;
                    returnValue = proxyServer.Invoke(name, args);
                     
                    message = new ReturnMessage(returnValue, args, args.Length, callMsg.LogicalCallContext, callMsg);
                }
                catch (Exception e)
                {
                    message = new ReturnMessage(e, callMsg);
                }

                return message;
            }

        }
    }


}
