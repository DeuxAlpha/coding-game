using CodinGame.Utilities.Heaps;

namespace CodinGame.Utilities.Extensions
{
    public static class HeapExtensions
    {
        public static bool Any<T>(this Heap<T> heap) where T : IHeapItem<T>
        {
            return heap.Count > 0;
        }
    }
}