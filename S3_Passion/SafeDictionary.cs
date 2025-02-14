using System.Collections.Generic;

namespace Passion.S3_Passion
{
	public class SafeDictionary<TKey, TValue>
	{
		private readonly object _safeDictionaryLock = new object();

		private readonly Dictionary<TKey, TValue> _items = new Dictionary<TKey, TValue>();

		public TValue this[TKey key]
		{
			get
			{
				lock (_safeDictionaryLock)
				{
					return _items[key];
				}
			}
			set
			{
				lock (_safeDictionaryLock)
				{
					_items[key] = value;
				}
			}
		}

		public int Count
		{
			get
			{
				lock (_safeDictionaryLock)
				{
					return _items.Count;
				}
			}
		}

		public List<TKey> Keys
		{
			get
			{
				lock (_safeDictionaryLock)
				{
					return new List<TKey>(_items.Keys);
				}
			}
		}

		public List<TValue> Values
		{
			get
			{
				lock (_safeDictionaryLock)
				{
					return new List<TValue>(_items.Values);
				}
			}
		}

		public bool ContainsKey(TKey key)
		{
			lock (_safeDictionaryLock)
			{
				return _items.ContainsKey(key);
			}
		}

		public bool Contains(TValue value)
		{
			return ContainsValue(value);
		}

		private bool ContainsValue(TValue value)
		{
			lock (_safeDictionaryLock)
			{
				return _items.ContainsValue(value);
			}
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			lock (_safeDictionaryLock)
			{
				return _items.TryGetValue(key, out value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			lock (_safeDictionaryLock)
			{
				_items.Add(key, value);
			}
		}

		public void Remove(TKey key)
		{
			lock (_safeDictionaryLock)
			{
				_items.Remove(key);
			}
		}
	}
}
