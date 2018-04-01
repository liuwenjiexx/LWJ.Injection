/**************************************************************
 *  Filename:    Injector.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using LWJ.Injection.Builder;
using LWJ.Injection.Configuration;
using LWJ.ObjectBuilder;
using LWJ.Proxies; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LWJ.Injection
{

    public class Injector : IInjector
    {
        private string name;
        private Injector parent;
        private List<Injector> children;


        private List<IBuilder> builders;

        private Dictionary<Tuple<Type, string>, TypeMapping> mappingTypes;

        private List<ICallPolicy> policies = new List<ICallPolicy>();
        private bool isDisposed;

        #region static fields

        private static Injector _default;
        private static readonly TransientLifetime transientLifetimeManager = new TransientLifetime();

        private static readonly object lockObj = new object();

        #endregion

        private Injector(string name)
        {

            this.name = name ?? string.Empty;
            mappingTypes = new Dictionary<Tuple<Type, string>, TypeMapping>();

        }

        #region Properties

        private IInjector Parent { get => parent; }

        public string Name { get => name; }

        public static Injector Default
        {
            get
            {
                Injector injector = _default;
                if (injector == null)
                {
                    lock (lockObj)
                    {
                        if (injector == null)
                        {
                            injector = LoadDefault();
                            _default = injector;
                        }
                    }
                }
                return injector;
            }
        }

        #endregion

        public IInjector RegisterType(Type interfaceType, string name, Type targetType, ILifetime lifetime, params IInjectMember[] injectMembers)
        {
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            if (targetType.IsInterface)
                throw new ArgumentException("{0} is interface, type:<{1}>"
                    .FormatArgs(nameof(targetType), targetType.FullName), nameof(targetType));
            if (targetType.IsAbstract)
                throw new ArgumentException("{0} is abstract, type:<{1}>"
                    .FormatArgs(nameof(targetType), targetType.FullName), nameof(targetType));
            if (!interfaceType.IsAssignableFrom(targetType))
                throw new ArgumentException("{0} not assignable from {1}, {0}: <{2}>, {1}: <{3}>"
                    .FormatArgs(nameof(interfaceType), nameof(targetType), interfaceType.FullName, targetType.FullName));

            var mapping = GetMapping(interfaceType, name);
            if (mapping == null)
            {
                mapping = CreateMapping(interfaceType, name);
            }
            InjectTypeMetadata targetMetadata = InjectTypeMetadata.Get(targetType);

            mapping.targetMetadata = targetMetadata;
            var constructorBuilder = targetMetadata.constructorBuilder;

            if (lifetime != null)
                mapping.lifetime = lifetime;

            List<IBuilderMember> buildMembers = new List<IBuilderMember>();

            if (targetMetadata.builderMembers != null)
                buildMembers.AddRange(targetMetadata.builderMembers);

            if (injectMembers != null && injectMembers.Length > 0)
            {

                for (int i = 0, len = injectMembers.Length; i < len; i++)
                {
                    var item = injectMembers[i];
                    if (item is InjectConstructor)
                    {
                        //if (mapping.constructorBuilder != null)
                        //    throw new ArgumentException("Inject constructor only a one", nameof(injectMembers));
                        var injectConstructor = item as InjectConstructor;
                        constructorBuilder = injectConstructor.GetConstructorBuilder(targetType);
                        mapping.builderHandler = null;
                        injectMembers[i] = null;
                        continue;
                    }

                    item.AddBuilderMember(targetType, buildMembers);
                }

            }
            mapping.buildMembers = buildMembers.Count == 0 ? null : buildMembers.ToArray();

            if (constructorBuilder == null)
                throw new InjectionException("{0} not found constructor, type:<{1}>".FormatArgs(nameof(targetType), targetType.FullName));
            mapping.builderHandler = new InjectionDefaultBuilder(/*this,*/ mapping.targetMetadata, constructorBuilder, mapping.buildMembers);

            mapping.flags |= MappingFlags.TypeRegistered;
            return this;
        }

        public IInjector RegisterProxy(Type interfaceType, string name, Type proxyType, Type proxyInvokerType)
        {
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));
            if (proxyType == null) throw new ArgumentNullException(nameof(proxyType));

            //if (!typeof(IProxyInvoker).IsAssignableFrom(proxyType))
            //    throw new ArgumentException("proxy type not implement interface <{0}> type:<{1}>"
            //   .FormatArgs(nameof(IProxyInvoker), proxyType.FullName), nameof(proxyType));

            var mapping = GetOrCreateMapping(interfaceType, name);
            mapping.proxyBuilder = new CustomProxyBuilder(proxyType, proxyInvokerType);
            mapping.flags |= MappingFlags.ProxyRegistered;
            return this;
        }



        public IInjector RegisterValue(Type interfaceType, string name, object value, ILifetime lifetime)
        {
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (!interfaceType.IsAssignableFrom(value.GetType()))
                throw new ArgumentException("register value interface type not assignable from value type, interface type: <{0}>  value type: <{1}> ".FormatArgs(interfaceType.FullName, value.GetType().FullName), nameof(value));

            var mapping = GetOrCreateMapping(interfaceType, name);


            if (lifetime == null)
            {
                lifetime = mapping.lifetime;
                if (lifetime == null)
                {
                    lifetime = new PersistentLifetime();
                    mapping.lifetime = lifetime;
                }
            }
            else
            {
                mapping.lifetime = lifetime;
            }

            lifetime.SetValue(value);

            mapping.flags |= MappingFlags.ValueRegistered;

            return this;
        }

        private void InitMapping(TypeMapping mapping)
        {
            var targetMetadata = mapping.targetMetadata;
            if (targetMetadata != null)
            {
                mapping.buildMembers = targetMetadata.builderMembers;
                var constructorBuilder = targetMetadata.constructorBuilder;
                if (constructorBuilder != null)
                {
                    mapping.builderHandler = new InjectionDefaultBuilder(targetMetadata, constructorBuilder, mapping.buildMembers);
                }

            }

        }


        private TypeMapping GetMapping(Type t, string name)
        {
            name = name ?? string.Empty;
            name = name.ToLower();
            var key = new Tuple<Type, string>(t, name);

            TypeMapping mapping;
            if (!mappingTypes.TryGetValue(key, out mapping))
                return null;

            return mapping;
        }

        private TypeMapping GetOrCreateMapping(Type interfaceType, string name)
        {
            TypeMapping mapping = GetMapping(interfaceType, name);
            if (mapping != null)
                return mapping;
            mapping = CreateMapping(interfaceType, name);

            InjectTypeMetadata targetTypeMetadata = InjectTypeMetadata.Get(interfaceType);
            mapping.targetMetadata = targetTypeMetadata;
            InitMapping(mapping);
            return mapping;
        }

        private TypeMapping CreateMapping(Type interfaceType, string name)
        {
            TypeMapping mapping;

            name = name ?? string.Empty;
            string lowerName = name.ToLower();

            var key = new Tuple<Type, string>(interfaceType, lowerName);
            mapping = new TypeMapping();

            mapping.interfaceType = interfaceType;
            mapping.name = name;
            mapping.flags = MappingFlags.None;

            mappingTypes[key] = mapping;

            return mapping;
        }

        private TypeMapping GetOrCreateMappingInParents(Type type, string name, MappingFlags flags)
        {
            var result = GetMappingInParents(type, name, flags);
            if (result == null && string.IsNullOrEmpty(name))
            {
                result = Default.GetOrCreateMapping(type, name);
            }
            return result;
        }

        private TypeMapping GetMappingInParents(Type type, string name, MappingFlags flags)
        {
            Injector parent = this;
            TypeMapping result = null, tmp;
            do
            {
                tmp = parent.GetMapping(type, name);
                if (tmp != null)
                {
                    if ((tmp.flags & flags) == flags)
                    {
                        result = tmp;
                        break;
                    }
                }
                parent = parent.parent;
            }
            while (parent != null);


            return result;
        }


        public bool IsTypeRegistered(Type interfaceType, string name)
        {
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");
            TypeMapping mapping = GetMappingInParents(interfaceType, name, MappingFlags.TypeRegistered);
            return mapping != null;
        }

        public bool IsProxyRegistered(Type interfaceType, string name)
        {
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));
            TypeMapping mapping = GetMappingInParents(interfaceType, name, MappingFlags.ProxyRegistered);
            return mapping != null;
        }

        public bool IsValueRegistered(Type interfaceType, string name)
        {
            TypeMapping mapping;
            mapping = GetMappingInParents(interfaceType, name, MappingFlags.ValueRegistered);
            if (mapping != null)
            {
                if (mapping.lifetime != null)
                    return mapping.lifetime.GetValue() != null;
            }
            return false;
        }

        public object CreateInstance(Type interfaceType, string name, IBuilderValue[] values)
        {
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));
            if (interfaceType == typeof(IInjector))
                return this;

            TypeMapping mapping = GetOrCreateMappingInParents(interfaceType, name, MappingFlags.TypeRegistered);

            if (mapping == null)
                throw new InjectionException("{0} name not registered, {0}: <{1}>, name: <{2}>".FormatArgs(nameof(interfaceType), interfaceType.FullName, name));



            InjectTypeMetadata targetMetadata = mapping.targetMetadata;
            object instance = null;

            if (targetMetadata.IsPrimitive)
            {
                instance = targetMetadata.defaultValue;
            }
            else
            {

                IBuilderContext context = new InjectionBuilderContext(this, mapping.interfaceType, mapping.name, values);


                IEnumerable<IBuilder> builders = GetAllBuilders();

                //handle proxy
                TypeMapping proxyMapping = GetMappingInParents(interfaceType, name, MappingFlags.ProxyRegistered);
                if (proxyMapping != null)
                {
                    builders = builders.Union(new IBuilder[] { mapping.proxyBuilder });
                }

                if (targetMetadata.builders != null)
                    builders = builders.Union(mapping.targetMetadata.builders);

                var builderPipeline = new BuilderPipeline(builders);


                instance = builderPipeline.Build(context, mapping.builderHandler.Build);

                if (instance == null)
                    throw new InjectionException("build object return null");

                if (!interfaceType.IsAssignableFrom(instance.GetType()))
                    throw new InjectionException("build object not assignable from type :<{0}>".FormatArgs(interfaceType));



            }


            return instance;
        }



        public void Inject(object target, IBuilderValue[] values)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Type targetType = target.GetType();
            //get registered builder members
            var mapping = GetOrCreateMappingInParents(targetType, null, MappingFlags.TypeRegistered);
            var builderMembers = mapping.buildMembers;

            if (builderMembers != null)
            {
                IBuilderContext ctx = new InjectionBuilderContext(this, targetType, null, values);
                foreach (var m in builderMembers)
                    m.Invoke(ctx, target, values);
            }
        }
        public void InjectMember(object target, IInjectMember injectMember, IBuilderValue[] values)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (injectMember == null)
                throw new ArgumentNullException(nameof(injectMember));
            InjectMembers(target, new IInjectMember[] { injectMember }, values);
        }

        public void InjectMembers(object target, IEnumerable<IInjectMember> injectMembers, IBuilderValue[] values)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Type targetType = target.GetType();


            if (injectMembers != null)
            {
                List<IBuilderMember> members = new List<IBuilderMember>();

                foreach (var inject in injectMembers)
                    inject.AddBuilderMember(targetType, members);
                InjectionBuilderContext ctx = new InjectionBuilderContext(this, targetType, null, values);
                foreach (var m in members)
                    m.Invoke(ctx, target, values);
            }

        }





        public IInjector AddBuilder(IBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var list = builders;
            if (list == null)
            {
                list = new List<IBuilder>();
                builders = list;
            }

            if (list.Contains(builder))
                list.Remove(builder);
            list.Insert(0, builder);
            return this;
        }



        public IInjector CreateChild(string name)
        {
            lock (lockObj)
            {
                if (isDisposed)
                    throw new ObjectDisposedException(this.Name);

                Injector child = new Injector(name);

                child.parent = this;
                if (children == null)
                    children = new List<Injector>();
                children.Add(child);

                return child;
            }
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            lock (lockObj)
            {
                if (isDisposed)
                    return;

                if (mappingTypes != null)
                {
                    foreach (var mapping in mappingTypes.Values)
                    {
                        if (mapping.lifetime != null)
                        {
                            if (mapping.lifetime is IDisposable)
                                ((IDisposable)mapping.lifetime).Dispose();
                        }
                    }
                    mappingTypes = null;
                }

                if (children != null)
                {

                    foreach (var child in children.ToArray())
                    {
                        child.Dispose();
                    }
                    children = null;
                }
                if (parent != null)
                {
                    parent.children.Remove(this);
                    parent = null;
                }
                if (_default == this)
                    _default = null;

                isDisposed = true;
            }

        }



        public ICallPolicy AddCallPolicy(string name)
        {
            ICallPolicy policy = new AopPolicy(this, name);
            return AddPolicy(policy);
        }
        public ICallPolicy AddCallPolicy()
        {
            return AddCallPolicy(string.Empty);
        }

        private ICallPolicy AddPolicy(ICallPolicy policy)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            if (policies.Contains(policy))
            {
                policies.Remove(policy);
            }
            policies.Insert(0, policy);
            return policy;
        }


        private IEnumerable<IBuilder> GetAllBuilders()
        {
            var parent = this.parent;
            if (parent != null)
            {
                foreach (var b in parent.GetAllBuilders())
                    yield return b;
            }

            var builds = this.builders;
            if (builds != null)
            {
                foreach (var b in builds)
                    yield return b;
            }
        }

        public IEnumerable<ICallPolicy> GetAllCallPolicies()
        {
            var policies = this.policies;
            if (policies != null)
            {
                foreach (var p in policies)
                    yield return p;
            }

            var parent = this.parent;
            if (parent != null)
            {
                foreach (var p in parent.GetAllCallPolicies())
                    yield return p;
            }

        }

        private bool EqualName(string name)
        {
            string name1 = name ?? string.Empty;
            string name2 = this.name ?? string.Empty;
            if (name1.ToLower() == name2.ToLower())
                return true;
            return false;
        }



        public bool TryGetValue(Type interfaceType, string name, IBuilderValue[] values, out object value)
        {

            if (values != null && values.Length > 0)
            {
                foreach (var v in values)
                {
                    if (v.IsMatchValue(interfaceType, name))
                    {
                        value = v.GetValue(interfaceType, name);
                        return true;
                    }
                }
            }

            var mapping = GetMappingInParents(interfaceType, name, MappingFlags.ValueRegistered);
            if (mapping != null)
            {
                var lifetime = mapping.lifetime;
                value = lifetime.GetValue();
                if (value != null)
                    return true;
            }

            if (interfaceType.IsAssignableFrom(GetType()) && EqualName(name))
            {
                value = this;
                return true;
            }

            value = null;
            return false;
        }


        public object GetOrCreateInstance(Type interfaceType, string name, IBuilderValue[] values)
        {
            object value;

            if (!TryGetValue(interfaceType, name, values, out value))
            {
                value = CreateInstance(interfaceType, name, values);
            }
            return value;
        }


        #region resolve value

        public object Resolve(Type interfaceType, string name, IBuilderValue[] values)
        {
            object result;

            if (TryGetValue(interfaceType, name, values, out result))
                return result;

            if (!IsTypeRegistered(interfaceType, name))
                throw new InjectionNotTypeRegsteredException(interfaceType, name);

            result = this.CreateInstance(interfaceType, name, values);

            this.RegisterValue(interfaceType, name, result);

            return result;
        }



        public void Config(string xml)
        {
            //**
            //using (var reader = new InjectionConfigurationReader(xml))
            //{
            //    reader.Read(this);
            //}
        }



        #endregion


        #region static method


        public static IInjector Create()
        {
            return Create(null);
        }

        public static IInjector Create(string name)
        {
            IInjector injector = Default.CreateChild(name);

            return injector;
        }


        private static Injector LoadDefault()
        {
            var injector = new Injector(null);

            Assembly assembly = null;
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (ass.GetName().Name == "LWJ.Injection.Extensions")
                {
                    assembly = ass;
                    break;
                }
            }
            if (assembly != null)
            {
                using (var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".injection.config"))
                using (var sr = new System.IO.StreamReader(stream, Encoding.UTF8))
                {
                    string xml = sr.ReadToEnd();
                    injector.Config(xml);
                }
            }
            return injector;
        }

        public static IInjector FindByName(string name)
        {
            var parent = Default;

            name = name ?? string.Empty;
            name = name.ToLower();
            string n = parent.Name.ToLower();
            if (name == n)
                return parent;
            return FindByName(parent, name);
        }

        private static IInjector FindByName(Injector parent, string name)
        {

            var children = parent.children;
            if (children == null)
                return null;
            foreach (var c in children)
            {
                if (c.Name.ToLower() == name)
                    return c;

            }
            IInjector result;
            foreach (var c in children)
            {
                result = FindByName(c, name);
                if (result != null)
                    return result;
            }

            return null;
        }


        #endregion



        enum MappingFlags
        {
            None = 0,
            TypeRegistered = 0x1,
            ValueRegistered = 0x2,
            ProxyRegistered = 0x4,

        }

        private class TypeMapping
        {
            public Type interfaceType;

            public string name;
            public InjectTypeMetadata targetMetadata;
            public ILifetime lifetime;
            public MappingFlags flags;
            public IBuilder proxyBuilder;
            public IBuilderMember[] buildMembers;
            public IBuilder builderHandler;

        }


    }







}
