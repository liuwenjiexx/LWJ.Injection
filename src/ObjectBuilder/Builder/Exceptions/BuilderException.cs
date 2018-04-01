/**************************************************************
 *  Filename:    BuilderException.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/

namespace LWJ.ObjectBuilder
{
    using System;
    public class BuilderException : Exception
    {
        public BuilderException()
        { }

        public BuilderException(string message)
            : base(message)
        { }

        public BuilderException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}
