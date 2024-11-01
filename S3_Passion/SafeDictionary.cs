using System.Collections.Generic;

namespace S3_Passion
{
	public class SafeDictionary<TKey, TValue>
	{
		private readonly object SafeDictionaryLock = new object();

		private readonly Dictionary<TKey, TValue> Items = new Dictionary<TKey, TValue>();

		public TValue this[TKey key]
		{
			get
			{
				lock (SafeDictionaryLock)
				{
					return Items[key];
				}
			}
			set
			{
				lock (SafeDictionaryLock)
				{
					Items[key] = value;
				}
			}
		}

		public int Count
		{
			get
			{
				lock (SafeDictionaryLock)
				{
					return Items.Count;
				}
			}
		}

		public List<TKey> Keys
		{
			get
			{
				lock (SafeDictionaryLock)
				{
					return new List<TKey>(Items.Keys);
				}
			}
		}

		public List<TValue> Values
		{
			get
			{
				lock (SafeDictionaryLock)
				{
					return new List<TValue>(Items.Values);
				}
			}
		}

		public bool ContainsKey(TKey key)
		{
			lock (SafeDictionaryLock)
			{
				return Items.ContainsKey(key);
			}
		}

		public bool Contains(TValue value)
		{
			return ContainsValue(value);
		}

		public bool ContainsValue(TValue value)
		{
			lock (SafeDictionaryLock)
			{
				return Items.ContainsValue(value);
			}
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			lock (SafeDictionaryLock)
			{
				return Items.TryGetValue(key, out value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			lock (SafeDictionaryLock)
			{
				Items.Add(key, value);
			}
		}

		public void Remove(TKey key)
		{
			lock (SafeDictionaryLock)
			{
				Items.Remove(key);
			}
		}
	}
}
