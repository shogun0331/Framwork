using System.Threading;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="ThreadSafeInt.cs" company="GB">
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
    /// <strong>Date:</strong> 10/2/2018<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Initial version.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 3/25/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Converted the class to a package.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.4-preview.1<br/>
    /// <strong>Date:</strong> 5/27/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Updating documentation to be compatible with DocFX.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// An int version of <see cref="ThreadSafe{T}"/> with more performant helper methods.
    /// </summary>
    public class ThreadSafeInt : ThreadSafe<int>
    {
        /// <summary>
        /// Default constructor that sets <see cref="ThreadSafe{T}.Value"/> to 0.
        /// </summary>
        public ThreadSafeInt() : base() { }

        /// <summary>
        /// Constructor that sets <see cref="ThreadSafe{T}.Value"/>.
        /// </summary>
        /// <param name="value">The initial <see cref="ThreadSafe{T}.Value"/> should be set to.</param>
        public ThreadSafeInt(int value) : base(value) { }

        /// <summary>
        /// A more performant version of <see cref="ThreadSafe{T}.Value"/>++.
        /// </summary>
        public void Increment()
        {
            Interlocked.Increment(ref value);
        }

        /// <summary>
        /// A more performant version of <see cref="ThreadSafe{T}.Value"/>--.
        /// </summary>
        public void Decrement()
        {
            Interlocked.Decrement(ref value);
        }

        /// <summary>
        /// A more performant version of <see cref="ThreadSafe{T}.Value"/> += addBy.
        /// </summary>
        public void Add(int addBy)
        {
            Interlocked.Add(ref value, addBy);
        }

        /// <summary>
        /// A more performant version of <see cref="ThreadSafe{T}.Value"/> -= subtractBy.
        /// </summary>
        public void Subtract(int subtractBy)
        {
            Add(-subtractBy);
        }
    }
}
