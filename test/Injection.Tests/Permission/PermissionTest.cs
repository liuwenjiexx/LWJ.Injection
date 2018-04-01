using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using LWJ.Injection.Aop;
using LWJ.Injection.Aop.Permission;
using LWJ.Proxies;

namespace LWJ.Injection.Test
{



    [TestClass]
    public class PermissionTest
    {

        public const string MyAuthenticationProviderName = "myAuth";


        [TestMethod]
        public void Not_Auth_GetName()
        {
            
            using (var injector = Injector.Create())
            { 
                injector.RegisterType<IPermissionClass, PermissionClass>();

                injector.RegisterValue<IPermissionProvider>(MyAuthenticationProviderName, new MyAuthentication());

                var target = injector.Resolve<IPermissionClass>();
                target.No_Auth_GetName();
            }
        }

        [TestMethod]
        public void Auth_GetName()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterValue<IPermissionProvider>(MyAuthenticationProviderName, new MyAuthentication());
                injector.RegisterType<IPermissionClass, PermissionClass>();
                var target = injector.Resolve<IPermissionClass>();
                target.Auth_GetName();
            }
        }


        [TestMethod]
        [ExpectedException(typeof(PermissionInvalidOperationException))]
        public void Auth_GetName1_Fail()
        {
            using (var injector = Injector.Create())
            {
                injector.RegisterValue<IPermissionProvider>(MyAuthenticationProviderName, new MyAuthentication());
                injector.RegisterType<IPermissionClass, PermissionClass>();
                var target = injector.Resolve<IPermissionClass>();
                target.Auth_GetName1_Fail();
            }
        }

        interface IPermissionClass
        {
            void Auth_GetName();
            void Auth_GetName1_Fail();
            void No_Auth_GetName();
        }

        [AopServer(typeof(ProxyPermissionClass))]
        class PermissionClass : IPermissionClass
        {
            [Permission("get_name", MyAuthenticationProviderName)]
            public void Auth_GetName()
            {

            }

            [Permission("get_name1", MyAuthenticationProviderName)]
            public void Auth_GetName1_Fail()
            {
                throw new Exception();
            }

            public void No_Auth_GetName()
            {

            }
        }


        [TransparentProxy]
        private class ProxyPermissionClass : ContextBoundObject, IPermissionClass
        {
            public void Auth_GetName()
            {
                throw new NotImplementedException();
            }

            public void Auth_GetName1_Fail()
            {
                throw new NotImplementedException();
            }

            public void No_Auth_GetName()
            {
                throw new NotImplementedException();
            }
        }



        public class MyAuthentication : IPermissionProvider
        {
            string[] allowOperationNames = new string[] { "get_name", "delete" };

            public bool HasPermission(string operationName)
            {
                return allowOperationNames.Contains(operationName);
            }
        }


    }



}
