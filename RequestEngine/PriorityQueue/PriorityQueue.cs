using System;
using System.Collections.Generic;
using System.Threading;

namespace RequestEngine
{
    public class PriorityQueue <T> where T: IComparable<T>
    {
        private readonly Semaphore availableTasks = new Semaphore(0, int.MaxValue);
        private readonly Object queueLock = new Object();
        private readonly LinkedList<T> queue = new LinkedList<T>();

        public PriorityQueue()
        {
            // add dummy node to avoid if in push
            queue.AddFirst(new LinkedListNode<T>(default(T)));
        }

        public T Pop()
        {
            T item;

            availableTasks.WaitOne();
            lock (queueLock)
            {
                LinkedListNode<T> node = queue.First.Next;
                item = node.Value;
                queue.Remove(node);
            }

            return item;
        }

        public bool Pop(int millisecondsTimeout, ref T item)
        {
            bool didPop = false;
            item = default(T);

            if (availableTasks.WaitOne(millisecondsTimeout))
            {
                didPop = true;

                lock (queueLock)
                {
                    LinkedListNode<T> node = queue.First.Next;
                    item = node.Value;
                    queue.Remove(node);
                }
            }
            return didPop;
        }

        public void Push(T data)
        {
            lock(queueLock)
            {
                // get the dummy to start iterating
                LinkedListNode<T> currNode = queue.First;
                
                // find insert location without passing last node
                while (currNode.Next != null && data.CompareTo(currNode.Value) >= 0)
                {
                    currNode = currNode.Next;
                }
                
                // move one node back (if not last) to always insert after
                if (currNode.Next != null)
                {
                    currNode = currNode.Previous;
                }

                queue.AddAfter(currNode, data);
                availableTasks.Release();
            }
        }

        public bool IsEmpty()
        {
            return (Count == 0);
        }

        public int Count
        {
            get
            {
                // minus 1 for the dummy
                return queue.Count - 1;
            }
        }
    }
}
