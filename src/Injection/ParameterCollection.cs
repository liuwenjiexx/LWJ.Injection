/**************************************************************
 *  Filename:    ParameterCollection.cs
 *  Description: LWJ.Injection ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/2/17
 **************************************************************/
using System;
using System.Collections;
using System.Reflection;

namespace LWJ.Injection
{

    internal class ParameterCollection : IParameterCollection
    {

        private ParameterInfo[] parameters;
        private object[] values;


        public static readonly ParameterCollection Empty = new ParameterCollection(new object[0], new ParameterInfo[0]);


        public ParameterCollection(object[] values, ParameterInfo[] parameters)
        {
            if (values == null)
                this.values = Empty.values;
            else
                this.values = (object[])values.Clone();

            if (parameters == null)
                this.parameters = Empty.parameters;
            else
                this.parameters = (ParameterInfo[])parameters.Clone();

        }


        public object this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();
                return values[index];
            }
            //set => throw new NotSupportedException();
            set { values[index] = value; }
        }

        int ParameterNameToIndex(string parameterName)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Name == parameterName)
                    return i;
            }
            return -1;
        }


        public object this[string parameterName]
        {
            get
            {
                return values[ParameterNameToIndex(parameterName)];
            }
            set => values[ParameterNameToIndex(parameterName)] = value;
            // set => throw new NotSupportedException();
        }

        public int Count { get => values.Length; }

        public bool IsFixedSize { get => true; }

        public bool IsReadOnly { get => true; }

        public bool IsSynchronized { get => false; }

        public object SyncRoot { get => this; }


        public bool Contains(object value)
        {
            return IndexOf(value) >= 0;
        }

        public bool ContainsParameter(string parameterName)
        {
            return ParameterNameToIndex(parameterName) >= 0;
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(values, 0, array, index, array.Length - index);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var o in values)
                yield return o;
        }

        public ParameterInfo GetParameterInfo(string parameterName)
        {
            return parameters[ParameterNameToIndex(parameterName)];
        }

        public ParameterInfo GetParameterInfo(int index)
        {
            return parameters[index];
        }

        public int IndexOf(object value)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (value == null)
                {
                    if (values[i] == null)
                        return i;
                }
                else if (value.Equals(values[i]))
                    return i;
            }
            return -1;
        }



        public string GetParameterName(int index)
        {
            return parameters[index].Name;
        }



        public object[] ToValueArray()
        {
            return (object[])values.Clone();
        }

        #region not supported member

        public int Add(object value) => throw new NotSupportedException();

        public void Insert(int index, object value) => throw new NotSupportedException();

        public void Remove(object value) => throw new NotSupportedException();

        public void RemoveAt(int index) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        #endregion

    }







}
