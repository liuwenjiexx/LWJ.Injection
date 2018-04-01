/**************************************************************
 *  Filename:    PermissionAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection.Aop.Permission
{
    public class PermissionAttribute : Attribute
    {

        private string providerName;

        public PermissionAttribute(string operationName)
        {
            this.OperationName = operationName;
        }


        public PermissionAttribute(string operationName, string providerName)
        {
            this.OperationName = operationName ?? throw new ArgumentNullException(nameof(operationName));

            this.providerName = providerName ?? throw new ArgumentNullException(nameof(providerName));
        }



        public string OperationName { get; set; }


        public virtual string GetProviderName()
        {
            return providerName;
        }


    }
}
