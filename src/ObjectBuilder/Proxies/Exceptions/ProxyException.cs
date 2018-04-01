/**************************************************************
 *  Filename:    ProxyException.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.Object ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.Proxies
{

    public class ProxyException : Exception
    {
        public ProxyException(string message)
            : base(message)
        {
        }

        public ProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
