/**************************************************************
 *  Filename:    PersistentLifetime.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.Injection
{

    public class PersistentLifetime : ILifetime
    {
        private object value;


        public object GetValue()
        {
            return value;
        }

        public void SetValue(object value)
        {
            this.value = value;
        }

        public void RemoveValue()
        {
            this.value = null;
        }
         

    }

}
