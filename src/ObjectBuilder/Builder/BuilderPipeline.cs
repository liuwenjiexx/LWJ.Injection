/**************************************************************
 *  Filename:    BuilderPipeline.cs
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

    public class BuilderPipeline
    {
        private IBuilder[] handlers;

        public BuilderPipeline()
        {

        }

        public BuilderPipeline(IEnumerable<IBuilder> handlers)
        {
            if (handlers != null)
                this.handlers = handlers.ToArray();
        }


        public object Build(IBuilderContext context, NextBuilderDelegate target)
        {
            int count = handlers == null ? 0 : handlers.Length;
            if (count <= 0)
                return target(context, null);


            int handleIndex = 0;

            return handlers[0].Build(context, delegate ()
            {
                handleIndex++;
                if (handleIndex < count)
                    return handlers[handleIndex].Build;
                return target;
            });
        }


    }
}
