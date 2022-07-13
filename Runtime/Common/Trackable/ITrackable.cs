namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="ITrackable.cs" company="GB">
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
    /// <strong>Date:</strong> 8/4/2021<br/>
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
    /// A value that allows tracking changes via C# events.
    /// </summary>
    /// <typeparam name="T">Type of value being tracked.</typeparam>
    public interface ITrackable<T>
    {
        /// <summary>
        /// Delegate for tracking changes to <seealso cref="Value"/>.
        /// </summary>
        public delegate void ChangeEvent(T oldValue, T newValue);
        /// <summary>
        /// Event called before the value has changed.
        /// Will be called even if the new value is the same as old.
        /// </summary>
        public event ChangeEvent OnBeforeValueChanged;
        /// <summary>
        /// Event called after the value has changed.
        /// Will be called even if the new value is the same as old.
        /// </summary>
        public event ChangeEvent OnAfterValueChanged;

        /// <summary>
        /// The value this class represents.
        /// </summary>
        T Value
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating if <see cref="Value"/> is not null.
        /// </summary>
        bool HasValue
        {
            get;
        }
    }
}
