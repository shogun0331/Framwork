using UnityEngine;

namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="ReadOnlyAttribute.cs">
    /// Code by It3ration from Unity Answers:
    /// http://answers.unity3d.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Date:</strong> 10/1/2014<br/>
    /// <strong>Author:</strong> It3ration
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
    /// Makes a field read-only in the Unity editor with [ReadOnly].
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}
