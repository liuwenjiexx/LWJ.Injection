/**************************************************************
 *  Filename:    PermissionException.cs
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

    public class PermissionException : Exception
    {
        private string operationName;

        public PermissionException(string message)
            : base(message) { }


        public PermissionException(string message, string operationName)
            : base(message)
        {
            this.operationName = operationName;
        }

        public string OperationName { get => operationName; }


        public override string Message
        {
            get
            {
                string message = base.Message;

                if (!string.IsNullOrEmpty(operationName))
                    message += Environment.NewLine + Resource1.Permission_OperationName.FormatArgs(operationName);

                return message;
            }
        }
    }

}
