using System;
using System.Collections;
using System.Collections.Generic;

namespace OmiyaGames
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="BidirectionalDictionary.cs" company="Omiya Games">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2020-2020 Omiya Games
	/// 
	/// Permission is hereby granted, free of charge, to any person obtaining a copy
	/// of this software and associated documentation files (the "Software"), to deal
	/// in the Software without restriction, including without limitation the rights
	/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	/// copies of the Software, and to permit persons to whom the Software is
	/// furnished to do so, subject to the following conditions:
	/// 
	/// The above copyright notice and this permission notice shall be included in
	/// all copies or substantial portions of the Software.
	/// 
	/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	/// THE SOFTWARE.
	/// </copyright>
	/// <list type="table">
	/// <listheader>
	/// <term>Revision</term>
	/// <description>Description</description>
	/// </listheader>
	/// <item>
	/// <term>
	/// <strong>Version:</strong> 0.1.0-preview.1<br/>
	/// <strong>Date:</strong> 4/4/2020<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>Initial version.</description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 0.1.4-preview.1<br/>
	/// <strong>Date:</strong> 5/27/2020<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Updating documentation to be compatible with DocFX.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 1.2.2<br/>
	/// <strong>Date:</strong> 2/19/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Using <see cref="nameof()"/> for exception handling.
	/// </description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// A two-direction dictionary where a key maps to a value, and vice versa.
	/// This does mean both keys and values must be unique.
	/// </summary>
	public class BidirectionalDictionary<KEY, VALUE> : IDictionary<KEY, VALUE>
	{
		/// <summary>
		/// The dictionary mapping from key to value.
		/// </summary>
		/// <seealso cref="ValueToKeyMap"/>
		private Dictionary<KEY, VALUE> KeyToValueMap
		{
			get;
		}

		/// <summary>
		/// The dictionary mapping from value to key.
		/// </summary>
		/// <seealso cref="KeyToValueMap"/>
		private Dictionary<VALUE, KEY> ValueToKeyMap
		{
			get;
		}

		#region Constructors
		/// <summary>
		/// Default constructor that creates an empty dictionary.
		/// </summary>
		public BidirectionalDictionary()
		{
			KeyToValueMap = new Dictionary<KEY, VALUE>();
			ValueToKeyMap = new Dictionary<VALUE, KEY>();
		}

		/// <summary>
		/// Constructor for setting the initial capacity of the dictionary.
		/// </summary>
		/// <param name="capacity">
		/// Starting capacity of the dictionary.
		/// </param>
		public BidirectionalDictionary(int capacity)
		{
			KeyToValueMap = new Dictionary<KEY, VALUE>(capacity);
			ValueToKeyMap = new Dictionary<VALUE, KEY>(capacity);
		}

		/// <summary>
		/// Constructor for setting the <see cref="IEqualityComparer{T}"/>
		/// to check equality of keys and values.
		/// </summary>
		/// <param name="keyComparer">Comparer for keys.</param>
		/// <param name="valueComparer">Comparer for values.</param>
		public BidirectionalDictionary(IEqualityComparer<KEY> keyComparer, IEqualityComparer<VALUE> valueComparer)
		{
			KeyToValueMap = new Dictionary<KEY, VALUE>(keyComparer);
			ValueToKeyMap = new Dictionary<VALUE, KEY>(valueComparer);
		}

		/// <summary>
		/// Constructor for setting the initial capacity and
		/// <see cref="IEqualityComparer{T}"/> of keys and values.
		/// </summary>
		/// <param name="capacity">
		/// Starting capacity of the dictionary.
		/// </param>
		/// <param name="keyComparer">Comparer for keys.</param>
		/// <param name="valueComparer">Comparer for values.</param>
		public BidirectionalDictionary(int capacity, IEqualityComparer<KEY> keyComparer, IEqualityComparer<VALUE> valueComparer)
		{
			KeyToValueMap = new Dictionary<KEY, VALUE>(capacity, keyComparer);
			ValueToKeyMap = new Dictionary<VALUE, KEY>(capacity, valueComparer);
		}

		/// <summary>
		/// Copies the content of another <see cref="Dictionary{TKey, TValue}"/>
		/// to this one.
		/// </summary>
		/// <param name="dictionary"><see cref="Dictionary{TKey, TValue}"/> to clone.</param>
		public BidirectionalDictionary(Dictionary<KEY, VALUE> dictionary) : this(dictionary, dictionary.Comparer, null)
		{ }

		/// <summary>
		/// Clones a <see cref="BidirectionalDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="dictionaryToClone"><see cref="BidirectionalDictionary{TKey, TValue}"/> to clone.</param>
		public BidirectionalDictionary(BidirectionalDictionary<KEY, VALUE> dictionaryToClone) : this(dictionaryToClone, dictionaryToClone.KeyComparer, dictionaryToClone.ValueComparer)
		{ }

		/// <summary>
		/// Copies the content of another <see cref="IDictionary{TKey, TValue}"/>
		/// to this one.
		/// </summary>
		/// <param name="dictionary"><see cref="IDictionary{TKey, TValue}"/> to clone.</param>
		/// <param name="keyComparer">Comparer for keys.</param>
		/// <param name="valueComparer">Comparer for values.</param>
		public BidirectionalDictionary(IDictionary<KEY, VALUE> dictionary, IEqualityComparer<KEY> keyComparer, IEqualityComparer<VALUE> valueComparer) : this(dictionary.Count, keyComparer, valueComparer)
		{
			foreach (KeyValuePair<KEY, VALUE> pair in dictionary)
			{
				Add(pair.Key, pair.Value);
			}
		}
		#endregion

		/// <summary>
		/// The comparer used to check whether two keys equal each other.
		/// </summary>
		public IEqualityComparer<KEY> KeyComparer => KeyToValueMap.Comparer;

		/// <summary>
		/// The comparer used to check whether two values equal each other.
		/// </summary>
		public IEqualityComparer<VALUE> ValueComparer => ValueToKeyMap.Comparer;

		#region Implemented Properties
		/// <summary>
		/// Same as <see cref="GetValue(KEY)"/> and <see cref="SetKey(VALUE, KEY)"/>:
		/// gets and sets a corresponding value.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public VALUE this[KEY key]
		{
			get => GetValue(key);
			set => SetValue(key, value);
		}

		/// <inheritdoc/>
		public ICollection<KEY> Keys => KeyToValueMap.Keys;

		/// <inheritdoc/>
		public ICollection<VALUE> Values => ValueToKeyMap.Keys;

		/// <inheritdoc/>
		public int Count => KeyToValueMap.Count;

		/// <summary>
		/// Always false.
		/// </summary>
		public bool IsReadOnly => false;
		#endregion

		/// <summary>
		/// Adds a new key and value combo, if and only if the key and the values are both unique.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException">Thrown when key or value is null.</exception>
		/// <exception cref="ArgumentException">Thrown when key or value is already in the dictionary.</exception>
		public void Add(KEY key, VALUE value)
		{
			if (ContainsKey(key) == true)
			{
				throw new ArgumentException("Argument is already in the bidirectional dictionary.", nameof(key));
			}
			else if (ContainsValue(value) == true)
			{
				throw new ArgumentException("Argument is already in the bidirectional dictionary.", nameof(value));
			}
			else
			{
				// Make sure the key and the value isn't already in their respective dictionaries.
				KeyToValueMap.Add(key, value);
				ValueToKeyMap.Add(value, key);
			}
		}

		/// <summary>
		/// Gets a corresponding value that's paired with a key.
		/// </summary>
		public VALUE GetValue(KEY key)
		{
			return KeyToValueMap[key];
		}

		/// <summary>
		/// Gets a corresponding key that's paired with a value.
		/// </summary>
		public KEY GetKey(VALUE value)
		{
			return ValueToKeyMap[value];
		}

		/// <summary>
		/// Checks if a key exists, and if so, returns the corresponding value.
		/// </summary>
		public bool TryGetValue(KEY key, out VALUE value)
		{
			return KeyToValueMap.TryGetValue(key, out value);
		}

		/// <summary>
		/// Checks if a value exists, and if so, returns the corresponding key.
		/// </summary>
		public bool TryGetKey(VALUE value, out KEY key)
		{
			return ValueToKeyMap.TryGetValue(value, out key);
		}

		/// <summary>
		/// Checks if a key exists, and the newValue doesn't;
		/// if so, replaces the key's pairing to newValue.
		/// </summary>
		/// <exception cref="KeyNotFoundException">
		/// Thrown if the <typeparamref name="KEY"/> isn't a valid key in the dictionary.
		/// </exception>
		public void SetValue(KEY key, VALUE newValue)
		{
			// First make sure the key is already in the dictionary, AND newValue isn't
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			else if (newValue == null)
			{
				throw new ArgumentNullException(nameof(newValue));
			}
			else if (ContainsKey(key) == false)
			{
				throw new KeyNotFoundException();
			}
			else if (ContainsValue(newValue) == true)
			{
				throw new ArgumentException("Argument is already in the bidirectional dictionary.", nameof(newValue));
			}
			else
			{
				// If so, first remove the old value from the value map
				ValueToKeyMap.Remove(KeyToValueMap[key]);

				// Remap the key to newValue
				KeyToValueMap[key] = newValue;

				// Add the new key to the value map
				ValueToKeyMap.Add(newValue, key);
			}
		}

		/// <summary>
		/// Checks if a value exists, and if so, returns the corresponding key.
		/// </summary>
		public void SetKey(VALUE value, KEY newKey)
		{
			// First make sure the value is already in the dictionary, AND newKey isn't
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}
			else if (newKey == null)
			{
				throw new ArgumentNullException(nameof(newKey));
			}
			else if (ContainsValue(value) == false)
			{
				throw new KeyNotFoundException("Value not found.");
			}
			else if (ContainsKey(newKey) == true)
			{
				throw new ArgumentException("Argument is already in the bidirectional dictionary.", nameof(newKey));
			}
			else
			{
				// If so, first remove the old key from the key map
				KeyToValueMap.Remove(ValueToKeyMap[value]);

				// Remap the value to newKey
				ValueToKeyMap[value] = newKey;

				// Add the new value to the key map
				KeyToValueMap.Add(newKey, value);
			}
		}

		/// <summary>
		/// If this dictionary contains the key, removes it
		/// and the value it's paired with.
		/// </summary>
		/// <param name="key">The key to remove from the dictionary.</param>
		/// <returns>True if key was found and removed from the dictionary.</returns>
		public bool RemoveKey(KEY key)
		{
			// Check if the key exists
			bool returnFlag = false;
			if (ContainsKey(key) == true)
			{
				// If so, remove from the value map first (this is to keep the mapping)
				ValueToKeyMap.Remove(KeyToValueMap[key]);

				// Then remove from the key map
				KeyToValueMap.Remove(key);
				returnFlag = true;
			}
			return returnFlag;
		}

		/// <summary>
		/// If this dictionary contains the value, removes it
		/// and the key it's paired with.
		/// </summary>
		/// <param name="value">The value to remove from the dictionary.</param>
		/// <returns>True if value was found and removed from the dictionary.</returns>
		public bool RemoveValue(VALUE value)
		{
			// Check if the key exists
			bool returnFlag = false;
			if (ContainsValue(value) == true)
			{
				// If so, remove from the key map first (this is to keep the mapping)
				KeyToValueMap.Remove(ValueToKeyMap[value]);

				// Then remove from the value map
				ValueToKeyMap.Remove(value);
				returnFlag = true;
			}
			return returnFlag;
		}

		/// <summary>
		/// Checks if the dictionary contains <paramref name="key"/>.
		/// </summary>
		/// <param name="key"><typeparamref name="KEY"/> to search for.</param>
		/// <returns>True if dictionary contains <paramref name="key"/></returns>
		public bool ContainsKey(KEY key)
		{
			return KeyToValueMap.ContainsKey(key);
		}

		/// <summary>
		/// Checks if the dictionary contains <paramref name="value"/>.
		/// </summary>
		/// <param name="value"><typeparamref name="VALUE"/> to search for.</param>
		/// <returns>True if dictionary contains <paramref name="value"/></returns>
		public bool ContainsValue(VALUE value)
		{
			return ValueToKeyMap.ContainsKey(value);
		}

		#region Implemented Methods
		/// <inheritdoc/>
		public void Clear()
		{
			KeyToValueMap.Clear();
			ValueToKeyMap.Clear();
		}

		/// <inheritdoc/>
		public void Add(KeyValuePair<KEY, VALUE> item)
		{
			Add(item.Key, item.Value);
		}

		/// <summary>
		/// Same as <see cref="RemoveKey(KEY)"/>: removes a key and its corresponding value from the dictionary.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <seealso cref="RemoveKey(KEY)"/>
		public bool Remove(KEY key)
		{
			return RemoveKey(key);
		}

		/// <summary>
		/// Removes a key-value pair if and only if there's a corresponding key-value map.
		/// </summary>
		/// <param name="item">Pairing to remove.</param>
		/// <returns>True if successfully removed; false, otherwise.</returns>
		/// <seealso cref="Contains(KeyValuePair{KEY, VALUE})"/>
		public bool Remove(KeyValuePair<KEY, VALUE> item)
		{
			// Check if the key exists, then check if the argument's value matches with the VALUE mapped in KeyToValueMap
			bool returnFlag = false;
			if (Contains(item) == true)
			{
				// If so, remove the key and value accordingly.
				KeyToValueMap.Remove(item.Key);
				ValueToKeyMap.Remove(item.Value);
				returnFlag = true;
			}
			return returnFlag;
		}

		/// <summary>
		/// Checks if this dictionary contains the key-to-value pairing.
		/// </summary>
		/// <param name="item">Pairing to verify whether this dictionary contains or not.</param>
		/// <returns>True, if and only if this dictionary contains the same pairing as <paramref name="item"/>.</returns>
		public bool Contains(KeyValuePair<KEY, VALUE> item)
		{
			// Check if the key exists, then check if the argument's value matches with the VALUE mapped in KeyToValueMap
			VALUE value;
			return (TryGetValue(item.Key, out value) == true) && (ValueComparer.Equals(item.Value, value) == true);
		}

		/// <inheritdoc/>
		public void CopyTo(KeyValuePair<KEY, VALUE>[] array, int arrayIndex)
		{
			((IDictionary<KEY, VALUE>)KeyToValueMap).CopyTo(array, arrayIndex);
		}

		/// <inheritdoc/>
		public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
		{
			return ((IDictionary<KEY, VALUE>)KeyToValueMap).GetEnumerator();
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<KEY, VALUE>)KeyToValueMap).GetEnumerator();
		}
		#endregion
	}
}
