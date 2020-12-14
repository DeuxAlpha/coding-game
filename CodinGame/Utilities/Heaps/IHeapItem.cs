using System;

namespace CodinGame.Utilities.Heaps
{
    public interface IHeapItem<in T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}