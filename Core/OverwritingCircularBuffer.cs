namespace poetools.Core
{
    /// <summary>
    /// A generic circular buffer implementation that overwrites old data to make room for new data.
    /// </summary>
    /// <typeparam name="T">The type to be stored in the buffer.</typeparam>
    public class OverwritingCircularBuffer<T>
    {
        private T[] _array;
        private int _startIndex;
        private int _endIndex;

        public int Count { get; private set; }
        public bool IsFull => Count >= _array.Length;
        
        public OverwritingCircularBuffer(int size)
        {
            _array = new T[size];
            _startIndex = 0;
            _endIndex = 0;
            Count = 0;
        }

        public void Enqueue(T value)
        {
            _array[_endIndex] = value;
            _endIndex = (_endIndex + 1) % _array.Length;

            if (IsFull)
                _startIndex = (_startIndex + 1) % _array.Length;
            
            else Count++;
        }

        public T Dequeue()
        {
            T result = _array[_startIndex];
            _startIndex = (_startIndex + 1) % _array.Length;
            Count--;
            return result;
        }
    }
}