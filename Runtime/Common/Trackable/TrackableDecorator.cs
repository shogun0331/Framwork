using UnityEngine;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="TrackableDecorator.cs" company="GB">
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
    /// A helper class to implement
    /// <seealso cref="ITrackable{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of value being tracked.</typeparam>
    public abstract class TrackableDecorator<T> : ITrackable<T>
    {
        /// <inheritdoc/>
        public abstract event ITrackable<T>.ChangeEvent OnBeforeValueChanged;
        /// <inheritdoc/>
        public abstract event ITrackable<T>.ChangeEvent OnAfterValueChanged;

        /// <inheritdoc/>
        public abstract T Value
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public bool HasValue => Value != null;
    }
}
