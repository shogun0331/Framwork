using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace GB
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="Helpers.cs" company="GB">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2014-2021 GB
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
	/// <description>Initial verison.</description>
	/// </item><item>
	/// <term>
	/// <strong>Date:</strong> 6/4/2018<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Added method for shortening URL.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 0.1.0-preview.1<br/>
	/// <strong>Date:</strong> 3/25/2020<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Converting the file to a package.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 0.1.4-preview.1<br/>
	/// <strong>Date:</strong> 5/25/2020<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Updating documentation.  Moving method <see cref="ShortenUrl(string)"/> to GB - Web package.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 1.1.0<br/>
	/// <strong>Date:</strong> 6/28/2021<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Adding a delegate to monitor changing values.
	/// </description>
	/// </item><item>
	/// <term>
	/// <strong>Version:</strong> 1.3.0<br/>
	/// <strong>Date:</strong> 3/13/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Adding <see cref="CloneComponent{T}(T, GameObject)"/> and its aliases.
	/// </description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// A series of utilities used throughout the <see cref="OmiyaGames"/> namespace.
	/// </summary>
	public static class Helpers
	{
		/// <summary>
		/// Flag whether <see cref="Log(string, bool)"/> prints the timestamp.
		/// </summary>
		public const bool IsTimeStampPrintedByDefault = true;
		/// <summary>
		/// Path divider Unity normalizes to.
		/// </summary>
		public const char PathDivider = '/';
		/// <summary>
		/// Distance between 2 UI elements before the animated one snaps to position.
		/// </summary>
		public const float SnapToThreshold = 0.01f;
		/// <summary>
		/// File extension for <see cref="ScriptableObjects"/> files.
		/// </summary>
		public const string FileExtensionScriptableObject = ".asset";
		/// <summary>
		/// File extension for text files.
		/// </summary>
		public const string FileExtensionText = ".txt";
		/// <summary>
		/// Timestamp format printed in <see cref="Log(string, bool)"/>.
		/// </summary>
		public const string TimeStampPrint = "HH:mm:ss.ffff GMTzz";
		/// <summary>
		/// Set of invalid folder chars: "/, \, :, *, ?, ", <, >, and |."
		/// </summary>
		public static readonly ISet<char> InvalidFileNameCharactersSet = new HashSet<char>()
		{
			'\\',
			'/',
			':',
			'*',
			'?',
			'"',
			'<',
			'>',
			'|'
		};
		/// <summary>
		/// Delegate for tracking changes to a single value.
		/// </summary>
		/// <typeparam name="SOURCE">Type of the object being changed.</typeparam>
		/// <typeparam name="VALUE">Type of the object's member variable being changed.</typeparam>
		/// <param name="eventSource">The object's member variable being changed.</param>
		/// <param name="oldValue">The old member variable's value.</param>
		/// <param name="newValue">The new value the member variable is going to be set to.</param>
		public delegate void ChangeEvent<SOURCE, VALUE>(SOURCE eventSource, VALUE oldValue, VALUE newValue);

		/// <summary>
		/// Creates a clone of the components <see cref="GameObject"/>, places it under
		/// the same parent on the hierarchy, and finally returns the copy of a component
		/// attached to that clone.
		/// </summary>
		/// <typeparam name="T">Component attached to a <see cref="GameObject"/></typeparam>
		/// <param name="copyFrom">The component to grab its <see cref="GameObject"/>.
		/// This will be used  to clone a new <see cref="GameObject"/>.</param>
		/// <param name="setActive">Whether the clone is active or not</param>
		/// <param name="copyPosition">Whether the clone will be at the same position
		/// as the original or not</param>
		/// <param name="copyRotation">Whether the clone will have the same rotation
		/// as the original or not</param>
		/// <param name="copyScale">Whether the clone will be scaled the same as the
		/// original or not</param>
		/// <returns>A component attached to the new clone</returns>
		public static T Replicate<T>(T copyFrom, bool setActive = true) where T : Component
		{
			// Create a clone
			GameObject clone = Replicate(copyFrom.gameObject, setActive);

			// Grab its component
			return clone.GetComponent<T>();
		}

		/// <summary>
		/// Creates a clone of the provided <see cref="GameObject"/> and places it under
		/// the assigned transform on the hierarchy.
		/// </summary>
		/// <typeparam name="T">Component attached to a <see cref="GameObject"/></typeparam>
		/// <param name="copyFrom">The component to grab its <see cref="GameObject"/>.
		/// <param name="attachTo">The <see cref="Transform"/> to make the clone a child of.
		/// <c>null</c> will place the clone at the hierarchy's root.</param>
		/// <param name="setActive">Whether the clone is active or not</param>
		/// <param name="copyLocalPosition">Whether the clone will be at the same position
		/// as the original or not</param>
		/// <param name="copyLocalRotation">Whether the clone will have the same rotation
		/// as the original or not</param>
		/// <param name="copyLocalScale">Whether the clone will be scaled the same as the
		/// original or not</param>
		/// <returns>A component attached to the new clone</returns>
		public static T Replicate<T>(T copyFrom, Transform attachTo, bool setActive = true, bool copyLocalPosition = true, bool copyLocalRotation = true, bool copyLocalScale = true) where T : Component
		{
			// Create a clone
			GameObject clone = Replicate(copyFrom.gameObject, copyFrom.transform.parent, setActive, copyLocalPosition, copyLocalRotation, copyLocalScale);

			// Grab its component
			return clone.GetComponent<T>();
		}

		/// <summary>
		/// Creates a clone of the provided <see cref="GameObject"/> and places it under
		/// the same parent on the hierarchy.
		/// </summary>
		/// <param name="copyFrom">The <see cref="GameObject"/> to clone off of.</param>
		/// <param name="setActive">Whether the clone is active or not</param>
		/// <returns>A clone of <see cref="GameObject"/></returns>
		public static GameObject Replicate(GameObject copyFrom, bool setActive = true)
		{
			return Replicate(copyFrom, copyFrom.transform.parent, setActive, true, true, true);
		}

		/// <summary>
		/// Creates a clone of the provided <see cref="GameObject"/> and places it under
		/// the assigned transform on the hierarchy.
		/// </summary>
		/// <param name="copyFrom">The <see cref="GameObject"/> to clone off of.</param>
		/// <param name="attachTo">The <see cref="Transform"/> to make the clone a child of.
		/// <c>null</c> will place the clone at the hierarchy's root.</param>
		/// <param name="setActive">Whether the clone is active or not</param>
		/// <param name="copyLocalPosition">Whether the clone will be at the same position
		/// as the original or not</param>
		/// <param name="copyLocalRotation">Whether the clone will have the same rotation
		/// as the original or not</param>
		/// <param name="copyLocalScale">Whether the clone will be scaled the same as the
		/// original or not</param>
		/// <returns>A clone of <see cref="GameObject"/></returns>
		public static GameObject Replicate(GameObject copyFrom, Transform attachTo, bool setActive = true, bool copyLocalPosition = true, bool copyLocalRotation = true, bool copyLocalScale = true)
		{
			// Create a clone
			GameObject clone = Object.Instantiate(copyFrom);

			// Setup its transform
			clone.transform.SetParent(attachTo, true);
			clone.transform.SetAsLastSibling();

			// Setup it's dimensions
			clone.SetActive(setActive);
			if (copyLocalPosition == true)
			{
				clone.transform.localPosition = copyFrom.transform.localPosition;
			}
			if (copyLocalRotation == true)
			{
				clone.transform.localRotation = copyFrom.transform.localRotation;
			}
			if (copyLocalScale == true)
			{
				clone.transform.localScale = copyFrom.transform.localScale;
			}
			return clone;
		}

		/// <summary>
		/// Shuffles the list.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="upTo">Number of elements to shuffle, starting at index 0.
		/// Elements outside of this range maybe be shuffled between this range as well.
		/// If negative, will shuffle all list elements.</param>
		/// <typeparam name="H">The list type parameter.</typeparam>
		public static void ShuffleList<H>(IList<H> list, int upTo = -1)
		{
			// Check if we want to shuffle the entire list
			if ((upTo < 0) || (upTo > list.Count))
			{
				upTo = list.Count;
			}

			// Go through every list element
			H swapObject = default;
			for (int index = 0; index < upTo; ++index)
			{
				// Swap a random element
				int randomIndex = Random.Range(0, list.Count);
				if (index != randomIndex)
				{
					swapObject = list[index];
					list[index] = list[randomIndex];
					list[randomIndex] = swapObject;
				}
			}
		}

		/// <summary>
		/// Remove duplicate entries from a <paramref name="list"/>.
		/// </summary>
		/// <typeparam name="H">The type of List.</typeparam>
		/// <param name="list">List to remove duplicates from.</param>
		/// <param name="comparer">
		/// Comparer to verify whether two elements are the same or not.
		/// </param>
		public static void RemoveDuplicateEntries<H>(IList<H> list, IEqualityComparer<H> comparer = null)
		{
			// Go through every list element
			int focusIndex = 0, compareIndex = 0;
			bool isDuplicate = false;
			for (; focusIndex < list.Count; ++focusIndex)
			{
				// Start the loop with the next element the next element
				for (compareIndex = (focusIndex + 1); compareIndex < list.Count; ++compareIndex)
				{
					// Check if the elements are the same
					if (comparer == null)
					{
						isDuplicate = list[focusIndex].Equals(list[compareIndex]);
					}
					else
					{
						isDuplicate = comparer.Equals(list[focusIndex], list[compareIndex]);
					}

					// Check if this element is a dupicate
					if (isDuplicate == true)
					{
						// If so, remove from the list
						list.RemoveAt(compareIndex);
						--compareIndex;
					}
				}
			}
		}

		/// <summary>
		/// Logs to the console <em>only if</em> DEBUG macro is turned on.
		/// </summary>
		/// <param name="message">Message to print.</param>
		/// <param name="showTimestamp">Timestampe format.</param>
		public static void Log(string message, bool showTimestamp = IsTimeStampPrintedByDefault)
		{
#if DEBUG
			// Only do something if we're in debug mode
			if (showTimestamp == true)
			{
				message = '<' + System.DateTime.Now.ToString(TimeStampPrint) + "> " + message;
			}
			Debug.Log(message);
#endif
		}

		/// <summary>
		/// Loops up the transform's parents, seeking for an instance of <see cref="Canvas"/>.
		/// </summary>
		/// <param name="checkTransform">
		/// <see cref="Transform"/> to search for a <see cref="Canvas"/>.
		/// </param>
		/// <returns>
		/// <see cref="Canvas"/> on <paramref name="checkTransform"/>,
		/// its parent transforms, or null if none was found.
		/// </returns>
		public static Canvas GetParentCanvas(Transform checkTransform)
		{
			// Check if it has a canvas
			Canvas parentCanvas = checkTransform.GetComponent<Canvas>();

			// Loop while canvas isn't set, and there is a parent to be concerned of
			while ((checkTransform != null) && (checkTransform.parent != null) && (parentCanvas == null))
			{
				// Grab the next parent
				checkTransform = checkTransform.parent;

				// Check if parent has a canvas
				parentCanvas = checkTransform.GetComponent<Canvas>();
			}
			return parentCanvas;
		}

		/// <summary>
		/// Converts <see cref="int"/> to an <typeparamref name="ENUM"/>.
		/// More useful for <see cref="UnityEditor.Editor"/>.
		/// </summary>
		/// <typeparam name="ENUM">Enum to convert to.</typeparam>
		/// <param name="value"><see cref="int"/> to convert.</param>
		/// <returns>
		/// <paramref name="value"/> converted to an <typeparamref name="ENUM"/>.
		/// </returns>
		public static ENUM ConvertToEnum<ENUM>(int value) where ENUM : System.Enum
		{
			return (ENUM)System.Enum.ToObject(typeof(ENUM), value);
		}

		/// <summary>
		/// Converts an <typeparamref name="ENUM"/> to <see cref="int"/>.
		/// More useful for <see cref="UnityEditor.Editor"/>.
		/// </summary>
		/// <typeparam name="ENUM">Enum to convert from.</typeparam>
		/// <param name="value"><typeparamref name="ENUM"/> to convert to.</param>
		/// <returns><paramref name="value"/> converted into an <see cref="int"/></returns>
		public static int ConvertToInt<ENUM>(ENUM value) where ENUM : System.Enum
		{
			return System.Convert.ToInt32(value);
		}

		/// <summary>
		/// A slightly more efficient way of setting a Vector3 than assignment.
		/// </summary>
		public static void SetVector(ref Vector3 toSet, ref Vector3 toCopy)
		{
			toSet.x = toCopy.x;
			toSet.y = toCopy.y;
			toSet.z = toCopy.z;
		}

		/// <summary>
		/// A slightly more efficient way of setting a Vector3 than assignment.
		/// </summary>
		public static void SetVector(ref Vector2 toSet, ref Vector2 copy)
		{
			toSet.x = copy.x;
			toSet.y = copy.y;
		}

		/// <summary>
		/// A slightly more efficient way of incrementing a Vector3 than assignment.
		/// </summary>
		public static void IncrementVector(ref Vector3 toSet, ref Vector3 add)
		{
			toSet.x += add.x;
			toSet.y += add.y;
			toSet.z += add.z;
		}

		/// <summary>
		/// A slightly more efficient way of incrementing a Vector3 than assignment.
		/// </summary>
		public static void IncrementVector(ref Vector2 toSet, ref Vector2 add)
		{
			toSet.x = add.x;
			toSet.y = add.y;
		}

		/// <summary>
		/// A slightly more efficient way of decrementing a Vector3 than assignment.
		/// </summary>
		public static void DecrementVector(ref Vector3 toSet, ref Vector3 subtract)
		{
			toSet.x -= subtract.x;
			toSet.y -= subtract.y;
			toSet.z -= subtract.z;
		}

		/// <summary>
		/// A slightly more efficient way of decrementing a Vector3 than assignment.
		/// </summary>
		public static void DecrementVector(ref Vector2 toSet, ref Vector2 subtract)
		{
			toSet.x -= subtract.x;
			toSet.y -= subtract.y;
		}

		/// <summary>
		/// Grabs a component, and sets it to cache, unless the cache isn't null.
		/// </summary>
		public static T GetComponentCached<T>(MonoBehaviour script, ref T cache) where T : Component
		{
			if (cache == null)
			{
				cache = script.GetComponent<T>();
			}
			return cache;
		}

		/// <summary>
		/// Removes any invalid characters for building a file name.
		/// </summary>
		/// <param name="text">Text to remove diacritics from.</param>
		/// <param name="stringBuilder">
		/// An optional <see cref="StringBuilder"/> this method will use to
		/// generate the returned string. Good for performance.
		/// </param>
		/// <returns>
		/// <paramref name="text"/> with invalid file characters removed.
		/// </returns>
		/// <remarks>
		/// Taken from
		/// <a href="http://archives.miloush.net/michkap/archive/2007/05/14/2629747.html">
		/// archives.miloush.net/michkap/archive/2007/05/14/2629747.html
		/// </a>
		/// </remarks>
		public static string RemoveDiacritics(string text, StringBuilder stringBuilder = null)
		{
			// Setup StringBuilder
			if (stringBuilder == null)
			{
				stringBuilder = new StringBuilder(text.Length);
			}
			else
			{
				stringBuilder.Clear();
			}

			// Go through each character in the string.
			string normalizedString = text.Normalize(NormalizationForm.FormD);
			foreach (char c in normalizedString)
			{
				// Check if this character is valid
				UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if ((unicodeCategory != UnicodeCategory.NonSpacingMark) && (InvalidFileNameCharactersSet.Contains(c) == false))
				{
					// If so, append to the String Builder.
					stringBuilder.Append(c);
				}
			}

			return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
		}

		/// <summary>
		/// Destroys an <see cref="Object"/> safely.
		/// </summary>
		/// <remarks>
		/// Code from Unity's Core RenderPipeline package (<c>CoreUtils.Destroy(obj)</c>.)
		/// </remarks>
		/// <param name="obj">Object to be destroyed.</param>
		public static void Destroy(Object obj)
		{
			if (obj != null)
			{
#if UNITY_EDITOR
				if ((Application.isPlaying == true) && (UnityEditor.EditorApplication.isPaused == false))
				{
					Object.Destroy(obj);
				}
				else
				{
					Object.DestroyImmediate(obj);
				}
#else
				Object.Destroy(obj);
#endif
			}
		}

		/// <summary>
		/// Clones a component onto the same <see cref="GameObject"/>
		/// <paramref name="original"/> is attached to.
		/// </summary>
		/// <typeparam name="T">
		/// Component type.
		/// </typeparam>
		/// <param name="original">
		/// The original component to clone.
		/// </param>
		/// <returns>
		/// The new component with fields copied
		/// from <paramref name="original"/>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// If <paramref name="original"/> is null.
		/// </exception>
		public static T CloneComponent<T>(T original) where T : Component
		{
			if (original == null)
			{
				throw new System.ArgumentNullException(nameof(original));
			}
			return CloneComponent(original, original.gameObject);
		}

		/// <summary>
		/// Clones a component to another <see cref="GameObject"/>.
		/// </summary>
		/// <typeparam name="T">
		/// Component type.
		/// </typeparam>
		/// <param name="original">
		/// The original component to clone.
		/// </param>
		/// <param name="destination">
		/// The <see cref="GameObject"/> to attach the cloned
		/// component to.
		/// </param>
		/// <returns>
		/// The new component with fields copied
		/// from <paramref name="original"/>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// If <paramref name="original"/> and/or
		/// <paramref name="destination"/> is null.
		/// </exception>
		/// <remarks>
		/// Original source from 
		/// <a href="http://answers.unity.com/answers/1118416/view.html">turbanov</a>.
		/// </remarks>
		public static T CloneComponent<T>(T original, GameObject destination) where T : Component
		{
			// Check arguments
			if (original == null)
			{
				throw new System.ArgumentNullException(nameof(original));
			}
			else if (destination == null)
			{
				throw new System.ArgumentNullException(nameof(destination));
			}

			// Grabbing the real type of original, rather than relying on T.
			System.Type realType = original.GetType();

			// Create the new component at the destination
			T clone = (T)destination.AddComponent(realType);

			// Copy non-static fields from original to clone
			System.Reflection.FieldInfo[] fields = realType.GetFields();
			foreach (System.Reflection.FieldInfo field in fields)
			{
				// Skip static fields
				if (field.IsStatic)
				{
					continue;
				}
				field.SetValue(clone, field.GetValue(original));
			}

			// Copy properties from original to clone
			System.Reflection.PropertyInfo[] props = realType.GetProperties();
			foreach (System.Reflection.PropertyInfo prop in props)
			{
				if (prop.CanWrite == false

					// Skip name
					|| prop.Name == "name"

					// Skip obsolete properties
					|| prop.IsDefined(typeof(System.ObsoleteAttribute), true)

					// Skip materials due to memory leaks
					|| prop.PropertyType.Equals(typeof(Material))
					|| prop.PropertyType.Equals(typeof(Material[])))
				{
					continue;
				}
				prop.SetValue(clone, prop.GetValue(original, null), null);
			}

			if (original is Renderer originalRenderer)
			{
				// Edge case: for renderers, we're deliberately skipping the material properties
				// to prevent memory leaks.  Instead, copy over the shared materials
				(clone as Renderer).sharedMaterials = originalRenderer.sharedMaterials;
			}
			else if (clone is AudioSource clonedAudioSource)
			{
				// Edge case: for audio source, forcing time to be 0 to prevent errors.
				clonedAudioSource.time = 0;
				clonedAudioSource.Stop();
			}
			return clone;
		}
	}
}
