/**************************************************************
 *  Filename:    TypedValue.cs
 *  Copyright:  © 2017 WenJie Liu. All rights reserved.
 *  Description: LWJ.ObjectBuilder ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections.Generic;

namespace LWJ.ObjectBuilder
{

    public class TypedValue : IBuilderValue
    {

        private Type matchType;
        private string matchName;
        private object value;

        /// <summary>
        /// match type is value type
        /// </summary>
        /// <param name="matchName"></param>
        /// <param name="value">not null</param>
        public TypedValue(string matchName, object value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
            this.matchType = value.GetType();
            if (matchName == "")
                matchName = null;
            this.matchName = matchName;
            this.value = value;
        }

        public TypedValue(Type matchType, string matchName, object value)
        {

            this.matchType = matchType ?? throw new ArgumentNullException("matchType");

            if (!matchType.IsAssignableFrom(value.GetType()))
                throw new ArgumentException("match type not is assignable from value type", nameof(value));

            if (matchName == "")
                matchName = null;
            this.matchName = matchName;
            this.value = value;

        }
        public Type MatchType
        {
            get { return matchType; }
        }

        public string MatchName
        {
            get { return matchName; }
        }

        public object Value
        {
            get { return value; }
        }

        public bool IsMatchValue(Type type, string name)
        {
            if (type == null)
                return false;
            if (name == "")
                name = null;

            if (matchName != name)
                return false;

            if (!type.IsAssignableFrom(matchType))
                return false;

            return true;
        }

        public object GetValue(Type type, string name)
        {
            if (!IsMatchValue(type, name))
                throw new ArgumentException("not match value");
            return value;
        }

         
    }


}
