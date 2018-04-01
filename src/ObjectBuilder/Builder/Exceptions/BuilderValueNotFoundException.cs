/**************************************************************
 *  Filename:    BuilderValueNotFoundException.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.ObjectBuilder
{

    public class BuilderValueNotFoundException : BuilderValueException
    {

        public BuilderValueNotFoundException(Type type, string name, string message)
            : base(message, type, name)
        { }

        public BuilderValueNotFoundException(Type type, string name)
            : this(type, name, Resource1.BuilderValue_NotFound)
        { }

    }
}
