using UnityEngine;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="SupportPlatforms.cs" company="GB">
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
    /// <strong>Date:</strong> 6/12/2018<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial version.</description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 3/25/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial version.</description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.4-preview.1<br/>
    /// <strong>Date:</strong> 5/26/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Updating documentation to be more DocFX-friendly.
    /// Splitting up the <see cref="SupportedPlatformsHelpers"/> into a separate file
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// An enum indicating supported platforms. Can be multi-selected in the Unity Editor.
    /// <seealso cref="SupportedPlatformsHelpers"/>
    /// <seealso cref="OmiyaGames.Common.Editor.SupportedPlatformsDrawer"/>
    /// </summary>
    [System.Flags]
    public enum SupportedPlatforms
    {
        /// <summary>
        /// Flag for no platforms.
        /// </summary>
        None = 0,

        // To add more plaforms, just add them to the list below,
        // AND to the AllPlatforms value at the end.
        /// <summary>
        /// Support for <see cref="RuntimePlatform.WindowsPlayer"/>.
        /// </summary>
        Windows = 1 << 0,
        /// <summary>
        /// Support for <see cref="RuntimePlatform.OSXPlayer"/>.
        /// </summary>
        MacOS = 1 << 1,
        /// <summary>
        /// Support for <see cref="RuntimePlatform.LinuxPlayer"/>.
        /// </summary>
        Linux = 1 << 2,
        /// <summary>
        /// Support for <see cref="RuntimePlatform.WebGLPlayer"/>.
        /// </summary>
        Web = 1 << 3,
        /// <summary>
        /// Support for <see cref="RuntimePlatform.IPhonePlayer"/>.
        /// </summary>
        iOS = 1 << 4,
        /// <summary>
        /// Support for <see cref="RuntimePlatform.Android"/>.
        /// </summary>
        Android = 1 << 5,

        /// <summary>
        /// Flag for all platforms.
        /// </summary>
        AllPlatforms = Windows | MacOS | Linux | Web | iOS | Android,
    }
}
