/**************************************************************
 *  Filename:    PermissionInvalidOperationException.cs
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
    public class PermissionInvalidOperationException : PermissionException
    {
        public PermissionInvalidOperationException(string operationName)
            : base(Resource1.Permission_InvalidOperation, operationName) { }

        public PermissionInvalidOperationException(string operationName, string message)
            : base(message, operationName) { }

    }

}
