/**************************************************************
 *  Filename:    InjectTypeMetadata.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; 
using LWJ.ObjectBuilder;  
using LWJ.Proxies;

namespace LWJ.Injection
{


    internal class InjectTypeMetadata
    {
        public Type type;
        public Type proxyType;
        public IBuilderMember[] builderMembers;
        public ConstructorBuilder constructorBuilder;
        public bool IsPrimitive;
        public object defaultValue;
        private static Dictionary<Type, InjectTypeMetadata> cached = new Dictionary<Type, InjectTypeMetadata>();

        public IBuilder[] builders;

        public static InjectTypeMetadata Get(Type type)
        {

            return cached.GetOrAdd(type, (t) =>
             {

                 InjectTypeMetadata typeInfo;

                 typeInfo = new InjectTypeMetadata();
                 typeInfo.type = type;

                 if (type.IsPrimitive)
                 {
                     typeInfo.IsPrimitive = true;
                     typeInfo.defaultValue = Activator.CreateInstance(type);

                 }
                 else if (type == typeof(string))
                 {
                     typeInfo.IsPrimitive = true;
                     typeInfo.defaultValue = null;
                 }

                 if (typeInfo.IsPrimitive)
                     return typeInfo;
                  

                 BindingFlags bindingFlags = BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.InvokeMethod;

                 var members = type.GetMembers(bindingFlags);
                 List<IBuilderMember> builderMembers = null;

                 foreach (var member in members)
                 {

                     var injAttr = member.GetCustomAttribute<InjectAttribute>(false);
                     if (injAttr == null)
                         continue;

                     IBuilderMember builderMember = null;

                     //if (injectionMember != null)
                     //    throw new Exception(string.Format("class {0}, {1} use multi", t.FullName, typeof(InjecAttribute).Name));

                     switch (member.MemberType)
                     {
                         case MemberTypes.Field:
                             var fInfo = (FieldInfo)member;
                             builderMember = new FieldBuilder(fInfo, InjectorUtils.FromAttributeProvider(fInfo, fInfo.FieldType));
                             break;
                         case MemberTypes.Property:
                             var pInfo = (PropertyInfo)member;
                             builderMember = new PropertyBuilder(pInfo, InjectorUtils.FromAttributeProvider(pInfo, pInfo.PropertyType));
                             break;
                         case MemberTypes.Method:
                             var mInfo = (MethodInfo)member;
                             builderMember = new MethodBuilder(mInfo, InjectorUtils.FromMethod(mInfo));
                             break;
                         case MemberTypes.Constructor:

                             var cInfo = (ConstructorInfo)member;
                             if (ValidateInjectConstructor(cInfo))
                             {
                                 if (typeInfo.constructorBuilder != null)
                                     throw new Exception(string.Format("class {0}, constructor only  use one attribute [{1}]", type.FullName, typeof(InjectAttribute).Name));

                                 typeInfo.constructorBuilder = new ConstructorBuilder(cInfo, InjectorUtils.FromMethod(cInfo));

                             }
                             continue;
                         default:
                             continue;
                     }

                     if (builderMember != null)
                     {
                         if (builderMembers == null)
                             builderMembers = new List<IBuilderMember>();

                         builderMembers.Add(builderMember);
                     }
                 }

                 if (builderMembers != null)
                     typeInfo.builderMembers = builderMembers.ToArray();

                 if (typeInfo.constructorBuilder == null)
                 {
                     var cInfos = members.Where(o => o.MemberType == MemberTypes.Constructor).Select(o => (ConstructorInfo)o);
                     foreach (var cInfo in cInfos)
                     {
                         if (!ValidateInjectConstructor(cInfo))
                             continue;
                         if (cInfo.GetParameters().Length != 0)
                             continue;
                         typeInfo.constructorBuilder = new ConstructorBuilder(cInfo, InjectorUtils.FromMethod(cInfo));
                         break;
                     }

                     if (typeInfo.constructorBuilder == null)
                     {
                         foreach (var cInfo in cInfos)
                         {
                             if (!ValidateInjectConstructor(cInfo))
                                 continue;
                             typeInfo.constructorBuilder = new ConstructorBuilder(cInfo, InjectorUtils.FromMethod(cInfo));
                             break;
                         }
                     }
                 }

                 UpdateTransparentProxy(typeInfo);


                 return typeInfo;
             });
        }

        static bool ValidateInjectConstructor(ConstructorInfo constructor)
        {
            if (constructor.IsStatic)
                return false;
            if (!constructor.IsPublic)
                return false;

            return true;
        }


        static bool UpdateTransparentProxy(InjectTypeMetadata typeMetadata)
        {
            Type targetType = typeMetadata.type;

            var proxyClassAttr = targetType.GetCustomAttribute<CustomProxyAttribute>(true);
            if (proxyClassAttr == null)
                return false;
            Type proxyType = proxyClassAttr.ProxyType;

            if (proxyType == null)
                return false;

            typeMetadata.proxyType = proxyType;
            
            typeMetadata.builders = new IBuilder[] { new CustomProxyBuilder(proxyClassAttr.ProxyType, proxyClassAttr.GetProxyServerType()) };
            return true;
        }
        
    }
}
