using UnityEngine;
using System;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="FolderPathAttribute.cs" company="GB">
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
    /// <strong>Date:</strong> 6/26/2018<br/>
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
    /// Adds a browse button to the inspector to select a path.
    /// Simply add [FolderPath] to a string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class FolderPathAttribute : PropertyAttribute
    {
        /// <summary>
        /// Default local path.
        /// </summary>
        public const string DefaultLocalPath = "Assets";

        /// <summary>
        /// The directory <see cref="FolderPathAttribute"/> should be set local to.
        /// </summary>
        public enum RelativeTo
        {
            /// <summary>
            /// Path will be set to absolute path.
            /// </summary>
            None,
            /// <summary>
            /// Path will be set to local, if detected.
            /// </summary>
            ProjectDirectory,
            //ResourcesFolder
        }

        /// <summary>
        /// Constructor for setting up the folder path inspector.
        /// </summary>
        /// <param name="defaultPath">The default path to open the dialog in.</param>
        /// <param name="relativeTo">The folder to make the path relative to.</param>
        /// <param name="displayWarning">
        /// Flag on whether to display a warning on an invalid path in the inspector.
        /// </param>
        public FolderPathAttribute(string defaultPath = DefaultLocalPath, RelativeTo relativeTo = RelativeTo.None, bool displayWarning = true)
        {
            DefaultPath = defaultPath;
            PathRelativeTo = relativeTo;
            IsWarningDisplayed = displayWarning;
        }

        /// <summary>
        /// What folder the path string is relative to.
        /// </summary>
        public RelativeTo PathRelativeTo
        {
            get;
        }

        /// <summary>
        /// The default path to open the dialog in.
        /// </summary>
        public string DefaultPath
        {
            get;
        }

        /// <summary>
        /// If true, displays a warning if the folder path is not valid.
        /// </summary>
        public bool IsWarningDisplayed
        {
            get;
        }
    }
}
