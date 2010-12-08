using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public class ListAdapter<T> : IEnumerable<T>
    {
        public class NativeListEnumerator<T> : IEnumerator<T>
        {
            private System.Collections.IEnumerator nativeEnumerator;

            public NativeListEnumerator(System.Collections.IEnumerator nativeEnumeratorParameter)
            {
                nativeEnumerator = nativeEnumeratorParameter;
            }

            T IEnumerator<T>.Current
            {
                get { return (T)nativeEnumerator.Current; }
            }

            public void Dispose()
            {
                nativeEnumerator = null;
            }

            public bool MoveNext()
            {
                return nativeEnumerator.MoveNext();
            }

            public void Reset()
            {
                nativeEnumerator.Reset();
            }

            public object Current
            {
                get { return nativeEnumerator.Current; }
            }
        }

        private System.Collections.IEnumerable nativeList;

        public ListAdapter(System.Collections.IEnumerable nativeListParameter)
        {
            nativeList = nativeListParameter;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new NativeListEnumerator<T>(GetEnumerator());
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return nativeList.GetEnumerator();
        }
    }
}
