/**************************************************************
 *  Filename:    ElementCollection.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;

namespace LWJ.Injection
{

    internal class ElementCollection : IEqualityComparer, ICollection
    {
        private ICollection value;

        public ElementCollection(ICollection value)
        {

            this.value = value;
        }

        public ICollection Value
        {
            get { return value; }
        }

        public int Count
        {
            get
            {
                return value == null ? 0 : value.Count;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var a = obj as ElementCollection;

            if (a == null)
                return false;

            int len, aLen;
            ICollection aItems = a.value;
            if (value == null)
                len = 0;
            else
                len = value.Count;
            if (aItems == null)
                aLen = 0;
            else
                aLen = aItems.Count;

            if (len != aLen)
                return false;

            if (len == 0)
                return true;

            var it1 = value.GetEnumerator();
            var it2 = aItems.GetEnumerator();

            return it1.ItemsEquals(it2);
        }




        public static bool operator ==(ElementCollection a, ElementCollection b)
        {
            if (object.ReferenceEquals(a, b))
                return true;

            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(ElementCollection a, ElementCollection b)
        {
            return !(a == b);
        }


        public override int GetHashCode()
        {
            int hashCode;

            if (value == null)
                hashCode = 0;
            else
                hashCode = value.GetElementsHashCode();

            return hashCode;
        }

        public new bool Equals(object x, object y)
        {
            if (object.ReferenceEquals(x, y))
                return true;
            var x1 = x as ElementCollection;
            if (x1 == null)
                return false;
            return x1.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            var o = obj as ElementCollection;
            if (o == null)
                return 0;
            return o.GetHashCode();
        }

        public void CopyTo(Array array, int index)
        {
            int count = array.Length - index;
            if (value != null)
            {
                int i = 0;
                foreach (var it in value)
                {
                    if (i >= count)
                        break;
                    array.SetValue(it, index + i);
                    i++;
                }
            }
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            if (value == null)
                yield break;
            foreach (var it in value)
                yield return it;
        }
    }
}
