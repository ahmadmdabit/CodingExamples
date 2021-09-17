using System;
using System.Runtime.Serialization;
using System.Text;

namespace Helpers
{
    public interface ICircularBuffer
    {
        void Put(object element);

        void Remove();

        bool IsFull();

        bool IsEmpty();
    }

    public class CircularBuffer : ICircularBuffer
    {
        public bool AllowOverwriting { get; set; }

        protected object[] _Buffer;

        protected int _Size;
        public int Size { get => _Size; }

        protected int _StartIndex;
        public int StartIndex { get => _StartIndex; }

        protected int _WritePointer;
        public int WritePointer { get => _WritePointer; }

        protected int _ReadPointer;
        public int ReadPointer { get => _ReadPointer; }

        public CircularBuffer(int size, int startIndex, bool allowOverwriting = false)
        {
            if (size <= 0) throw new ArgumentException("'size' argument must be > 0.");
            if (startIndex >= size) throw new ArgumentException("'startIndex' argument must be > 'size' argument.");

            this._Size = size;
            this._StartIndex = startIndex;
            this._WritePointer = startIndex;
            this._ReadPointer = startIndex;
            this._Buffer = new object[this._Size];
            this.AllowOverwriting = allowOverwriting;
        }

        public void Put(object element)
        {
            if (IsFull())
            {
                if (this.AllowOverwriting) _PointerRepostion(ref this._ReadPointer);
                else throw new BufferFullException();
            }
            this._Buffer[this._WritePointer] = element;
            _PointerRepostion(ref this._WritePointer);
            Console.WriteLine(ToString());
        }

        public void Remove()
        {
            if (IsEmpty()) throw new BufferEmptyException();
            this._Buffer[this._ReadPointer] = null;
            _PointerRepostion(ref this._ReadPointer);
            Console.WriteLine(ToString());
        }

        public bool IsFull() => (this._ReadPointer == 0 && this._WritePointer == this._Size - 1) || this._WritePointer == this._ReadPointer - 1;

        public bool IsEmpty() => this._ReadPointer == this._WritePointer;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var element in this._Buffer)
            {
                stringBuilder.Append(element?.ToString() ?? " ").Append(" _ ");
            }
            return stringBuilder.ToString().TrimEnd(' ', '_').TrimEnd();
        }

        protected void _PointerRepostion(ref int pointer)
        {
            if (pointer + 1 == this._Size) pointer = 0;
            else ++pointer;
        }
    }

    #region Exceptions

    public class BufferFullException : Exception
    {
        public BufferFullException()
        {
        }

        public BufferFullException(string message) : base(message)
        {
        }

        public BufferFullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BufferFullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class BufferEmptyException : Exception
    {
        public BufferEmptyException()
        {
        }

        public BufferEmptyException(string message) : base(message)
        {
        }

        public BufferEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BufferEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    #endregion Exceptions
}