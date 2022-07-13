using System.Text;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="ThreadSafeStringBuilder.cs" company="GB">
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
    /// Creates a thread-safe <see cref="StringBuilder"/>.
    /// </summary>
    public class ThreadSafeStringBuilder : ThreadSafe<StringBuilder>
    {
        /// <summary>
        /// Default constructor: creates an empty <see cref="StringBuilder"/>.
        /// </summary>
        public ThreadSafeStringBuilder() : base(new StringBuilder()) { }

        /// <summary>
        /// Sets up the initial capacity for <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="capacity"></param>
        public ThreadSafeStringBuilder(int capacity) : base(new StringBuilder(capacity)) { }

        /// <summary>
        /// Length of string.
        /// Same as <seealso cref="StringBuilder.Length"/>.
        /// </summary>
        public int Length
        {
            get
            {
                lock (ThreadLock)
                {
                    return value.Length;
                }
            }
        }

        /// <summary>
        /// Same as <seealso cref="StringBuilder.Clear"/>.
        /// </summary>
        public void Clear()
        {
            lock (ThreadLock)
            {
                value.Clear();
            }
        }

        /// <summary>
        /// Same as <seealso cref="StringBuilder.Append(string)"/>.
        /// </summary>
        public void Append(string append)
        {
            lock (ThreadLock)
            {
                value.Append(append);
            }
        }

        /// <summary>
        /// Same as <seealso cref="StringBuilder.Insert(int, string)"/>.
        /// </summary>
        public void Insert(int index, string insert)
        {
            lock (ThreadLock)
            {
                value.Insert(index, insert);
            }
        }

        /// <summary>
        /// Same as <seealso cref="StringBuilder.AppendLine()"/>.
        /// </summary>
        public void AppendLine()
        {
            lock (ThreadLock)
            {
                value.AppendLine();
            }
        }
    }
}
