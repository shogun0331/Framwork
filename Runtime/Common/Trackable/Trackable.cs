using UnityEngine;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="Trackable.cs" company="GB">
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
    /// <strong>Date:</strong> 6/28/2021<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Initial version.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A <see cref="System.Serializable"/> value one can track changes to by
    /// using <seealso cref="OnBeforeValueChanged"/> and
    /// <seealso cref="OnAfterValueChanged"/>.
    /// </summary>
    /// <typeparam name="T">Type of value being tracked.</typeparam>
    [System.Serializable]
    public class Trackable<T> : TrackableDecorator<T>, IEditorTrackable
    {
        [SerializeField]
        T value;

        /// <inheritdoc/>
        public override event ITrackable<T>.ChangeEvent OnBeforeValueChanged;
        /// <inheritdoc/>
        /// <remarks>
        /// This event will also be called from the editor, although arguments
        /// may not be updated to the latest if <typeparamref name="T"/>
        /// is a <see cref="System.SerializableAttribute"/> class or struct instance.
        /// </remarks>
        public override event ITrackable<T>.ChangeEvent OnAfterValueChanged;

        /// <summary>
        /// Constructor to set the initial value.
        /// No events will be called.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public Trackable(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Trackable() : this(default(T)) { }

        /// <summary>
        /// Converts a <see cref="Trackable{T}"/> to its value type.
        /// </summary>
        public static implicit operator T(Trackable<T> trackable) => trackable.Value;

        /// <inheritdoc/>
        public override T Value
        {
            get => value;
            set
            {
                OnBeforeValueChanged?.Invoke(this.value, value);
                T oldValue = this.value;
                this.value = value;
                OnAfterValueChanged?.Invoke(oldValue, this.value);
            }
        }

        /// <inheritdoc/>
        public object EditorValue => Value;

        /// <inheritdoc/>
        /// <remarks>
        /// Arguments may not be updated to the latest if <typeparamref name="T"/>
        /// is a <see cref="System.SerializableAttribute"/> class or struct instance.
        /// </remarks>
        public void OnValueChangedInEditor(object oldValue, object newValue)
        {
            OnAfterValueChanged?.Invoke((T)oldValue, (T)newValue);
        }
    }
}
