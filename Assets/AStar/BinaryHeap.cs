using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BinaryHeap<T>
{
    protected abstract bool Compare(T current, T other);

    private List<T> list;

    public BinaryHeap()
    {
        list = new List<T>();
    }

    public BinaryHeap(int capacity)
    {
        list = new List<T>(capacity);
    }

    public T this[int index]
    {
        get
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException($"index={index} ,Count={Count}");
            }
            return list[index];
        }
    }

    public int Count
    {
        get
        {
            return list.Count;
        }
    }

    public void Clear()
    {
        list.Clear();
    }

    public T Pop()
    {
        if (list.Count <= 0)
        {
            throw new IndexOutOfRangeException("no element can be pop");
        }
        T result = list[0];
        list[0] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        BubbleDown();
        return result;
    }

    public void Push(T t)
    {
        list.Add(t);
        BubbleUp();
    }

    

    private void BubbleUp()
    {
        int currentIndex = Count - 1;
        while (currentIndex > 0)
        {
            int parentIndex = (currentIndex - 1) >> 1;
            T currentValue = list[currentIndex];
            T parentValue = list[parentIndex];
            if(Compare(parentValue, currentValue))
            {
                list[currentIndex] = parentValue;
                list[parentIndex] = currentValue;
                currentIndex = parentIndex;
            }
            else
            {
                break;
            }
        }
    }


    private void BubbleDown()
    {
        int currentIndex = 0;
        int leftChildIndex = 1;
        int rightChildIndex = 2;
        int biggerChildIndex;

        while (leftChildIndex < Count)
        {
            //Compare 函数中第一个 < 第二个 返回true 时 为大顶堆
            //此时需要和更大的元数较，然后下沉
            if(rightChildIndex < Count && Compare(list[leftChildIndex], list[rightChildIndex]))
            {
                biggerChildIndex = rightChildIndex;
            }
            else
            {
                biggerChildIndex = leftChildIndex;
            }
            T currentValue = list[currentIndex];
            T biggerChildValue = list[biggerChildIndex];

            if(Compare(currentValue, biggerChildValue))
            {
                list[currentIndex] = biggerChildValue;
                list[biggerChildIndex] = currentValue;

                currentIndex = biggerChildIndex;
                leftChildIndex = (currentIndex << 1) + 1;
                rightChildIndex = leftChildIndex + 1;
            }
            else
            {
                break;
            }
        }
    }

   
}

public class MinHeap<T> : BinaryHeap<T> where T : IComparable<T>
{
    public MinHeap() { }
    public MinHeap(int capacity) : base(capacity) { }

    protected override bool Compare(T current, T other)
    {
        return other.CompareTo(current) < 0;
    }
}

public class MaxHeap<T> : BinaryHeap<T> where T : IComparable<T>
{
    public MaxHeap() { }
    public MaxHeap(int capacity) : base(capacity) { }

    protected override bool Compare(T current, T other)
    {
        return other.CompareTo(current) > 0;
    }
}
