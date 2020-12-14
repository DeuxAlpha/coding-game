namespace CodinGame.Utilities.Heaps
{
    // Source: https://www.youtube.com/watch?v=3Dw5d7PlcTM
    public class Heap<T> where T : IHeapItem<T>
    {
        public T[] Items { get; set; }
        public int Count { get; private set; }

        public Heap(int maxHeapSize)
        {
            Items = new T[maxHeapSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = Count;
            Items[Count] = item;
            SortUp(item);
            Count += 1;
        }

        public T Pop()
        {
            var firstItem = Items[0];
            Count -= 1;
            Items[0] = Items[Count];
            Items[0].HeapIndex = 0;
            SortDown(Items[0]);
            return firstItem;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        public bool Contains(T item)
        {
            return Equals(Items[item.HeapIndex], item);
        }

        private void SortDown(T item)
        {
            while (true)
            {
                var childIndexLeft = item.HeapIndex * 2 + 1;
                var childIndexRight = item.HeapIndex * 2 + 2;

                if (childIndexLeft < Count)
                {
                    var swapIndex = childIndexLeft;
                    if (childIndexRight < Count)
                    {
                        if (Items[childIndexLeft].CompareTo(Items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(Items[swapIndex]) < 0)
                    {
                        Swap(item, Items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void SortUp(T item)
        {
            var parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                var parentItem = Items[parentIndex];

                if (item.CompareTo(parentItem) > 0)
                    Swap(item, parentItem);
                else
                    break;

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        private void Swap(T itemA, T itemB)
        {
            Items[itemA.HeapIndex] = itemB;
            Items[itemB.HeapIndex] = itemA;
            var itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }
}