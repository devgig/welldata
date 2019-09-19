using System.Collections.Generic;
using System.Linq;

namespace WellData.Core.Common
{
    public class HashtableList<TKey, TItem>
    {
        private readonly Dictionary<TKey, List<TItem>> _keyToListItems = new Dictionary<TKey, List<TItem>>();

        public void Add(TKey key, TItem item)
        {
            if(!_keyToListItems.ContainsKey(key))
                _keyToListItems[key] = new List<TItem>();

            _keyToListItems[key].Add(item);
        }

        public IEnumerable<TItem> this[TKey key]
        {
            get { return _keyToListItems[key]; }
        }

        public IEnumerable<TItem> GetItems(TKey key)
        {
            return _keyToListItems.ContainsKey(key) ? _keyToListItems[key] : Enumerable.Empty<TItem>();
        }

        public IEnumerable<TKey> Keys
        {
            get { return _keyToListItems.Keys; }
        }

        public bool ContainsKey(TKey key)
        {
            return _keyToListItems.ContainsKey(key);
        }

        public void Remove(TKey key)
        {
            _keyToListItems.Remove(key);
        }

        public bool TryGetValue(TKey key, out IEnumerable<TItem> items)
        {
            List<TItem> outItems;

            var result = _keyToListItems.TryGetValue(key, out outItems);
            items = outItems;

            return result;
        }
    }
}