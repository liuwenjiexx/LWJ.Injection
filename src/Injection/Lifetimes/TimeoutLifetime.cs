/**************************************************************
 *  Filename:    TimeoutLifetime.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.Injection
{

    /// <summary>
    /// timeout auto remove object
    /// </summary>
    public class TimeoutLifetime : ILifetime
    {
        private object value;
        private int milliseconds;
        private int extendMilliseconds;
        private DateTime timeout;

        public TimeoutLifetime(int milliseconds)
            : this(milliseconds, 0)
        {
        }
        public TimeoutLifetime(int milliseconds, int extendMilliseconds)
        {
            this.milliseconds = milliseconds;
            this.extendMilliseconds = extendMilliseconds;
        }

        /// <summary>
        /// auto extend time
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            if (value != null)
            {
                var timeout = this.timeout;
                var t = (DateTime.Now - timeout).TotalMilliseconds;
                if (t > 0)
                {
                    RemoveValue();
                }
                else if (extendMilliseconds > 0 && t < extendMilliseconds)
                {
                    this.timeout = timeout.AddMilliseconds(extendMilliseconds * 1.2d);
                }
            }
            return value;
        }

        public void RemoveValue()
        {
            this.value = null;
        }

        /// <summary>
        /// reset timeout
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(object value)
        {
            this.value = value;
            if (value != null)
            {
                this.timeout = DateTime.Now.AddMilliseconds(milliseconds);
            }
        }

        public void SetTimeout(DateTime timeout)
        {
            this.timeout = timeout;
        }



        public void AddMilliseconds(double milliseconds)
        {
            var newTime = timeout.AddMilliseconds(milliseconds);
            SetTimeout(newTime);
        }

        public void AddSeconds(double seconds)
        {
            var newTime = timeout.AddSeconds(seconds);
            SetTimeout(newTime);
        }



    }
}
