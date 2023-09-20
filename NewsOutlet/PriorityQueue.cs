using System.Collections.Generic;
using System;

public class PriorityQueue<T>
{
    private List<T> _data;
    private Comparison<T> _comparison;

    public PriorityQueue() : this(Comparer<T>.Default) { }

    public PriorityQueue(IComparer<T> comparer) : this((x, y) => comparer.Compare(x, y)) { }

    public PriorityQueue(Comparison<T> comparison)
    {
        _data = new List<T>();
        _comparison = comparison;
    }

    public int Count => _data.Count;

    public void Enqueue(T item)
    {
        _data.Add(item);
        int childIndex = _data.Count - 1;
        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (_comparison(_data[childIndex], _data[parentIndex]) >= 0)
            {
                break;
            }
            Swap(childIndex, parentIndex);
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        int lastIndex = _data.Count - 1;
        T frontItem = _data[0];
        _data[0] = _data[lastIndex];
        _data.RemoveAt(lastIndex);

        --lastIndex;
        int parentIndex = 0;
        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;
            if (leftChildIndex > lastIndex)
            {
                break;
            }
            int rightChildIndex = leftChildIndex + 1;
            if (rightChildIndex <= lastIndex && _comparison(_data[rightChildIndex], _data[leftChildIndex]) < 0)
            {
                leftChildIndex = rightChildIndex;
            }
            if (_comparison(_data[leftChildIndex], _data[parentIndex]) >= 0)
            {
                break;
            }
            Swap(leftChildIndex, parentIndex);
            parentIndex = leftChildIndex;
        }

        return frontItem;
    }

    public T Peek()
    {
        return _data[0];
    }

    public bool Remove(T item)
    {
        int index = _data.IndexOf(item);
        if (index == -1)
        {
            return false;
        }
        int lastIndex = _data.Count - 1;
        _data[index] = _data[lastIndex];
        _data.RemoveAt(lastIndex);

        --lastIndex;
        int parentIndex = (index - 1) / 2;
        if (index > 0 && _comparison(_data[index], _data[parentIndex]) < 0)
        {
            while (index > 0)
            {
                parentIndex = (index - 1) / 2;
                if (_comparison(_data[index], _data[parentIndex]) >= 0)
                {
                    break;
                }
                Swap(index, parentIndex);
                index = parentIndex;
            }
        }
        else
        {
            while (true)
            {
                int leftChildIndex = index * 2 + 1;
                if (leftChildIndex > lastIndex)
                {
                    break;
                }
                int rightChildIndex = leftChildIndex + 1;
                if (rightChildIndex <= lastIndex && _comparison(_data[rightChildIndex], _data[leftChildIndex]) < 0)
                {
                    leftChildIndex = rightChildIndex;
                }
                if (_comparison(_data[leftChildIndex], _data[index]) >= 0)
                {
                    break;
                }
                Swap(leftChildIndex, index);
                index = leftChildIndex;
            }
        }
        return true;
    }

    private void Swap(int i, int j)
    {
        T temp = _data[i];
        _data[i] = _data[j];
        _data[j] = temp;
    }
}