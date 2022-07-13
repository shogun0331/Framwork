using UnityEngine;
using System;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="DefaultNumberAttribute.cs" company="GB">
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
    /// Creates a checkbox in the editor.
    /// If unchecked, the default value is set to this value.
    /// If checked, reveals a number field, allowing the user to change this value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class DefaultNumberAttribute : PropertyAttribute
    {
        /// <summary>
        /// Indicates whether the editor number is limited to being greater, less, or neither.
        /// </summary>
        public enum Range
        {
            /// <summary>
            /// Allow inspector to enter any number.
            /// </summary>
            FullRange,
            /// <summary>
            /// Prevents the inspector from entering a number below a certain value.
            /// </summary>
            /// <example>
            /// Useful for, say, setting a value to -1 by default, then forcing the inspector to only accept positive values.
            /// </example>
            GreaterThanOrEqualTo,
            /// <summary>
            /// Prevents the inspector from entering a number above a certain value.
            /// </summary>
            /// <example>
            /// Useful for, say, setting a value to 1 by default, then forcing the inspector to only accept negative values.
            /// </example>
            LessThanOrEqualTo
        }

        /// <summary>
        /// Creates a checkbox in the editor. If unchecked, argument is set to first argument.
        /// Otherwise, the user is allowed to set the 
        /// </summary>
        /// <param name="defaultNumber">
        /// The number set to the value if editor's checkbox is unchecked.
        /// </param>
        public DefaultNumberAttribute(float defaultNumber)
        {
            this.DefaultNumber = defaultNumber;
            StartNumber = defaultNumber;
            NumberRange = Range.FullRange;
        }

        /// <summary>
        /// Creates a checkbox in the editor. If unchecked, argument is set to first argument.
        /// Otherwise, the user is allowed to set the 
        /// </summary>
        /// <param name="defaultNumber">
        /// The number set to the value if editor's checkbox is unchecked.
        /// </param>
        /// <param name="greaterThan">
        /// Indicates whether the editor number is limited to being greater or less.
        /// </param>
        /// <param name="startNumber">
        /// If <code>greaterThan</code> is true, the number in the editor must be greater than this parameter.
        /// If false, then the number in the editor must be less.
        /// </param>
        public DefaultNumberAttribute(float defaultNumber, bool greaterThan, float startNumber)
        {
            this.DefaultNumber = defaultNumber;
            this.StartNumber = startNumber;

            // Set the number range
            NumberRange = Range.LessThanOrEqualTo;
            if (greaterThan == true)
            {
                NumberRange = Range.GreaterThanOrEqualTo;
            }
        }

        /// <summary>
        /// The number set to the value if editor's checkbox is unchecked.
        /// </summary>
        public float DefaultNumber
        {
            get;
        }

        /// <summary>
        /// Indicates whether the editor number is limited to being greater, less, or neither.
        /// </summary>
        public Range NumberRange
        {
            get;
        }

        /// <summary>
        /// If <code>numberRange</code> is <code>Range.GreaterThanOrEqualTo</code>,
        /// the number in the editor must be greater than this parameter.
        /// If <code>Range.LessThanOrEqualTo</code>, then the number in the editor must be less.
        /// Otherwise, this value is the first value the editor is set to when it's checked.
        /// </summary>
        public float StartNumber
        {
            get;
        }
    }
}
