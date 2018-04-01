/**************************************************************
 *  Filename:    CallReturn.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;

namespace LWJ.Injection.Aop
{

    internal class CallReturn : ICallReturn
    {
        private Exception exception;
        private Dictionary<string, object> extraData;
        private IParameterCollection outputs;
        private object returnValue;


        public CallReturn(object returnValue)
        {
            this.returnValue = returnValue;
        }

        public CallReturn(Exception exception)
        {
            this.exception = exception;
        }


        public Exception Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        public IDictionary<string, object> ExtraData
        {
            get
            {
                if (extraData == null)
                    extraData = new Dictionary<string, object>();
                return extraData;
            }
        }

        public IParameterCollection Outputs
        {
            get
            {
                //if (outputs == null)
                //    outputs = new Dictionary<string, object>();
                return outputs;
            }
        }

        public object ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }
    }

}
