using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineConsultant
{
    public class ListQueue<T> : IEnumerable<T>
    {
        private readonly List<T> _queue = new List<T>();

        private object _sync = new object();


        public void Dequeue(out T item)
        {
            lock (_sync)
            {
                var result = _queue.Last();
                item = result;

                _queue.Remove(result);
            }
        }

        public void Enqueue(T item)
        {
            lock(_sync)
            {
                _queue.Add(item);
            }
        }

        public bool IsEmpty()
        {
            return !_queue.Any();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        public void Remove(T item)
        {
            lock(_sync)
            {
                int pos = _queue.IndexOf(item);
                if (pos > -1)
                    _queue.RemoveAt(pos);
            }
        }

       
    }
}