using UnityEngine;

namespace GB.Global
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="ISingletonScript.cs" company="GB">
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
    /// <strong>Version:</strong> 0.0.0-preview.1<br/>
    /// <strong>Date:</strong> 5/18/2015<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial verison.</description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 5/18/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Converting code to package.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// An abstract class with functions that are called on certain
    /// <code>Singleton</code>'s events.
    /// <seealso cref="Singleton"/>
    /// </summary>
    public abstract class ISingletonScript : MonoBehaviour
    {
        /// <summary>
        /// Indicates whether this instance can retrievable from <see cref="Singleton"/> or not.
        /// </summary>
        /// <value>Set to true if this script is attached to a <see cref="GameObject"/> on or a child of <see cref="Singleton"/></value>
        /// <seealso cref="Singleton"/>
        public bool IsPartOfSingleton
        {
            get;
            internal set;
        } = false;

        /// <summary>
        /// Runs when the <see cref="Singleton.Instance"/> calls Awake().
        /// </summary>
        public abstract void SingletonAwake();
        /// <summary>
        /// Runs when, after a scene transition, Awake() is called.
        /// This method is called directly after <see cref="SingletonAwake"/> as well.
        /// </summary>
        public abstract void SceneAwake();
    }
}
