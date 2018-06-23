// This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team
// and is licensed for your use under the conditions of the NDA or other legally binding contract
// that you or a legal entity you represent has signed with the SharpMedia team.
// In an event that you have received or obtained this file without such legally binding contract
// in place, you MUST destroy all files and other content to which this lincese applies and
// contact the SharpMedia team for further instructions at the internet mail address:
//
//    legal@sharpmedia.com
//
using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Testing;
using System.Collections.ObjectModel;

namespace SharpMedia.Math.Graphs
{
    /// <summary>
    /// A min-heap structure implementation. Heap allows fast element
    /// ordering.
    /// </summary>
    public sealed class MinHeap<T> where T : IComparable<T>
    {
        #region Private Members
        List<T> internalHeap;
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes the parent position of child.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The parent position.</returns>
        private int GetParent(int child)
        {
            return (child - 1) / 2;
        }


        /// <summary>
        /// Gets the first child.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        private int GetFirstChild(int parent)
        {
            return 2 * parent + 1;
        }

        private void SwapElementsUpCond(int parent, int child)
        {
            if (internalHeap[parent].CompareTo(internalHeap[child]) > 0)
            {
                // We swap them.
                T tmp = internalHeap[child];
                internalHeap[child] = internalHeap[parent];
                internalHeap[parent] = tmp;

                if (parent != 0)
                {
                    // We go level up.
                    SwapElementsUpCond(GetParent(parent), parent);
                }
            }
        }

        private void SwapElementsDownCond(int element)
        {
            // Extract children.
            int child = GetFirstChild(element);

            // We check if 1 is in range.
            if (child >= internalHeap.Count) return;

            // We check with child is smaller.
            if (child+1 < internalHeap.Count)
            {
                if (internalHeap[child + 1].CompareTo(internalHeap[child]) <= 0)
                {
                    child = child + 1;
                }
            } 

            // We check if we need swapping.
            if (internalHeap[element].CompareTo(internalHeap[child]) > 0)
            {
                T tmp = internalHeap[child];
                internalHeap[child] = internalHeap[element];
                internalHeap[element] = tmp;

                // Call recursevelly.
                SwapElementsDownCond(child);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap&lt;T&gt;"/> class.
        /// </summary>
        public MinHeap()
        {
            this.internalHeap = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public MinHeap(int capacity)
        {
            this.internalHeap = new List<T>(capacity);
        }

        /// <summary>
        /// Gets the array.
        /// </summary>
        /// <value>The array.</value>
        public List<T> Array
        {
            get
            {

                return internalHeap;
            }
        }

        /// <summary>
        /// Pops this heap, returning the instance. If not found, default is returned.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (internalHeap.Count == 0) return default(T);

            // We pop the top element and replace it with the one at the bottom.
            T toReturn = internalHeap[0];
            T toSwap = internalHeap[internalHeap.Count - 1];
            internalHeap[0] = toSwap;
            internalHeap.RemoveAt(internalHeap.Count - 1);
            

            // We recusevelly check if one of the children is bigger and requires swapping. We
            // swap with smaller child.
            SwapElementsDownCond(0);

            return toReturn;
        }

        /// <summary>
        /// Pushes a new element on the heap.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Push(T value)
        {
            // We push the value T at lastEntry.
            int index = internalHeap.Count;
            internalHeap.Add(value);

            // We swap until ok.
            if(index != 0) SwapElementsUpCond(GetParent(index), index);
        }

        /// <summary>
        /// Returns the top 
        /// </summary>
        public T Top
        {
            get
            {
                if (internalHeap.Count == 0) return default(T);
                return internalHeap[0];
            }
        }

        public uint Count
        {
            get
            {
                return (uint)internalHeap.Count;
            }
        }


        #endregion

    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class MinHeapTest
    {
        [CorrectnessTest]
        public void PushAndPop()
        {
            MinHeap<int> heap = new MinHeap<int>();
            heap.Push(2);
            heap.Push(5);
            heap.Push(1);
            heap.Push(11);
            heap.Push(-2);
            heap.Push(3);
            heap.Push(2);

            Assert.AreEqual(-2, heap.Pop());
            Assert.AreEqual(1, heap.Pop());
            Assert.AreEqual(2, heap.Pop());
            Assert.AreEqual(2, heap.Pop());
            Assert.AreEqual(3, heap.Pop());
            Assert.AreEqual(5, heap.Pop());
            Assert.AreEqual(11, heap.Pop());

        }

    }
#endif
}
