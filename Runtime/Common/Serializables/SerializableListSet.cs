using System;
using System.Collections.Generic;
using UnityEngine;

namespace GB
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="SerializableListSet.cs" company="GB">
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
	/// A serializable <seealso cref="ListSet{T}"/>. Expose it on the inspector
	/// like a normal list.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class SerializableListSet<T> : ListSet<T>, ISerializationCallbackReceiver
	{
		[SerializeField]
		List<T> serializedList;
		[SerializeField, HideInInspector]
		bool isSerializing = false;

		/// <summary>
		/// Default constructor that sets up an empty list.
		/// </summary>
		public SerializableListSet()
		{
			serializedList = new List<T>();
		}

		/// <summary>
		/// Constructor an empty list with initial capacity defined.
		/// </summary>
		/// <param name="capacity">Initial capacity of this list.</param>
		public SerializableListSet(int capacity) : base(capacity)
		{
			serializedList = new List<T>(capacity);
		}

		/// <summary>
		/// Constructor to set the <see cref="IEqualityComparer{T}"/>,
		/// used to check if two elements matches.
		/// </summary>
		/// <param name="comparer">
		/// Comparer to check if two elements matches.
		/// </param>
		public SerializableListSet(IEqualityComparer<T> comparer) : base(comparer)
		{
			serializedList = new List<T>();
		}

		/// <summary>
		/// Constructor to set the <see cref="IEqualityComparer{T}"/>,
		/// used to check if two elements matches.
		/// </summary>
		/// <param name="capacity">Initial capacity of this list.</param>
		/// <param name="comparer">
		/// Comparer to check if two elements matches.
		/// </param>
		public SerializableListSet(int capacity, IEqualityComparer<T> comparer) : base(capacity, comparer)
		{
			serializedList = new List<T>(capacity);
		}

		/// <summary>
		/// Indicates if this collection is in the middle of serializing.
		/// </summary>
		public bool IsSerializing => isSerializing;

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
	}
}
