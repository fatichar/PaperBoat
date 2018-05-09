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
        private readonly ImmutableArray<T> _sourceArray;
        private readonly int _offset;

        public ArraySlice(ImmutableArray<T> sourceArray, int offset, int length)
        {
            _sourceArray = sourceArray;
            _offset      = offset;
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
                return _sourceArray[_offset + index];
            }
        }

        public int Length { get; }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) _sourceArray).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
