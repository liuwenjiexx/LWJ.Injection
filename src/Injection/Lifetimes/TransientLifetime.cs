/**************************************************************
 *  Filename:    TransientLifetime.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Injection
{
    /// <summary>
    /// no storage object
    /// </summary>
    public class TransientLifetime : ILifetime
    {
        public object GetValue()
        {
            return null;
        }

        public void RemoveValue()
        {
        }

        public void SetValue(object value)
        {
        }

    }


}
