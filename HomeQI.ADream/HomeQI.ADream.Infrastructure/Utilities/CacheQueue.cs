using System.Collections.Generic;

namespace HomeQI.ADream.Infrastructure.HomeQI.ADreamities
{
    /// <summary>
    /// 队列
    /// </summary>
    /// <typeparam name="TKeyType"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public class CacheQueue<TKeyType, TValueType>
    {

        private readonly int _capacity;

        private readonly Queue<TKeyType> _keyQ;

        private readonly Dictionary<TKeyType, TValueType> _contents;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValueType this[TKeyType key]
        {
            get
            {
                if (_contents.TryGetValue(key, out TValueType result))
                {
                    return result;
                }
                return default;
            }
            set
            {
                InternalAdd(key, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialCapacity"></param>
        /// <param name="capacity"></param>
        public CacheQueue(int initialCapacity, int capacity)
        {
            _capacity = capacity;
            _contents = new Dictionary<TKeyType, TValueType>(initialCapacity);
            if (capacity > 0)
            {
                _keyQ = new Queue<TKeyType>(initialCapacity);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKeyType key, TValueType value)
        {
            InternalAdd(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void InternalAdd(TKeyType key, TValueType value)
        {
            if (!_contents.ContainsKey(key) && _capacity > 0)
            {
                _keyQ.Enqueue(key);
                if (_keyQ.Count > _capacity)
                {
                    _contents.Remove(_keyQ.Dequeue());
                }
            }
            _contents[key] = value;
        }
    }

}
