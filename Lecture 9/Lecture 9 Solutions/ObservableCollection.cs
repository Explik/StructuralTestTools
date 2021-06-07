using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_9_Solutions
{
    public class ObservableCollection<T> : ICollection<T>, INotifyCollectionChanged
    {
        List<T> _data = new List<T>();

        public int Count => _data.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item)
        {
            _data.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            _data.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return _data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public bool Remove(T item)
        {
            bool wasRemoved = _data.Remove(item);
            if (wasRemoved)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return wasRemoved;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
