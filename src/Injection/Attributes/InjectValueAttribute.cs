/**************************************************************
 *  Filename:    InjectValueAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.Injection
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class InjectValueAttribute : InjectAttribute
    { 

        public InjectValueAttribute(string name)
        {
            this.Name = name;
        }
          
        public InjectValueAttribute(Type type, string name)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            this.Name = name;
            this.Type = type;
        }
         
        public Type Type { get; set; }

        public string Name { get; set; }
          
    }


}
