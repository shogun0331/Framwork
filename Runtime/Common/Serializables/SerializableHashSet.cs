using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GB
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="SerializableHashSet.cs" company="GB">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2022 GB
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
	/// <strong>Date:</strong> 2/5/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Initial version.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Date:</strong> 2/20/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Updating to match with <see cref="SerializableHelpers"/>'s new
	/// function arguments.
	/// </description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// A serializable <seealso cref="HashSet{T}"/>. Expose it on the inspector
	/// like a normal list.
	/// </summary>
	[Serializable]
	public class SerializableHashSet<T> : ISet<T>, IReadOnlyCollection<T>, ISerializationCallbackReceiver
	{
		[SerializeField]
		List<T> serializedList;
		[SerializeField, HideInInspector]
		bool isSerializing = false;

		readonly HashSet<T> actualSet;

		/// <summary>
		/// Default constructor that sets up an empty set.
		/// </summary>
		public SerializableHashSet()
		{
			serializedList = new List<T>();
			actualSet = new HashSet<T>();
		}

		/// <summary>
		/// Constructor to set the <see cref="IEqualityComparer{T}"/>,
		/// used to check if two elements matches.
		/// </summary>
		/// <param name="comparer">
		/// Comparer to check if two elements matches.
		/// </param>
		public SerializableHashSet(IEqualityComparer<T> comparer)
		{
			serializedList = new List<T>();
			actualSet = new HashSet<T>(comparer);
		}

		/// <summary>
		/// Constructor an empty set with initial capacity defined.
		/// </summary>
		/// <param name="capacity">Initial capacity of this list.</param>
		public SerializableHashSet(int capacity)
		{
			serializedList = new List<T>(capacity);
			actualSet = new HashSet<T>(capacity);
		}

		/// <summary>
		/// Constructor to set the <see cref="IEqualityComparer{T}"/>,
		/// used to check if two elements matches.
		/// </summary>
		/// <param name="capacity">Initial capacity of this set.</param>
		/// <param name="comparer">
		/// Comparer to check if two elements matches.
		/// </param>
		public SerializableHashSet(int capacity, IEqualityComparer<T> comparer)
		{
			serializedList = new List<T>(capacity);
			actualSet = new HashSet<T>(capacity, comparer);
		}

		/// <summary>
		/// Indicates if this collection is in the middle of serializing.
		/// </summary>
		public bool IsSerializing => isSerializing;

		#region ISet<T> Implementations
		/// <inheritdoc/>
		public int Count => actualSet.Count;
		/// <inheritdoc/>
		public bool IsReadOnly => false;
		/// <inheritdoc/>
		public bool Add(T item) => actualSet.Add(item);
		/// <inheritdoc/>
		public void ExceptWith(IEnumerable<T> other) => actualSet.ExceptWith(other);
		/// <inheritdoc/>
		public void IntersectWith(IEnumerable<T> other) => actualSet.IntersectWith(other);
		/// <inheritdoc/>
		public bool IsProperSubsetOf(IEnumerable<T> other) => actualSet.IsProperSubsetOf(other);
		/// <inheritdoc/>
		public bool IsProperSupersetOf(IEnumerable<T> other) => actualSet.IsProperSupersetOf(other);
		/// <inheritdoc/>
		public bool IsSubsetOf(IEnumerable<T> other) => actualSet.IsSubsetOf(other);
		/// <inheritdoc/>
		public bool IsSupersetOf(IEnumerable<T> other) => actualSet.IsSupersetOf(other);
		/// <inheritdoc/>
		public bool Overlaps(IEnumerable<T> other) => actualSet.Overlaps(other);
		/// <inheritdoc/>
		public bool SetEquals(IEnumerable<T> other) => actualSet.SetEquals(other);
		/// <inheritdoc/>
		public void SymmetricExceptWith(IEnumerable<T> other) => actualSet.SymmetricExceptWith(other);
		/// <inheritdoc/>
		public void UnionWith(IEnumerable<T> other) => actualSet.UnionWith(other);
		/// <inheritdoc/>
		public void Clear() => actualSet.Clear();
		/// <inheritdoc/>
		public bool Contains(T item) => actualSet.Contains(item);
		/// <inheritdoc/>
		public void CopyTo(T[] array, int arrayIndex) => actualSet.CopyTo(array, arrayIndex);
		/// <inheritdoc/>
		public bool Remove(T item) => actualSet.Remove(item);
		/// <inheritdoc/>
		public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)actualSet).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)actualSet).GetEnumerator();
		void ICollection<T>.Add(T item) => ((ICollection<T>)actualSet).Add(item);
		#endregion

		#region ISerializationCallbackReceiver Implementations
		/// <inheritdoc/>
		[Obsolete("Manual call not supported.", true)]
		public void OnBeforeSerialize()
		{
			// Indicate we started serializing
			isSerializing = true;

			// Sync this set's data into the list
			SerializableHelpers.PushSetIntoSerializedList(this, serializedList, false);
		}

		/// <inheritdoc/>
		[Obsolete("Manual call not supported.", true)]
		public void OnAfterDeserialize()
		{
			// Sync the list's data into the set.
			SerializableHelpers.PushSerializedListIntoSet(serializedList, this, false);

			// Indicate we're done serializing
			isSerializing = false;
		}
		#endregion
	}
}
