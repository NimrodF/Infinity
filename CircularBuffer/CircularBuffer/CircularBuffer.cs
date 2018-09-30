using System;
using System.Threading;

namespace ProjectCircularBuffer
{
    public class CircularBuffer<T>
    {
        private T[] buffer;
        private readonly uint size;
        private uint readIndex = 0;
        private uint writeIndex = 0;
        private Semaphore writable;
        private Semaphore readable;
        private readonly Object readLock = new object();
        private readonly Object writeLock = new object();

        public CircularBuffer(uint _size)
        {
            size = _size;
            buffer = new T[size];
            int sizeInSignedInt = (int)size;
            writable = new Semaphore(sizeInSignedInt, sizeInSignedInt);
            readable = new Semaphore(0, sizeInSignedInt);
        }

        public T Read()
        {
            readable.WaitOne();
            T data;

            lock (readLock)
            {
                data = buffer[readIndex];
            }

            readIndex = (readIndex + 1) % size;
            writable.Release();
            return data;
        }

        public void Write(T data)
        {
            writable.WaitOne();

            lock (writeLock)
            {
                buffer[writeIndex] = data;
            }

            writeIndex =(writeIndex + 1) % size;
            readable.Release();
        }

    }
}
