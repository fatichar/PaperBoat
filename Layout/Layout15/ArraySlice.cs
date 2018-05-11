using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layout15
{
    public class ArraySlice<T> : IEnumerable<T>
    {
        public ImmutableArray<T> SourceArray { get; }
        public int Offset { get; }

        public ArraySlice(ImmutableArray<T> sourceArray, int offset, int length)
        {
            SourceArray  = sourceArray;
            Offset      = offset;
            Length       = length;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                {
                    throw new ArgumentOutOfRangeException(index, nameof(index), 0, Length - 1);
                }
                return SourceArray[Offset + index];
            }
        }

        public int Length { get; }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) SourceArray).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
