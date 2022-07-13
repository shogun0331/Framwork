using UnityEngine;

namespace GB.Global
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="ComponentSingleton.cs" company="GB">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2021 GB
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
    /// <strong>Version:</strong> 1.1.0<br/>
    /// <strong>Date:</strong> 11/27/2021<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial verison.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Use this class to get a static instance of a component.
    /// Mainly used to have a default instance.
    /// </summary>
    /// <remarks>
    /// Code from Unity's Core RenderPipeline package (<c>ComponentSingleton<TType></c>.)
    /// </remarks>
    /// <typeparam name="T">Component type.</typeparam>
    public static class ComponentSingleton<T> where T : Component
    {
        const bool DefaultActive = false;
        const HideFlags DefaultFlags = HideFlags.HideAndDontSave;
        static T instance = null;

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <remarks>
        /// If this method creates a new <see cref="GameObject"/>,
        /// it will be deactivated by default with <c>hideFlags</c>
        /// set to <see cref="HideFlags.HideAndDontSave"/>.
        /// </remarks>
        /// <returns>Instance of the required component type.</returns>
        public static T Get() => Get(out _);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <remarks>
        /// If this method creates a new <see cref="GameObject"/>,
        /// it will be deactivated by default with <c>hideFlags</c>
        /// set to <see cref="HideFlags.HideAndDontSave"/>.
        /// </remarks>
        /// <param name="isFirstTimeCreated">
        /// True if this is the call that creates the first instance of this
        /// singleton; false, otherwise (which is most of the time.)
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(out bool isFirstTimeCreated) => Get(DefaultActive, out isFirstTimeCreated);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <remarks>
        /// If this method creates a new <see cref="GameObject"/>,
        /// it will be deactivated by default.
        /// </remarks>
        /// <param name="flags">
        /// If first time creating this singleton, this flag determines
        /// what condition the associated <see cref="GameObject"/> will be
        /// in.
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(HideFlags flags) => Get(flags, out _);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <remarks>
        /// If this method creates a new <see cref="GameObject"/>,
        /// the <c>hideFlags</c> will be set to <see cref="HideFlags.HideAndDontSave"/>.
        /// </remarks>
        /// <param name="isActive">
        /// If first time creating this singleton, this boolean determines
        /// whether the associated <see cref="GameObject"/> will be active
        /// as well.
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(bool isActive) => Get(isActive, out _);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <remarks>
        /// If this method creates a new <see cref="GameObject"/>,
        /// it will be deactivated by default.
        /// </remarks>
        /// <param name="flags">
        /// If first time creating this singleton, this flag determines
        /// what condition the associated <see cref="GameObject"/> will be
        /// in.
        /// </param>
        /// <param name="isFirstTimeCreated">
        /// True if this is the call that creates the first instance of this
        /// singleton; false, otherwise (which is most of the time.)
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(HideFlags flags, out bool isFirstTimeCreated) => Get(DefaultActive, flags, out isFirstTimeCreated);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <remarks>
        /// If this method creates a new <see cref="GameObject"/>,
        /// the <c>hideFlags</c> will be set to <see cref="HideFlags.HideAndDontSave"/>.
        /// </remarks>
        /// <param name="isActive">
        /// If first time creating this singleton, this boolean determines
        /// whether the associated <see cref="GameObject"/> will be active
        /// as well.
        /// </param>
        /// <param name="isFirstTimeCreated">
        /// True if this is the call that creates the first instance of this
        /// singleton; false, otherwise (which is most of the time.)
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(bool isActive, out bool isFirstTimeCreated) => Get(isActive, DefaultFlags, out isFirstTimeCreated);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <param name="isActive">
        /// If first time creating this singleton, this boolean determines
        /// whether the associated <see cref="GameObject"/> will be active
        /// as well.
        /// </param>
        /// <param name="flags">
        /// If first time creating this singleton, this flag determines
        /// what condition the associated <see cref="GameObject"/> will be
        /// in.
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(HideFlags flags, bool isActive) => Get(isActive, flags, out _);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <param name="isActive">
        /// If first time creating this singleton, this boolean determines
        /// whether the associated <see cref="GameObject"/> will be active
        /// as well.
        /// </param>
        /// <param name="flags">
        /// If first time creating this singleton, this flag determines
        /// what condition the associated <see cref="GameObject"/> will be
        /// in.
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(bool isActive, HideFlags flags) => Get(isActive, flags, out _);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <param name="isActive">
        /// If first time creating this singleton, this boolean determines
        /// whether the associated <see cref="GameObject"/> will be active
        /// as well.
        /// </param>
        /// <param name="flags">
        /// If first time creating this singleton, this flag determines
        /// what condition the associated <see cref="GameObject"/> will be
        /// in.
        /// </param>
        /// <param name="isFirstTimeCreated">
        /// True if this is the call that creates the first instance of this
        /// singleton; false, otherwise (which is most of the time.)
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(HideFlags flags, bool isActive, out bool isFirstTimeCreated) => Get(isActive, flags, out isFirstTimeCreated);

        /// <summary>
        /// Instance of the required component type.
        /// </summary>
        /// <param name="isActive">
        /// If first time creating this singleton, this boolean determines
        /// whether the associated <see cref="GameObject"/> will be active
        /// as well.
        /// </param>
        /// <param name="flags">
        /// If first time creating this singleton, this flag determines
        /// what condition the associated <see cref="GameObject"/> will be
        /// in.
        /// </param>
        /// <param name="isFirstTimeCreated">
        /// True if this is the call that creates the first instance of this
        /// singleton; false, otherwise (which is most of the time.)
        /// </param>
        /// <returns>Instance of the required component type.</returns>
        public static T Get(bool isActive, HideFlags flags, out bool isFirstTimeCreated)
        {
            isFirstTimeCreated = false;

            if (instance == null)
            {
                // Create gameobject
                GameObject gameObject = new GameObject(typeof(T).Name)
                {
                    hideFlags = flags
                };
                gameObject.SetActive(isActive);

                // Mark GameObject as not to destroy
#if UNITY_EDITOR
                if (Application.isPlaying == true)
                {
                    Object.DontDestroyOnLoad(gameObject);
                }
#else
                Object.DontDestroyOnLoad(gameObject);
#endif

                // Create component
                instance = gameObject.AddComponent<T>();
                isFirstTimeCreated = true;
            }

            return instance;
        }

        /// <summary>
        /// Destroys this singleton and all components attached to it.
        /// </summary>
        public static bool Release()
        {
            if (instance != null)
            {
                Helpers.Destroy(instance.gameObject);
                instance = null;
                return true;
            }
            return false;
        }
    }
}
