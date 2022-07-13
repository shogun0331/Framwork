using UnityEngine;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="HsvColor.cs">
    /// <para>
    /// Code by Jonathan Czeck from Unify Community:
    /// http://wiki.unity3d.com/index.php/HSBColor
    /// </para><para>
    /// Licensed under Creative Commons Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0):
    /// http://creativecommons.org/licenses/by-sa/3.0/
    /// </para>
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Date:</strong> 9/3/2015<br/>
    /// <strong>Author:</strong> Jonathan Czeck
    /// </term>
    /// <description>
    /// Initial wiki version.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 3/25/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Converted the class to a package. Using Unity's own helper functions.
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
    /// Displays the frame-rate in the upper-left hand corner of the screen.
    /// </summary>
    [System.Serializable]
    public struct HsvColor
    {
        [Range(0f, 1f)]
        [SerializeField]
        float hue;
        [Range(0f, 1f)]
        [SerializeField]
        float saturation;
        [Range(0f, 1f)]
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("brightness")]
        float value;
        [Range(0f, 1f)]
        [SerializeField]
        float alpha;

        #region Properties
        /// <summary>
        /// The hue of the color, or it's color type.<br/>
        /// Set as a fraction between 0 and 1.
        /// </summary>
        public float Hue
        {
            get
            {
                return hue;
            }
            set
            {
                hue = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// The saturation of the color, or its "intenseness."<br/>
        /// Set as a fraction between 0 and 1.
        /// </summary>
        public float Saturation
        {
            get
            {
                return saturation;
            }
            set
            {
                saturation = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// The value of the color.
        /// Also known as lightness, brightness, etc.<br/>
        /// Set as a fraction between 0 and 1.
        /// </summary>
        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// The alpha of the color, or its opacity.<br/>
        /// Set as a fraction between 0 and 1.
        /// </summary>
        public float Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = Mathf.Clamp01(value);
            }
        }
        #endregion

        /// <summary>
        /// Constructs a new color, setting <see cref="Hue"/>,
        /// <see cref="Saturation"/>, <see cref="Value"/>, and
        /// <see cref="Alpha"/>.
        /// </summary>
        /// <param name="h">Sets <see cref="Hue"/>.</param>
        /// <param name="s">Sets <see cref="Saturation"/>.</param>
        /// <param name="v">Sets <see cref="Value"/>.</param>
        /// <param name="a">Sets <see cref="Alpha"/>.</param>
        public HsvColor(float h, float s, float v, float a = 1f)
        {
            hue = Mathf.Clamp01(h);
            saturation = Mathf.Clamp01(s);
            value = Mathf.Clamp01(v);
            alpha = Mathf.Clamp01(a);
        }

        /// <summary>
        /// Clone constructor: creates a duplicate of <paramref name="col"/>.
        /// </summary>
        /// <param name="col">Color to duplicate.</param>
        public HsvColor(HsvColor col) : this(col.Hue, col.Saturation, col.Value, col.Alpha) { }

        /// <summary>
        /// Constructor that converts <see cref="Color"/> to <see cref="HsvColor"/>.
        /// </summary>
        /// <param name="col">Color to convert.</param>
        public HsvColor(Color col)
        {
            // Just use Unity's own helper function
            Color.RGBToHSV(col, out hue, out saturation, out value);
            alpha = Mathf.Clamp01(col.a);
        }

        /// <summary>
        /// Converts <see cref="Color"/> to <see cref="HsvColor"/>.
        /// <seealso cref="HsvColor(Color)"/>
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <returns><see cref="HsvColor"/> equivalent of <paramref name="color"/>.</returns>
        public static HsvColor FromColor(Color color)
        {
            return new HsvColor(color);
        }

        /// <summary>
        /// Converts <see cref="HsvColor"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <param name="isHdr">Flag indicating whether High-Definition Range is on.</param>
        /// <returns><see cref="Color"/> equivalent of <paramref name="color"/>.</returns>
        public static Color ToColor(HsvColor color, bool isHdr = false)
        {
            // Just use Unity's helper function
            Color toReturn = Color.HSVToRGB(color.Hue, color.Saturation, color.Value, isHdr);

            // Don't forget to set the alpha value
            toReturn.a = color.Alpha;
            return toReturn;
        }

        /// <summary>
        /// Converts this to a <see cref="Color"/>.
        /// </summary>
        /// <param name="isHdr">Flag indicating whether High-Definition Range is on.</param>
        /// <returns><see cref="Color"/> equivalent.</returns>
        public Color ToColor(bool isHdr = false)
        {
            return ToColor(this, isHdr);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "H:" + hue + " S:" + saturation + " V:" + value;
        }

        /// <summary>
        /// Linearly interpolates between two colors.
        /// </summary>
        /// <param name="from">The start color.</param>
        /// <param name="to">The end color.</param>
        /// <param name="time">A value between 0 to 1, with 0 as start, and 1 as end.</param>
        /// <returns>
        /// A color in-between <paramref name="from"/> and <paramref name="to"/>,
        /// based on <paramref name="time"/>.
        /// </returns>
        /// <example>
        /// Output depends on <paramref name="time"/> in particular. For example:
        /// <code>
        /// Lerp(from, to, 0f);
        /// </code>
        /// ...would return just "from."  Similarly:
        /// <code>
        /// Lerp(from, to, 1f);
        /// </code>
        /// ...would return just "to."  Nautrally, it would follow that:
        /// <code>
        /// Lerp(from, to, 0.5f);
        /// </code>
        /// ...gives a color midway between "from" and "to."
        /// </example>
        public static HsvColor Lerp(HsvColor from, HsvColor to, float time)
        {
            float hue, saturation;

            //check special case black (color.b==0): interpolate neither hue nor saturation!
            //check special case grey (color.s==0): don't interpolate hue!
            if (from.value == 0)
            {
                hue = to.hue;
                saturation = to.saturation;
            }
            else if (to.value == 0)
            {
                hue = from.hue;
                saturation = from.saturation;
            }
            else
            {
                if (from.saturation == 0)
                {
                    hue = to.hue;
                }
                else if (to.saturation == 0)
                {
                    hue = from.hue;
                }
                else
                {
                    // works around bug with LerpAngle
                    float angle = Mathf.LerpAngle((from.hue * 360f), (to.hue * 360f), time);
                    while (angle < 0f)
                    {
                        angle += 360f;
                    }
                    while (angle > 360f)
                    {
                        angle -= 360f;
                    }
                    hue = angle / 360f;
                }
                saturation = Mathf.Lerp(from.saturation, to.saturation, time);
            }
            return new HsvColor(hue, saturation, Mathf.Lerp(from.value, to.value, time), Mathf.Lerp(from.alpha, to.alpha, time));
        }
    }
}
