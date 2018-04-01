/**************************************************************
 *  Filename:    IBuilder.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/

namespace LWJ.ObjectBuilder
{

    /// <summary>
    /// Build Object Instance
    /// </summary>
    public interface IBuilder
    {
        object Build(IBuilderContext context, GetNextBuilderDelegate next);

    }

    /*
    public interface IBuilderStrategy
    {

        void BeforeBuilder(IBuilderContext context);


        void AfterBuilder(IBuilderContext context);

    }*/

    //public interface IAfterBuilder
    //{

    //    void OnAfterBuilder();
    //}


    public delegate object NextBuilderDelegate(IBuilderContext context, GetNextBuilderDelegate getNext);

    public delegate NextBuilderDelegate GetNextBuilderDelegate();


}
