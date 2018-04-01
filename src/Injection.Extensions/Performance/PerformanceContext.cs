/**************************************************************
 * Filename:    PerformanceContext.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.Injection.Aop.Performance
{
    public class PerformanceContext
    {
        private object target;
        private MethodBase method;
        //private object[] args;
        //private string type;
        //private object value;

        public object Target { get => target; set => target = value; }
        public MethodBase Method { get => method; set => method = value; }
        //public object[] Arguments { get => args; set => args = value; }
        //public string Type { get => type; set => type = value; }
        //public object Value { get => value; set => this.value = value; }
    }

    public class PerformanceTimeContext : PerformanceContext
    {
        private DateTime startTime;
        private DateTime endTime;
        private TimeSpan? elapsed;

        public DateTime StartTime { get => startTime; internal set => startTime = value; }
        public DateTime EndTime
        {
            get => endTime;
            internal set
            {
                endTime = value;
                elapsed = null;
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                if (elapsed == null)
                    elapsed = endTime - startTime;
                return elapsed.Value;
            }
        }
    }

}
