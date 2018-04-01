/**************************************************************
 *  Filename:    ParameterValidatorBehaviour.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.ObjectBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


namespace LWJ.Injection.Aop.ParameterValidator
{

    public class ParameterValidatorBehaviour : IAopBehaviour
    {

        private int order = 0;

        [InjectValue(ParameterNameProperty)]
        private string parameterName;

        [InjectValue(ValidatorTypeProperty)]
        private ParameterValidatorType validatorType;

        [DefaultValue(null)]
        [InjectValue(ValidatorParametersProperty)]
        private IDictionary<string, object> validatorParameters;

        static readonly IDictionary<string, object> EmptyValidatorParameters = new Dictionary<string, object>();

        private static Dictionary<MethodBase, ParameterValidatorHandler> cachedStaticHandlers;
        private Dictionary<MethodBase, ParameterValidatorHandler> cachedHandlers;

        public const string ParameterNameProperty = "parameterName";
        public const string ValidatorTypeProperty = "validatorType";
        public const string ValidatorParametersProperty = "validatorParameters";

        public int Order
        {
            get { return order; }
            set { order = value; }
        }


        private void AddAopHandlerByUsageAttribute(MethodBase methodBase, IList methodHandlers)
        {
            ParameterValidatorHandler handler;

            if (cachedStaticHandlers == null)
                cachedStaticHandlers = new Dictionary<MethodBase, ParameterValidatorHandler>();

            if (!cachedStaticHandlers.TryGetValue(methodBase, out handler))
            {
                List<int> paramIndexs = null;
                List<IParameterValidator> validators = null;

                var ps = methodBase.GetParameters();

                for (int i = 0, len = ps.Length; i < len; i++)
                {
                    var p = ps[i];

                    foreach (var attr in p.GetCustomAttributes<ParameterValidatorAttribute>(true))
                    {
                        if (paramIndexs == null)
                        {
                            paramIndexs = new List<int>();
                            validators = new List<IParameterValidator>();
                        }
                        paramIndexs.Add(i);
                        IParameterValidator validator = attr.CreateValidator(p);
                        if (validator != null)
                            validators.Add(validator);
                    }
                }
                if (paramIndexs != null)
                    handler = new ParameterValidatorHandler(paramIndexs.ToArray(), validators.ToArray());
                cachedStaticHandlers[methodBase] = handler;
            }

            if (handler != null)
            {
                methodHandlers.Add(handler);
            }
        }

        public void AddAopHandler(MethodBase methodBase, IList methodHandlers)
        {


            if (validatorType == ParameterValidatorType.UsageAttribute)
            {
                AddAopHandlerByUsageAttribute(methodBase, methodHandlers);
            }
            else
            {
                if (cachedHandlers == null)
                    cachedHandlers = new Dictionary<MethodBase, ParameterValidatorHandler>();

                ParameterValidatorHandler handler;

                if (!cachedHandlers.TryGetValue(methodBase, out handler))
                {

                    string paramName = this.parameterName;
                    paramName = paramName.ToLower();
                    int paramIndex = -1;
                    ParameterInfo paramInfo = null;
                    var ps = methodBase.GetParameters();
                    for (int i = 0, len = ps.Length; i < len; i++)
                    {
                        var p = ps[i];
                        if (p.Name.ToLower() == paramName)
                        {
                            paramInfo = p;
                            paramIndex = i;
                            break;
                        }
                    }
                    if (paramInfo == null)
                        throw new AopMethodParameterNotFoundException(methodBase.DeclaringType, methodBase.Name, paramName);

                    var validator = CreateValidator(validatorType, paramInfo.ParameterType, validatorParameters);
                    handler = new ParameterValidatorHandler(paramIndex, validator);

                    cachedHandlers[methodBase] = handler;
                }

                if (handler != null)
                {
                    methodHandlers.Add(handler);
                }

            }
        }

        static internal IParameterValidator CreateValidator(ParameterValidatorType validatorType, Type valueType, IDictionary<string, object> parameters)
        {
            switch (validatorType)
            {
                case ParameterValidatorType.Range:
                    return RangeAttribute.CreateRangeValidator(valueType, parameters);
                case ParameterValidatorType.Regex:
                    return RegexAttribute.CreateRegexValidator(parameters);
                case ParameterValidatorType.NotNull:
                    return NotNullAttribute.NotNullValidator.instance;
                case ParameterValidatorType.InstanceOfType:
                    return InstanceOfTypeAttribute.CreateValidator(parameters);
                case ParameterValidatorType.AssignableFromType:
                    return AssignableFromTypeAttribute.CreateValidator(parameters);

            }
            throw new NotImplementedException(string.Format("validator type <{0}>, value type <{1}>", validatorType, valueType));
        }

        public void OnAopInit()
        {

        }

        class ParameterValidatorHandler : IBeforeCall
        {
            public int[] paramIndexs;
            private IParameterValidator[] validators;
            public ParameterValidatorHandler(int paramIndex, IParameterValidator validator)
                : this(new int[] { paramIndex }, new IParameterValidator[] { validator })
            {
            }
            public ParameterValidatorHandler(int[] paramIndexs, IParameterValidator[] validators)
            {
                this.paramIndexs = paramIndexs ?? throw new ArgumentNullException(nameof(paramIndexs));
                this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
            }

            public void BeforeInvoke(ICallInvocation invocation)
            {
                var methodBase = invocation.MethodBase;
                var args = invocation.Arguments;
                int paramIndex;
                object value;
                IParameterValidator validator;
                for (int i = 0, len = paramIndexs.Length; i < len; i++)
                {
                    paramIndex = paramIndexs[i];
                    validator = validators[i];
                    value = args[paramIndex];
                    if (!validator.Validate(value))
                    {
                        var p = methodBase.GetParameters()[paramIndex];
                        throw validator.GetException(p, value);
                    }
                }
            }
        }


        private class ValidatorInfo
        {
            public int parameterIndex;
            public IParameterValidator validator;
            private static Dictionary<MethodBase, ValidatorInfo[]> cached = new Dictionary<MethodBase, ValidatorInfo[]>();


            public ValidatorInfo(int parameterIndex, IParameterValidator validator)
            {
                this.parameterIndex = parameterIndex;
                this.validator = validator;
            }

            public static ValidatorInfo[] GetOrCache(MethodBase methodBase)
            {
                return cached.GetOrAdd(methodBase, (method) =>
                {
                    ValidatorInfo[] infos;

                    var ps = method.GetParameters();

                    List<ValidatorInfo> lst = new List<ValidatorInfo>(ps.Length);

                    for (int i = 0, len = ps.Length; i < len; i++)
                    {
                        var p = ps[i];

                        foreach (var attr in p.GetCustomAttributes<ParameterValidatorAttribute>(true))
                        {
                            ValidatorInfo info = new ValidatorInfo(i, attr.CreateValidator(p));
                            lst.Add(info);
                        }
                    }


                    if (lst.Count > 0)
                        infos = lst.ToArray();
                    else
                        infos = null;

                    return infos;
                });
            }
        }


    }



    public enum ParameterValidatorType
    {
        Custom,
        UsageAttribute,
        Range,
        Regex,
        NotNull,
        InstanceOfType,
        AssignableFromType,
    }


}
