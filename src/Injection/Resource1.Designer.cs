﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LWJ.Injection {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource1 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource1() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LWJ.Injection.Resource1", typeof(Resource1).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 Class: {0} 的本地化字符串。
        /// </summary>
        internal static string Aop_Method_ClassType {
            get {
                return ResourceManager.GetString("Aop_Method_ClassType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Method Name: {0} 的本地化字符串。
        /// </summary>
        internal static string Aop_Method_MethodName {
            get {
                return ResourceManager.GetString("Aop_Method_MethodName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Method Parameter Not Found 的本地化字符串。
        /// </summary>
        internal static string Aop_Method_Parameter_NotFound {
            get {
                return ResourceManager.GetString("Aop_Method_Parameter_NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Parameter Type Cast Failed, Name:&lt;{0}&gt;, Type:&lt;{1}&gt;, Value:&lt;{2}&gt; 的本地化字符串。
        /// </summary>
        internal static string Aop_Parameter_InvalidCast {
            get {
                return ResourceManager.GetString("Aop_Parameter_InvalidCast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Parameter Not Found, Name:&lt;{0}&gt; 的本地化字符串。
        /// </summary>
        internal static string Aop_Parameter_NotFound {
            get {
                return ResourceManager.GetString("Aop_Parameter_NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Parameter Name: {0} 的本地化字符串。
        /// </summary>
        internal static string Aop_Parameter_ParameterName {
            get {
                return ResourceManager.GetString("Aop_Parameter_ParameterName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Parameter Is Null Or Empty 的本地化字符串。
        /// </summary>
        internal static string Argument_NullOrEmpty {
            get {
                return ResourceManager.GetString("Argument_NullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Parameter Type: {0} 的本地化字符串。
        /// </summary>
        internal static string Argument_ParameterType {
            get {
                return ResourceManager.GetString("Argument_ParameterType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Interface Type Not Registered 的本地化字符串。
        /// </summary>
        internal static string Injection_NotTypeRegistered {
            get {
                return ResourceManager.GetString("Injection_NotTypeRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Interface Value Not Registered 的本地化字符串。
        /// </summary>
        internal static string Injection_NotValueRegistered {
            get {
                return ResourceManager.GetString("Injection_NotValueRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Interface Name: {0} 的本地化字符串。
        /// </summary>
        internal static string Injection_Register_InterfaceName {
            get {
                return ResourceManager.GetString("Injection_Register_InterfaceName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Interface Type: {0} 的本地化字符串。
        /// </summary>
        internal static string Injection_Register_InterfaceType {
            get {
                return ResourceManager.GetString("Injection_Register_InterfaceType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Inject Value Type Convertion Failed 的本地化字符串。
        /// </summary>
        internal static string InjectValue_InvalidCast {
            get {
                return ResourceManager.GetString("InjectValue_InvalidCast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Value Type: {0} 的本地化字符串。
        /// </summary>
        internal static string InjectValue_InvalidCast_FromType {
            get {
                return ResourceManager.GetString("InjectValue_InvalidCast_FromType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Name: {0} 的本地化字符串。
        /// </summary>
        internal static string InjectValue_Name {
            get {
                return ResourceManager.GetString("InjectValue_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Inject Value Not Found 的本地化字符串。
        /// </summary>
        internal static string InjectValue_NotFound {
            get {
                return ResourceManager.GetString("InjectValue_NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Type: {0} 的本地化字符串。
        /// </summary>
        internal static string InjectValue_Type {
            get {
                return ResourceManager.GetString("InjectValue_Type", resourceCulture);
            }
        }
    }
}