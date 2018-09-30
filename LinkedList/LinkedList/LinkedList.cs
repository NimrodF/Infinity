using System.Collections;
using System.Collections.Generic;

namespace GenericLinkedList
{
    public sealed class GenericLinkedList<T> : IEnumerable
    {
        internal sealed class Node
        {
            internal Node(T data, Node next = null)
            {
                Data = data;
                Next = next;
            }

            internal Node()
            {
                Next = null;
            }

            internal T Data { get; set; }

            internal Node Next { get; set; }
        }

        private Node head = null;
        private uint size = 0;

        /// <summary>
        /// Creates an empty linked list
        /// </summary> 
        /// <returns> none </returns>
        /// <remarks> Constructor </remarks>
        public GenericLinkedList()
        {
            head = new Node();
        }

        /// <summary>
        /// The first data item in the list.
        /// </summary>
        /// <remarks> Property </remarks>
        public T Head
        {
            get { return head.Next.Data; }
        }


        /// <summary>
        /// Adds a data item to the list.
        /// </summary>
        /// <param name="data"> The data to be added to beginning of the list. </param>
        /// <returns> none </returns>
        /// <remarks> Item is pushed to the head of the list. </remarks> 
        public void PushBack(T data)
        {
            Node new_node = new Node(data, null);
            Node currNode = head;

            while (currNode.Next != null)
            {
                currNode = currNode.Next;
            }

            currNode.Next = new_node;
            
            ++size;
        }

        /// <summary>
        /// Removes the first data item from the list.
        /// </summary>
        /// <returns> none </returns>
        /// <remarks> Method </remarks>
        public void Pop()
        {
            if (!IsEmpty())
            {
                head.Next = head.Next.Next;
                --size;
            }
        }

        /// <summary>
        /// Checks if the list is empty
        /// </summary>
        /// <returns> true if the list is empty, false otherwise </returns>
        /// <remarks> Method </remarks>
        public bool IsEmpty()
        {
            return head.Next == null;
        }

        /// <summary>
        /// The number of data items in the list.
        /// </summary>
        /// <remarks> Property </remarks>
        public uint Count
        {
            get { return size; }
        }

        /// <summary>
        /// Empties the list.
        /// </summary>
        /// <remarks> Method </remarks>
        public void Clear()
        {
            head.Next = null;
            size = 0;
        }

        /// <summary>
        /// Generates a list iterator.
        /// </summary>
        /// <returns> A list iterator. </returns>
        /// <remarks> Method </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            Node curr_node = head.Next;
            while (curr_node != null)
            {
                yield return curr_node.Data;
                curr_node = curr_node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}