/**************************************************************
 *  Filename:    CallerFrameOffsetAttribute.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;

namespace LWJ.Injection.Aop.Caller
{

    public sealed class CallerFrameOffsetAttribute : Attribute
    {
        public int FrameOffset { get; set; }
        public CallerFrameOffsetAttribute(int frameOffset)
        {
            this.FrameOffset = frameOffset;
        }

      
    }


}
