using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GB
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="UndoHistory.cs" company="GB">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2021 GB
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
	/// <strong>Version:</strong> 1.1.0<br/>
	/// <strong>Date:</strong> 11/27/2021<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>Initial version.</description>
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
	/// Records a list of previous actions, and allows user to undo or redo
	/// them.
	/// </summary>
	public class UndoHistory : IReadOnlyCollection<UndoHistory.IRecord>
	{
		/// <summary>
		/// A record stored in <seealso cref="UndoHistory"/>.
		/// </summary>
		public interface IRecord
		{
			/// <summary>
			/// Description of this action.
			/// </summary>
			public string Description
			{
				get;
			}

			/// <summary>
			/// Called by <seealso cref="UndoHistory.Undo(Object)"/>.
			/// </summary>
			/// <param name="source">The caller of this method.</param>
			/// <param name="history">The history this record is in.</param>
			public void OnUndo(object source, UndoHistory history);
			/// <summary>
			/// Called by <seealso cref="UndoHistory.Redo(Object)"/>.
			/// </summary>
			/// <param name="source">The caller of this method.</param>
			/// <param name="history">The history this record is in.</param>
			public void OnRedo(object source, UndoHistory history);
		}

		public const int DefaultCapacity = int.MaxValue;

		LinkedListNode<IRecord> undoMarker = null;

		/// <summary>
		/// TODO
		/// </summary>
		public event Action<UndoHistory> OnBeforeChanged;
		/// <summary>
		/// TODO
		/// </summary>
		public event Action<UndoHistory> OnAfterChanged;
		/// <summary>
		/// TODO
		/// </summary>
		public event Action<object, UndoHistory> OnBeforeUndo;
		/// <summary>
		/// TODO
		/// </summary>
		public event Action<object, UndoHistory> OnAfterUndo;
		/// <summary>
		/// TODO
		/// </summary>
		public event Action<object, UndoHistory> OnBeforeRedo;
		/// <summary>
		/// TODO
		/// </summary>
		public event Action<object, UndoHistory> OnAfterRedo;

		/// <summary>
		/// Constructs an Undo history.
		/// </summary>
		/// <param name="capacity">How many records this list stores.  Defaults to <see cref="int.MaxValue"/></param>
		/// <exception cref="ArgumentException">If <paramref name="capacity"/> is less than 1.</exception>
		public UndoHistory(int capacity = DefaultCapacity)
		{
			if (capacity < 1)
			{
				throw new ArgumentException("Capacity is below one.", nameof(capacity));
			}
			Capacity = capacity;
		}

		/// <summary>
		/// History of actions, up to <see cref="Capacity"/>.
		/// First node is oldest, while last is latest.
		/// </summary>
		protected LinkedList<IRecord> History
		{
			get;
		} = new LinkedList<IRecord>();
		/// <summary>
		/// The max number of actions stored.
		/// </summary>
		public int Capacity
		{
			get;
		}
		/// <summary>
		/// Actual number of actions stored.
		/// </summary>
		public int Count => History.Count;
		/// <summary>
		/// The node in <see cref="History"/> that <seealso cref="Undo(Object)"/> will call.
		/// </summary>
		protected virtual LinkedListNode<IRecord> UndoMarker
		{
			get => undoMarker;
			set => undoMarker = value;
		}
		/// <summary>
		/// The node in <see cref="History"/> that <seealso cref="Redo(Object)"/> will call.
		/// </summary>
		protected LinkedListNode<IRecord> RedoMarker
		{
			get
			{
				if (UndoMarker != null)
				{
					return UndoMarker.Next;
				}
				else
				{
					return History.First;
				}
			}
		}
		/// <summary>
		/// The record that would be called by <seealso cref="Undo(Object)"/>, or <c>null</c> if can't undo.
		/// </summary>
		public IRecord UndoRecord => UndoMarker?.Value;
		/// <summary>
		/// Provides the description of what will be redone, or <c>null</c> if can't redo.
		/// </summary>
		public IRecord RedoRecord => RedoMarker?.Value;
		/// <summary>
		/// Indicates whether there's an action to undo.
		/// </summary>
		public bool CanUndo => (UndoMarker != null);
		/// <summary>
		/// Indicates whether there's an action to redo.
		/// </summary>
		public bool CanRedo => (RedoMarker != null);

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="record"></param>
		/// <exception cref="ArgumentNullException">If <paramref name="record"/> is <c>null</c>.</exception>
		public virtual void Add(IRecord record)
		{
			if (record == null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			// Call before events
			OnBeforeChanged?.Invoke(this);

			// Check if there are any action after the marker (from a result of undo)
			if (UndoMarker != null)
			{
				// If so, remove all recent actions after the latest
				while (UndoMarker.Next != null)
				{
					History.RemoveLast();
				}
			}
			else if (History.Count > 0)
			{
				// If at the last undo, clear the entire history
				History.Clear();
			}

			// Add a new action at the end of history
			UndoMarker = History.AddLast(record);

			// Check if the history is over-capacity
			while (History.Count > Capacity)
			{
				// Remove the earliest actions
				History.RemoveFirst();
			}

			// Call after events
			OnAfterChanged?.Invoke(this);
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="source"></param>
		public virtual bool Undo(object source)
		{
			// Check if we can even undo
			if (UndoMarker != null)
			{
				// Call before events
				OnBeforeChanged?.Invoke(this);
				OnBeforeUndo?.Invoke(source, this);

				// Perform undo
				UndoMarker.Value.OnUndo(source, this);

				// Move the marker to the previous history entry
				UndoMarker = UndoMarker.Previous;

				// Call after events
				OnAfterUndo?.Invoke(source, this);
				OnAfterChanged?.Invoke(this);
				return true;
			}
			return false;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="source"></param>
		public virtual bool Redo(object source)
		{
			// Check if we can even redo
			LinkedListNode<IRecord> newMarker = RedoMarker;
			if (newMarker != null)
			{
				// Call before events
				OnBeforeChanged?.Invoke(this);
				OnBeforeRedo?.Invoke(source, this);

				// Perform redo
				newMarker.Value.OnRedo(source, this);

				// Move the marker to the next history entry
				UndoMarker = newMarker;

				// Call after events
				OnAfterRedo?.Invoke(source, this);
				OnAfterChanged?.Invoke(this);
				return true;
			}
			return false;
		}

		/// <summary>
		/// TODO
		/// </summary>
		public virtual void Clear()
		{
			// Call before events
			OnBeforeChanged?.Invoke(this);

			// Reset records
			UndoMarker = null;
			History.Clear();

			// Call after events
			OnAfterChanged?.Invoke(this);
		}

		/// <inheritdoc/>
		public IEnumerator<IRecord> GetEnumerator()
		{
			return ((IEnumerable<IRecord>)History).GetEnumerator();
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)History).GetEnumerator();
		}
	}
}
