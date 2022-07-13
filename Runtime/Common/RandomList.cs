using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GB
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="RandomList.cs" company="GB">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2014-2020 GB
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
	/// <strong>Date:</strong> 8/18/2015<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>Initial version.</description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 0.1.0-preview.1<br/>
	/// <strong>Date:</strong> 3/25/2020<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Converted the class to a package.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 0.1.2-preview.1<br/>
	/// <strong>Date:</strong> 4/5/2020<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Updating to be serializable...albeit, in Unity 2020.1.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 0.1.4-preview.1<br/>
	/// <strong>Date:</strong> 5/27/2020<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Upgrading documentation to be DocFX compatible.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 1.2.1<br/>
	/// <strong>Date:</strong> 2/18/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Updating <see cref="CurrentElement"/> to always flag shuffle
	/// if list <see cref="Count"/> is 1 or less.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 1.2.2<br/>
	/// <strong>Date:</strong> 2/18/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Using <see cref="nameof()"/> for throwing argument exceptions.
	/// </description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// A list that shuffles its elements.  Common example:
	/// <code>
	/// int[] allNumbers = new int[] { 1, 2, 3, 4 };
	/// RandomList&lt;int&gt; shuffledNumbers = new RandomList&lt;int&gt;(allNumbers);
	/// for(int i = 0; i &lt; allNumbers.Length; ++i)
	/// {
	///     Debug.Log(shuffledNumbers.NextRandomElement);
	/// }
	/// </code>
	/// </summary>
	[System.Serializable]
	public class RandomList<T> : ICollection<T>
	{
		/// <summary>
		/// Indicates the frequency an element is going to be added into the index list,
		/// The higher the frequency, the more often an element appears.
		/// </summary>
		[System.Serializable]
		public struct ElementFrequency
		{
			[SerializeField]
			[Tooltip("An element in the RandomList.")]
			T element;
			[SerializeField]
			[Tooltip("Number of times the element is shuffled into the RandomList.")]
			int frequency;

			/// <summary>
			/// The element this struct is representing.
			/// </summary>
			public T Element
			{
				get => element;
				set => element = value;
			}

			/// <summary>
			/// The number of times this element appears in the shuffled index list.
			/// This value is never below 1
			/// </summary>
			public int Frequency
			{
				get
				{
					// If somehow the frequency is below 1, force frequency to 1.
					// Basically safe-guareding from Unity's serializion.
					if (frequency < 1)
					{
						frequency = 1;
					}
					return frequency;
				}
				set
				{
					// Prevent ferquency from going below zero
					frequency = value;
					if (frequency < 1)
					{
						frequency = 1;
					}
				}
			}

			public ElementFrequency(T element, int frequency = 1)
			{
				this.element = element;
				this.frequency = frequency;

				// Force frequency to be floored to 1
				if (this.frequency < 1)
				{
					this.frequency = 1;
				}
			}

			public override int GetHashCode()
			{
				return Element.GetHashCode() ^ Frequency.GetHashCode();
			}

			/// <summary>
			/// Checks the type of argument.
			/// If it's another <see cref="ElementFrequency"/>, compares both
			/// <see cref="Element"/> and <see cref="Frequency"/>.
			/// If it's <typeparamref name="T"/>, compares if
			/// <see cref="Element"/> matches with the argument.
			/// </summary>
			/// <param name="obj">The object to compare to.</param>
			/// <returns>
			/// If it's another <see cref="ElementFrequency"/>, returns true
			/// if both <see cref="Element"/> and <see cref="Frequency"/> matches.
			/// If it's <typeparamref name="T"/>, returns true
			/// if <see cref="Element"/> matches with the argument.
			/// Otherwise, false.
			/// </returns>
			public override bool Equals(object obj)
			{
				if (obj is ElementFrequency other)
				{
					return (other.Frequency == this.Frequency) && (Comparer<T>.Default.Compare(other.Element, this.Element) == 0);
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// The serialized list of elements.
		/// </summary>
		/// <remarks>
		/// To affirm this does get set by Unity, left as normal, non-read-only variable.
		/// That said, only the constructors actually touches this pointer.
		/// </remarks>
		[SerializeField]
		List<ElementFrequency> elementsList;

		#region Non-Serialized Member Variables
		/// <summary>
		/// An index within <see cref="ShuffledIndexes"/>.
		/// If it's *not* within, <see cref="ShuffledIndexes"/>,
		/// the next time <see cref="CurrentElement"/> is called,
		/// it'll shuffle <see cref="ShuffledIndexes"/>.
		/// </summary>
		int index = int.MinValue;
		/// <summary>
		/// IMPORTANT: do NOT access this member variable directly;
		/// use <see cref="ElementToIndexMap"/> property, instead.
		/// Dictionary mapping an element to the index in <see cref="elementsList"/>.
		/// Lazy-loaded by the property, <see cref="ElementToIndexMap"/> to
		/// support Unity serialization.
		/// </summary>
		/// <seealso cref="ElementToIndexMap"/>
		/// <seealso cref="SyncAllMapsAndLists"/>
		Dictionary<T, int> elementToIndexMap = null;
		/// <summary>
		/// IMPORTANT: do NOT access this member variable directly;
		/// use <see cref="ShuffledIndexes"/> property, instead.
		/// Contains a list of whole numbers corresponding to an index
		/// in <see cref="elementsList"/>, shuffled. Note that the <see cref="ElementFrequency.Frequency"/>
		/// will affect the number of times an index appears in this list.
		/// Lazy-loaded by the property, <see cref="ShuffledIndexes"/> to
		/// support Unity serialization.
		/// </summary>
		/// <seealso cref="ShuffledIndexes"/>
		/// <seealso cref="SyncAllMapsAndLists"/>
		List<int> shuffledIndexes = null;
		/// <summary>
		/// <see cref="ElementToIndexMap"/>'s comparer,
		/// when it finally gets constructed from lazy-loading.
		/// </summary>
		/// <seealso cref="SyncAllMapsAndLists"/>
		readonly IEqualityComparer<T> elementComparer = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates an empty list.
		/// </summary>
		public RandomList()
		{
			// Setup member variables
			elementsList = new List<ElementFrequency>();
		}

		/// <summary>
		/// Creates an empty list, utilizing a comparer
		/// to detect overlap when running <see cref="Add(T, int)"/> and <see cref="Remove(T, int)"/>.
		/// </summary>
		/// <param name="comparer">The comparer used to detect if an element already exists in this list.</param>
		public RandomList(IEqualityComparer<T> comparer) : this()
		{
			// Setup member variables
			elementComparer = comparer;
		}

		/// <summary>
		/// Creates an empty list, starting with the defined capacity and utilizing a comparer
		/// to detect overlap when running <see cref="Add(T, int)"/> and <see cref="Remove(T, int)"/>.
		/// </summary>
		/// <param name="initialCapacity">Initial size of this list</param>
		/// <param name="comparer">The comparer used to detect if an element already exists in this list.</param>
		public RandomList(int initialCapacity, IEqualityComparer<T> comparer)
		{
			// Setup member variables
			elementsList = new List<ElementFrequency>(initialCapacity);
			elementComparer = comparer;
		}

		/// <summary>
		/// Creates an empty list, starting with the defined capacity.
		/// </summary>
		/// <param name="initialCapacity">Initial size of this list</param>
		public RandomList(int initialCapacity) : this(initialCapacity, null) { }

		/// <summary>
		/// Copies the elements of the list,
		/// each with equal frequency of appearance,
		/// into a new <see cref="RandomList{T}"/>.
		/// </summary>
		/// <param name="list">List of elements to copy over into this list.</param>
		public RandomList(IList<T> list) : this(list.Count)
		{
			// Populate list
			for (int index = 0; index < list.Count; ++index)
			{
				Add(list[index]);
			}
		}

		/// <summary>
		/// Copies the elements of a list into
		/// a new <see cref="RandomList{T}"/>.
		/// </summary>
		/// <param name="list">List of elements to copy over into this list.</param>
		public RandomList(IList<ElementFrequency> list) : this(list.Count)
		{
			// Populate list
			for (int index = 0; index < list.Count; ++index)
			{
				Add(list[index].Element, list[index].Frequency);
			}
		}

		/// <summary>
		/// Copies the elements of the list,
		/// each with equal frequency of appearance,
		/// into a new <see cref="RandomList{T}"/>.
		/// </summary>
		/// <param name="list">List of elements to copy over into this list.</param>
		/// <param name="comparer">The comparer used to detect if an element already exists in this list.</param>
		public RandomList(IList<T> list, IEqualityComparer<T> comparer) : this(list.Count, comparer)
		{
			// Populate list
			for (int index = 0; index < list.Count; ++index)
			{
				Add(list[index]);
			}
		}

		/// <summary>
		/// Copies the elements of a list into
		/// a new <see cref="RandomList{T}"/>.
		/// </summary>
		/// <param name="list">List of elements to copy over into this list.</param>
		/// <param name="comparer">The comparer used to detect if an element already exists in this list.</param>
		public RandomList(IList<ElementFrequency> list, IEqualityComparer<T> comparer) : this(list.Count, comparer)
		{
			// Populate list
			for (int index = 0; index < list.Count; ++index)
			{
				Add(list[index].Element, list[index].Frequency);
			}
		}

		/// <summary>
		/// Copies the elements of a dictionary into
		/// a new <see cref="RandomList{T}"/>.
		/// </summary>
		/// <param name="frequencyMap">Map of elements to copy over into this list.</param>
		/// <param name="comparer">The comparer used to detect if an element already exists in this list.</param>
		public RandomList(IDictionary<T, int> frequencyMap, IEqualityComparer<T> comparer) : this(frequencyMap.Count, comparer)
		{
			// Populate list
			foreach (KeyValuePair<T, int> pair in frequencyMap)
			{
				Add(pair.Key, pair.Value);
			}
		}

		/// <summary>
		/// Copies the elements of a dictionary into
		/// a new <see cref="RandomList{T}"/>.
		/// </summary>
		/// <param name="frequencyMap">Map of elements to copy over into this list. Will also copy over its <see cref="Dictionary{TKey, TValue}.Comparer"/>.</param>
		public RandomList(Dictionary<T, int> frequencyMap) : this(frequencyMap, frequencyMap.Comparer)
		{ }
		#endregion

		#region Helper Properties
		/// <summary>
		/// Dictionary mapping an element to the index in <see cref="elementsList"/>.
		/// </summary>
		protected Dictionary<T, int> ElementToIndexMap
		{
			get
			{
				if ((elementToIndexMap == null) || (shuffledIndexes == null))
				{
					SyncAllMapsAndLists();
				}
				return elementToIndexMap;
			}
		}

		/// <summary>
		/// Contains a list of whole numbers corresponding to an index
		/// in <see cref="elementsList"/>, shuffled. Note that the <see cref="ElementFrequency.Frequency"/>
		/// will affect the number of times an index appears in this list.
		/// </summary>
		protected List<int> ShuffledIndexes
		{
			get
			{
				if ((shuffledIndexes == null) || (elementToIndexMap == null))
				{
					SyncAllMapsAndLists();
				}
				return shuffledIndexes;
			}
		}
		#endregion

		/// <summary>
		/// Number of <em>unique</em> elements in this list.
		/// Disregards the <see cref="ElementFrequency.Frequency"/> value.
		/// </summary>
		public int Count => elementsList.Count;

		/// <summary>
		/// Comparer used to check whether the list already contains an item or not.
		/// </summary>
		public IEqualityComparer<T> Comparer => ElementToIndexMap.Comparer;

		/// <summary>
		/// Capacity of this list. This value automatically
		/// increases as more elements are added to this list.
		/// </summary>
		public int Capacity => elementsList.Capacity;

		/// <summary>
		/// Grabs the currently focused element in the list.
		/// </summary>
		/// <remarks>
		/// This method shuffles <see cref="ShuffledIndexes"/>
		/// if <see cref="index"/> is outside of the list's range.
		/// </remarks>
		/// <seealso cref="NextRandomElement"/>
		public T CurrentElement
		{
			get
			{
				// Get the default return variable
				T returnElement = default;

				// Check how many elements are in the list
				if (Count > 1)
				{
					// Check if the index is out of bounds
					if ((index < 0) || (index >= ShuffledIndexes.Count))
					{
						// Shuffle the list
						Helpers.ShuffleList(ShuffledIndexes);

						// Reset the index
						index = 0;
					}

					// Grab the current element
					returnElement = elementsList[ShuffledIndexes[index]].Element;
				}
				else
				{
					if (Count == 1)
					{
						// Grab the only element
						returnElement = elementsList[0].Element;
					}

					// Force index to reset
					Reshuffle();
				}
				return returnElement;
			}
		}

		/// <summary>
		/// Grabs the next random element from the list.
		/// </summary>
		/// <seealso cref="CurrentElement"/>
		public T NextRandomElement
		{
			get
			{
				if (Count > 1)
				{
					++index;
				}
				return CurrentElement;
			}
		}

		/// <summary>
		/// Flags the list to reshuffle next time when
		/// <see cref="CurrentElement"/> or
		/// <see cref="NextRandomElement"/> gets called.
		/// </summary>
		public void Reshuffle()
		{
			index = int.MinValue;
		}

		/// <summary>
		/// Appends an item to the end of the list, paired with a frequency value.
		/// This method does *not* shuffle the list, thus making the item appear at the end of enumeration consistently.
		/// Remember to run <see cref="Reshuffle()"> after this method.
		/// </summary>
		/// <param name="item">Item to add into the list</param>
		/// <param name="numberOfItemsToAdd">Number of times to add this item to the list.</param>
		public virtual void Add(T item, int numberOfItemsToAdd)
		{
			// Make sure the frequency is a number larger than zero
			if (numberOfItemsToAdd <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(numberOfItemsToAdd));
			}

			// Check if the item is already in the list
			int itemIndex = IndexOf(item);
			if (itemIndex >= 0)
			{
				// If so, just increment the frequency of the item
				IncrementFrequency(itemIndex, numberOfItemsToAdd);
			}
			else
			{
				// If not, create a new struct
				ElementFrequency element = new ElementFrequency(item, numberOfItemsToAdd);

				// Set the item index
				itemIndex = elementsList.Count;

				// Add the item to both the list and the map
				elementsList.Add(element);
				elementToIndexMap.Add(item, itemIndex);
			}

			// Populate the ShuffledIndexes with the newly added index
			for (int numAdded = 0; numAdded < numberOfItemsToAdd; ++numAdded)
			{
				ShuffledIndexes.Add(itemIndex);
			}
		}

		/// <summary>
		/// Removes the first instance of the item from the list.
		/// This method does *not* shuffle the list: remember to run
		/// <see cref="Reshuffle()"> after this method.
		/// </summary>
		/// <param name="item">Item to remove from the list</param>
		/// <param name="numberOfItemsToRemove">Number of times to remove from the item from the list.</param>
		/// <return>Actual number of items removed from the list.</return>
		public virtual int Remove(T item, int numberOfItemsToRemove)
		{
			// Make sure the frequency is a number larger than zero
			if (numberOfItemsToRemove <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(numberOfItemsToRemove));
			}

			// Setup default return value
			int returnNumRemoved = 0;

			// Check if the item is already in the list
			int removeIndex = IndexOf(item);
			if (removeIndex >= 0)
			{
				// If so, get the frequency in the list
				returnNumRemoved = elementsList[removeIndex].Frequency;

				// Check if this value exceeds the number of elements we're removing
				if (returnNumRemoved > numberOfItemsToRemove)
				{
					// If not, decrement the frequency.
					// Return the difference between old frequency, and the new one
					returnNumRemoved -= IncrementFrequency(removeIndex, -numberOfItemsToRemove);
				}
				else
				{
					// If not, remove the item from all member variables
					// Remove the element from the list
					elementsList.RemoveAt(removeIndex);

					// Remove the element from the map
					elementToIndexMap.Remove(item);

					// Decrement the mapped index values
					for (int index = removeIndex; index < elementsList.Count; ++index)
					{
						elementToIndexMap[elementsList[index].Element] -= 1;
					}
				}
				RemoveFromIndexList(removeIndex, returnNumRemoved);
			}
			return returnNumRemoved;
		}

		/// <summary>
		/// Removes all instances of an item from the list.
		/// </summary>
		/// <param name="item">The item to remove from the list.</param>
		/// <returns>Number of instanes of item removed.</returns>
		public int RemoveAllOf(T item)
		{
			// Setup default return value
			int returnFrequency = GetFrequency(item);

			// Check if there are any items to remove
			if (returnFrequency > 0)
			{
				// Run the remove operation, taking out all instances
				returnFrequency = Remove(item, returnFrequency);
			}
			return returnFrequency;
		}

		/// <summary>
		/// Gets the number of instances an item appears in this list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>Number of instances an item appears in this list.</returns>
		public int GetFrequency(T item)
		{
			// Setup default return value
			int returnFrequency = 0;

			// Check if the item is already in the list
			int removeIndex = IndexOf(item);
			if (removeIndex >= 0)
			{
				// If so, get the frequency in the list
				returnFrequency = elementsList[removeIndex].Frequency;
			}
			return returnFrequency;
		}

		#region Interface Implementation
		/// <summary>
		/// Always returns false.
		/// </summary>
		/// <returns>false</returns>
		public bool IsReadOnly => false;

		/// <summary>
		/// Enumerates through all items, in order of appended elements.
		/// </summary>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			foreach (ElementFrequency item in elementsList)
			{
				yield return item.Element;
			}
		}

		/// <summary>
		/// Enumerates through all items, in order of appended elements.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (ElementFrequency item in elementsList)
			{
				yield return item.Element;
			}
		}

		/// <summary>
		/// Appends an item to the end of the list.
		/// This method does *not* shuffle the list, thus making the item appear at the end of enumeration consistently.
		/// Remember to run <see cref="Reshuffle()"> after this method.
		/// </summary>
		/// <param name="item">A single item to add into the list</param>
		public void Add(T item)
		{
			Add(item, 1);
		}

		/// <inheritdoc/>
		public void Clear()
		{
			elementsList.Clear();
			ElementToIndexMap.Clear();
			ShuffledIndexes.Clear();
			Reshuffle();
		}

		/// <inheritdoc/>
		public bool Contains(T item)
		{
			return ElementToIndexMap.ContainsKey(item);
		}

		/// <inheritdoc/>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException(nameof(array));
			}
			else if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			}
			else if (array.Rank > 1)
			{
				throw new ArgumentException("array isn't one-dimensional");
			}
			else if (array.Length < (elementsList.Count + arrayIndex))
			{
				throw new ArgumentException("array is too small to copy to");
			}

			for (int offsetIndex = 0; offsetIndex < elementsList.Count; ++offsetIndex)
			{
				array[arrayIndex + offsetIndex] = elementsList[offsetIndex].Element;
			}
		}

		/// <inheritdoc/>
		public void CopyTo(ElementFrequency[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException(nameof(array));
			}
			else if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			}
			else if (array.Rank > 1)
			{
				throw new ArgumentException("array isn't one-dimensional");
			}
			else if (array.Length < (elementsList.Count + arrayIndex))
			{
				throw new ArgumentException("array is too small to copy to");
			}

			for (int offsetIndex = 0; offsetIndex < elementsList.Count; ++offsetIndex)
			{
				array[arrayIndex + offsetIndex] = elementsList[offsetIndex];
			}
		}

		/// <summary>
		/// Removes the first instance of the item from the list.
		/// This method does *not* shuffle the list: remember to run
		/// <see cref="Reshuffle()"> after this method.
		/// </summary>
		public bool Remove(T item)
		{
			return Remove(item, 1) > 0;
		}
		#endregion

		#region Helper Methods
		/// <summary>
		/// Gets index of the first instance of item from <see cref="elementsList"/>.
		/// </summary>
		/// <param name="item">Element in <see cref="elementsList"/> to search for.</param>
		/// <returns>Index of item in  <see cref="elementsList"/>, or -1 if not found.</returns>
		protected int IndexOf(T item)
		{
			int returnIndex;
			if (ElementToIndexMap.TryGetValue(item, out returnIndex) == false)
			{
				returnIndex = -1;
			}
			return returnIndex;
		}

		/// <summary>
		/// Syncs the information in <see cref="elementsList"/>
		/// to <see cref="ElementToIndexMap"/> and <see cref="ShuffledIndexes"/>.
		/// As the name of the method implies, this is potentially a costly operation;
		/// use it sparringly!
		/// </summary>
		protected virtual void SyncAllMapsAndLists()
		{
			// IMPORTANT: this is the only method that can (and must, to avoid stack overflow)
			// access member variables shuffledIndexes and elementToIndexMap directly.

			// Because elementList is serialized, it may be already pre-populated.
			// This method will sync.

			// Check if shuffledIndexes is already created
			if (shuffledIndexes != null)
			{
				// Empty shuffledIndexes
				shuffledIndexes.Clear();
			}
			else
			{
				// Initialize an empty shuffledIndexes
				shuffledIndexes = new List<int>(elementsList.Capacity);
			}

			// Check if elementToIndexMap is already created
			if (elementToIndexMap != null)
			{
				// Empty elementToIndexMap
				elementToIndexMap.Clear();
			}
			else if (elementComparer != null)
			{
				// Initialize an empty shuffledIndexes, using the constructor's comparer
				elementToIndexMap = new Dictionary<T, int>(elementsList.Capacity, elementComparer);
			}
			else
			{
				// Initialize an empty shuffledIndexes
				elementToIndexMap = new Dictionary<T, int>(elementsList.Capacity);
			}

			// Setup loop variables
			index = 0;
			int updatedIndex;
			ElementFrequency currentItem;

			// Go through all the elements in the elementList
			while (index < elementsList.Count)
			{
				// Grab the element from the list
				currentItem = elementsList[index];

				// Check if, due to user error, this item is duplicated or not
				if (elementToIndexMap.TryGetValue(currentItem.Element, out updatedIndex) == false)
				{
					// If not (which is most cases), add a mapping from element to index
					elementToIndexMap.Add(currentItem.Element, index);

					// Flag that the current index is the one we want to add to shuffledIndexes
					updatedIndex = index;

					// In the next loop, move on to the next elementsList item
					++index;
				}
				else
				{
					// If so, increment the existing element
					IncrementFrequency(updatedIndex, currentItem.Frequency);

					// Then remove the element from the list
					elementsList.RemoveAt(index);
				}

				// Add updatedIndex by item.Frequency times
				for (int numAdded = 0; numAdded < currentItem.Frequency; ++numAdded)
				{
					shuffledIndexes.Add(updatedIndex);
				}
			}

			// Flag the list for re-shuffling
			Reshuffle();
		}

		/// <summary>
		/// Removes all instances of a value from <see cref="ShuffledIndexes"/>,
		/// and decrements any other value greater than removeIndex.
		/// </summary>
		/// <param name="removeIndex">The value to remove from  <see cref="ShuffledIndexes"/>.</param>
		protected virtual void RemoveFromIndexList(int removeIndex, int numberOfItemsToRemove)
		{
			// Shift every index in the indexes list
			int checkIndex = 0, numRemovedSoFar = 0;
			while ((checkIndex < ShuffledIndexes.Count) && (numRemovedSoFar < numberOfItemsToRemove))
			{
				// Compare indexes
				// Note: doing less-than and greater-than comparisons first,
				// as they're more likely to occur, and it's (slightly) more
				// efficient to hit the earlier conditionals first.
				if (ShuffledIndexes[checkIndex] < removeIndex)
				{
					// If less, skip to the next index
					++checkIndex;
				}
				else if (ShuffledIndexes[checkIndex] > removeIndex)
				{
					// If greater, shift this index down one
					ShuffledIndexes[checkIndex] -= 1;

					// Skip to the next index
					++checkIndex;
				}
				else
				{
					// Remove this index
					ShuffledIndexes.RemoveAt(checkIndex);

					// Increment the number of items removed so far
					++numRemovedSoFar;

					// Don't change checkIndex; the line above will shift
					// all elements by one, so we don't want to miss
					// the next element.
				}
			}
		}

		/// <summary>
		/// Increments the frequency in an element in <see cref="elementsList"/>.
		/// Accepts negative frequency values (which, of course, causes the method to
		/// decrement the item's frequency).
		/// </summary>
		/// <param name="itemIndex">Index of the element to increment frequency of.</param>
		/// <param name="frequency">The change in frequency.</param>
		/// <returns>The sum of the old and new frequency values.</returns>
		protected int IncrementFrequency(int itemIndex, int frequency)
		{
			ElementFrequency existingItem = elementsList[itemIndex];
			existingItem.Frequency += frequency;
			elementsList[itemIndex] = existingItem;
			return existingItem.Frequency;
		}
		#endregion
	}
}
