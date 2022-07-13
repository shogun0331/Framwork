using UnityEngine;
using System;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="SupportedPlatformsHelpers.cs" company="GB">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2020 GB
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
    /// <strong>Version:</strong> 0.1.4-preview.1<br/>
    /// <strong>Date:</strong> 5/26/2020<br/>
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
    /// A class full of helper and extended methods for <see cref="SupportPlatforms"/>.
    /// </summary>
    public static class SupportedPlatformsHelpers
    {
        /// <summary>
        /// Argument for event <see cref="OnAfterIsSupportedNoArgs"/>.
        /// </summary>
        public class PlatformSupportArgs : EventArgs
        {
            /// <summary>
            /// The current build platform.
            /// </summary>
            public RuntimePlatform BuildPlatform
            {
                get;
            }

            /// <summary>
            /// What the event will be returning.
            /// Editing this value will also affect what
            /// <see cref="SupportedPlatformsHelpers.IsSupported(SupportedPlatforms)"/>
            /// will return.
            /// </summary>
            public bool IsSupported
            {
                get;
                set;
            }

            /// <summary>
            /// Constructor to set all property values.
            /// </summary>
            /// <param name="currentPlatform">Sets <see cref="BuildPlatform"/></param>
            /// <param name="returnFlag">Sets <see cref="IsSupported"/></param>
            public PlatformSupportArgs(RuntimePlatform currentPlatform, bool returnFlag)
            {
                BuildPlatform = currentPlatform;
                IsSupported = returnFlag;
            }
        }

        /// <summary>
        /// Delegate for <see cref="OnAfterIsSupportedNoArgs"/> event.
        /// <seealso cref="PlatformSupportArgs"/>
        /// </summary>
        /// <param name="source">The source of the event call.</param>
        /// <param name="args">
        /// Arguments for the event. Can be edited to change what
        /// <see cref="OnAfterIsSupportedNoArgs"/> returns.
        /// </param>
        public delegate void OverridePlatformSupport(SupportedPlatforms source, PlatformSupportArgs args);
        /// <summary>
        /// Called towards the end of <see cref="IsSupported(SupportedPlatforms)"/>.
        /// Allows changing the return value with
        /// <see cref="PlatformSupportArgs.IsSupported"/>.
        /// </summary>
        public static event OverridePlatformSupport OnAfterIsSupportedNoArgs;

        /// <summary>
        /// Gets the number of flags set in <code>SupportPlatforms</code>.
        /// It is highly recommended to cache this value.
        /// </summary>
        public static int NumberOfPlatforms
        {
            get
            {
                int returnNumber = 0;
                int flags = (int)SupportedPlatforms.AllPlatforms;
                while (flags != 0)
                {
                    // Remove the last bit
                    flags &= (flags - (1 << 0));

                    // Increment the return value;
                    ++returnNumber;
                }
                return returnNumber;
            }
        }

        /// <summary>
        /// Gets a list of all the platform names.
        /// It is highly recommended to cache this value.
        /// </summary>
        public static string[] AllPlatformNames
        {
            get
            {
                // Setup return value
                int numberOfPlatforms = NumberOfPlatforms;
                string[] returnNames = new string[numberOfPlatforms];

                // Iterate through all the platforms, in order
                SupportedPlatforms convertedEnum;
                for (int bitPosition = 0; bitPosition < numberOfPlatforms; ++bitPosition)
                {
                    convertedEnum = (SupportedPlatforms)(1 << bitPosition);
                    returnNames[bitPosition] = convertedEnum.ToString();
                }
                return returnNames;
            }
        }

        /// <summary>
        /// Indicates if <paramref name="currentPlatforms"/> matches
        /// the current build.
        /// <seealso cref="Application.platform"/>
        /// <seealso cref="OnAfterIsSupportedNoArgs"/>
        /// </summary>
        /// <param name="currentPlatforms">
        /// Flag to check whether it supports this build.
        /// </param>
        /// <returns>
        /// True if <paramref name="currentPlatforms"/> matches this build.
        /// </returns>
        public static bool IsSupported(this SupportedPlatforms currentPlatforms)
        {
            // Call the default method
            bool returnFlag = IsSupported(currentPlatforms, Application.platform);

            // Run an event to see if any intends to change the default return value.
            PlatformSupportArgs eventArgs = new PlatformSupportArgs(Application.platform, returnFlag);
            OnAfterIsSupportedNoArgs?.Invoke(currentPlatforms, eventArgs);

            // Return the event's aggregate return value.
            return eventArgs.IsSupported;
        }

        /// <summary>
        /// Indicates if <paramref name="currentPlatforms"/> matches
        /// <paramref name="singlePlatform"/>.
        /// </summary>
        /// <param name="currentPlatforms">
        /// Flag to check whether it supports <paramref name="singlePlatform"/>.
        /// </param>
        /// <param name="singlePlatform">
        /// Platform to check against <paramref name="currentPlatforms"/>.
        /// </param>
        /// <returns>
        /// True if <paramref name="currentPlatforms"/> contains
        /// <paramref name="singlePlatform"/>.
        /// </returns>
        public static bool IsSupported(this SupportedPlatforms currentPlatforms, SupportedPlatforms singlePlatform)
        {
            return (currentPlatforms & singlePlatform) != 0;
        }

        /// <summary>
        /// Indicates if <paramref name="currentPlatforms"/> matches
        /// <paramref name="platform"/>.
        /// </summary>
        /// <param name="currentPlatforms">
        /// Flag to check whether it supports <paramref name="platform"/>.
        /// </param>
        /// <param name="platform">
        /// Platform to check against <paramref name="currentPlatforms"/>.
        /// </param>
        /// <returns>
        /// True if <paramref name="currentPlatforms"/> contains
        /// <paramref name="platform"/>.
        /// </returns>
        public static bool IsSupported(this SupportedPlatforms currentPlatforms, RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.Windows);
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.MacOS);
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.Linux);
                case RuntimePlatform.WebGLPlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.Web);
                case RuntimePlatform.IPhonePlayer:
                    return IsSupported(currentPlatforms, SupportedPlatforms.iOS);
                case RuntimePlatform.Android:
                    return IsSupported(currentPlatforms, SupportedPlatforms.Android);
                default:
                    return false;
            }
        }
    }
}
