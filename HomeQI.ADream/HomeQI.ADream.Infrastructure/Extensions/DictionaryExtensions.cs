namespace System.Collections.Generic
{
    public  static class DictionaryExtensions
    {
        public static TValue GetOrAddNew<TKey, TValue>(
             this IDictionary<TKey, TValue> source,
             TKey key)
            where TValue : new()
        {
            if (!source.TryGetValue(key, out var value))
            {
                value = new TValue();
                source.Add(key, value);
            }
            return value;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static TValue Find<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            TKey key)
            => !source.TryGetValue(key, out var value) ? default : value;
    }
}
