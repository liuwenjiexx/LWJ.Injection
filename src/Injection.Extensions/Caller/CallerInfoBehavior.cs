/**************************************************************
 *  Filename:    CallerInfoBehavior.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using LWJ.ObjectBuilder;

namespace LWJ.Injection.Aop.Caller
{

    public class CallerInfoBehavior : IAopBehaviour
    {
        private bool usageAttribute;

        private int order = 0;
        private static Dictionary<MethodBase, CallerInfoHandler> cachedStaticHandlers;
        private Dictionary<MethodBase, CallerInfoHandler> cachedHandlers;

        [DefaultValue(0)]
        [InjectValue(OffsetProperty)]
        private int offset;

        [DefaultValue(null)]
        [InjectValue(ParameterNameProperty)]
        private string parameterName;

        [DefaultValue(CallerParameterType.MemberName)]
        [InjectValue(CallerTypeProperty)]
        private CallerParameterType callerType;

        public const string OffsetProperty = "offset";
        public const string ParameterNameProperty = "parameterName";
        public const string CallerTypeProperty = "callerType";


        public int Order
        {
            get { return order; }
            set { order = value; }
        }



        public CallerInfoBehavior(int order)
        {
            this.order = order;
        }

        public CallerInfoBehavior(bool usageAttribute)
        {
            this.usageAttribute = usageAttribute;
        }

        public void AddAopHandler(MethodBase methodBase, IList methodHandlers)
        {
            if (usageAttribute)
            {
                AddAopHandlerByUsageAttribute(methodBase, methodHandlers);
            }
            else
            {
                CallerInfoHandler handler;

                if (cachedHandlers == null)
                    cachedHandlers = new Dictionary<MethodBase, CallerInfoHandler>();

                if (!cachedHandlers.TryGetValue(methodBase, out handler))
                {
                    var parameters = methodBase.GetParameters();
                    string paramName = this.parameterName;
                    CallerParamInfo paramInfo = null;
                    for (int i = 0, len = parameters.Length; i < len; i++)
                    {
                        var p = parameters[i];
                        if (p.Name == paramName)
                        {
                            paramInfo = new CallerParamInfo(i, this.callerType);
                            break;
                        }
                    }

                    if (paramInfo == null)
                        throw new AopMethodParameterNotFoundException(methodBase.DeclaringType, methodBase.Name, paramName);
                    handler = new CallerInfoHandler(offset, new CallerParamInfo[] { paramInfo });
                    cachedHandlers[methodBase] = handler;
                }

                if (handler != null)
                    methodHandlers.Add(handler);
            }
        }
        private void AddAopHandlerByUsageAttribute(MethodBase methodBase, IList methodHandlers)
        {
            CallerInfoHandler callerInfo;

            if (cachedStaticHandlers == null)
                cachedStaticHandlers = new Dictionary<MethodBase, CallerInfoHandler>();

            if (!cachedStaticHandlers.TryGetValue(methodBase, out callerInfo))
            {
                int offset = 0;
                var callerAttr = methodBase.GetCustomAttribute<CallerFrameOffsetAttribute>(false);
                if (callerAttr != null)
                {
                    offset = callerAttr.FrameOffset;
                    offset = Math.Max(offset, 0);
                }

                var ps = methodBase.GetParameters();
                List<CallerParamInfo> callerParameters = null;
                CallerParamInfo callerParameter;
                for (int i = 0, len = ps.Length; i < len; i++)
                {
                    var p = ps[i];
                    callerParameter = null;

                    if (p.IsDefined(typeof(CallerMemberNameAttribute), false))
                    {
                        callerParameter = new CallerParamInfo(i, CallerParameterType.MemberName);
                    }
                    else if (p.IsDefined(typeof(CallerFilePathAttribute), false))
                    {
                        callerParameter = new CallerParamInfo(i, CallerParameterType.FilePath);
                    }
                    else if (p.IsDefined(typeof(CallerLineNumberAttribute), false))
                    {
                        callerParameter = new CallerParamInfo(i, CallerParameterType.LineNumber);
                    }

                    if (callerParameter != null)
                    {
                        if (callerParameters == null)
                            callerParameters = new List<CallerParamInfo>();
                        callerParameters.Add(callerParameter);
                    }
                }

                if (callerParameters == null || callerParameters.Count == 0)
                    callerInfo = null;
                else
                    callerInfo = new CallerInfoHandler(offset, callerParameters.ToArray());
                cachedStaticHandlers[methodBase] = callerInfo;
            }

            if (callerInfo != null)
            {
                methodHandlers.Add(callerInfo);
            }

        }

        class CallerInfoHandler : IBeforeCall
        {
            public int Offset;
            public CallerParamInfo[] Parameters;

            public CallerInfoHandler(int offset, CallerParamInfo[] parameters)
            {
                this.Offset = offset;
                this.Parameters = parameters;
            }

            public void BeforeInvoke(ICallInvocation invocation)
            {
                MethodBase method = invocation.MethodBase;


                StackTrace trace = new StackTrace(true);
                int index = -1;
                var frames = trace.GetFrames();

                for (int i = frames.Length - 1; i >= 0; i--)
                {
                    var t = frames[i];
                    //if (t.GetMethod().DeclaringType == thisType)
                    //{
                    //    index = i;
                    //    break;
                    //}
                    var frameMethod = t.GetMethod();

                    if (frameMethod.Name == method.Name &&
                        frameMethod.DeclaringType.IsAssignableFrom(method.DeclaringType))
                    {
                        index = i;
                        break;
                    }
                }

                if (index < 0)
                    return;
                index += 1;
                index += Offset;

                if (index >= 0 && index < frames.Length)
                {
                    var frame = frames[index];
                    var args = invocation.Arguments;

                    object value;
                    foreach (var it in Parameters)
                    {

                        switch (it.ValueType)
                        {
                            case CallerParameterType.FilePath:
                                value = frame.GetFileName();
                                break;
                            case CallerParameterType.LineNumber:
                                value = frame.GetFileLineNumber();
                                break;
                            case CallerParameterType.MemberName:
                            default:
                                var m = frame.GetMethod();
                                if (m.IsSpecialName)
                                {
                                    int index2 = m.Name.IndexOf("_");
                                    if (index2 >= 0)
                                    {
                                        value = m.Name.Substring(index2 + 1);
                                    }
                                    else
                                    {
                                        value = m.Name;
                                    }
                                }
                                else
                                {
                                    value = m.Name;
                                }
                                break;
                        }
                        args[it.ParameterIndex] = value;
                    }
                }
            }
        }



        private class CallerParamInfo
        {
            public int ParameterIndex;
            public CallerParameterType ValueType;

            public CallerParamInfo(int parameterIndex, CallerParameterType valueType)
            {
                this.ParameterIndex = parameterIndex;
                this.ValueType = valueType;
            }

        }


    }


}
