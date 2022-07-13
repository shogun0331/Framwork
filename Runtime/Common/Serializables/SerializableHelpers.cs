using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GB
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="SerializableHelpers.cs" company="GB">
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
	/// <strong>Date:</strong> 2/6/2022<br/>
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
	/// Adding <c>allowNull</c> arguments to
	/// <see cref="PushSerializedListIntoSet{T}(IList{T}, ISet{T}, bool)"/>
	/// and
	/// <see cref="PushSetIntoSerializedList{T}(ISet{T}, IList{T}, bool)"/>.
	/// </description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// Helper methods for serialization.
	/// </summary>
	public static class SerializableHelpers
	{
		/// <summary>
		/// Pushes the data from <paramref name="baseSet"/>
		/// into <paramref name="listToSerialize"/>.
		/// </summary>
		/// <param name="baseSet">The original data.</param>
		/// <param name="listToSerialize">
		/// The serialized list, used to display elements
		/// in the inspector.
		/// </param>
		/// <param name="allowNull">
		/// If <c>true</c>, checks for <c>null</c> elements in
		/// <paramref name="baseSet"/>.
		/// </param>
		public static void PushSetIntoSerializedList<T>(ISet<T> baseSet, IList<T> listToSerialize, bool allowNull)
		{
			// Remove entries from the serialized list that are not in the set
			T element;
			for (int i = 0; i < listToSerialize.Count; ++i)
			{
				element = listToSerialize[i];
				if (Allow(element, allowNull) && (baseSet.Contains(element) == false))
				{
					listToSerialize.RemoveAt(i);
					--i;
				}
			}

			// Populate the list with new entries
			var serializedSet = new HashSet<T>(listToSerialize);
			foreach (var item in baseSet)
			{
				if (serializedSet.Contains(item) == false)
				{
					listToSerialize.Add(item);
				}
			}
		}

		/// <summary>
		/// Pushes the data from <paramref name="serializedList"/>
		/// into <paramref name="setToSync"/>.
		/// </summary>
		/// <param name="setToSync">The original data.</param>
		/// <param name="serializedList">
		/// The serialized list, used to display elements
		/// in the inspector.
		/// </param>
		/// <param name="allowNull">
		/// If <c>true</c>, let's <c>null</c> be added into
		/// <paramref name="setToSync"/>.
		/// </param>
		public static void PushSerializedListIntoSet<T>(IList<T> serializedList, ISet<T> setToSync, bool allowNull)
		{
			// Clear this HashSet's contents
			setToSync.Clear();

			// Populate this HashSet
			foreach (T item in serializedList)
			{
				if (Allow(item, allowNull))
				{
					setToSync.Add(item);
				}
			}
		}

		static bool Allow<T>(T item, bool allowNull) => (allowNull || item != null);
	}
}
