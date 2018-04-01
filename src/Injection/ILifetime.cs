/**************************************************************
 *  Filename:    ILifetime.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/

namespace LWJ.Injection
{
    public interface ILifetime
    {

        void SetValue(object value);

        /// <summary>
        /// object invalid return null 
        /// </summary>
        /// <returns></returns>
        object GetValue();


        void RemoveValue();
    } 

}
