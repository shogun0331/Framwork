using System.Collections;
using UnityEngine;

namespace GB.Global
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="Manager.cs" company="GB">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2014-2022 GB
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
	/// <strong>Version:</strong> 1.4.0<br/>
	/// <strong>Date:</strong> 2/27/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>Initial verison.</description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// Helper manager to start coroutines and/or listen to Unity's
	/// update events. Useful for runtime scripts that <em>doesn't</em> extend
	/// <see cref="MonoBehaviour"/>, e.g. <see cref="ScriptableObject"/>s.
	/// Note that Unity events needs to be manually started by calling
	/// <seealso cref="StartEvents"/> once.
	/// </summary>
	/// <code>
	/// // Listen to the Update event
	/// Manager.OnUpdate += (args) => Debug.Log($"deltaTime = {args.DeltaTimeUnscaled}");
	/// 
	/// // Calling StartEvents() is required for OnUpdate to trigger properly
	/// Manager.StartEvents();
	/// 
	/// // Start a coroutine
	/// Coroutine test = Manager.StartCoroutine(TestCoroutine());
	/// 
	/// // Stop a coroutine
	/// Manager.StopCoroutine(test);
	/// </code>
	public static class Manager
	{
		/// <summary>
		/// Arguments to pass each event call.
		/// </summary>
		public class TimeArgs : System.EventArgs
		{
			/// <summary>
			/// Seconds between each call, affected by
			/// <see cref="Time.timeScale"/>.
			/// </summary>
			public float DeltaTimeScaled
			{
				get;
				internal set;
			}
			/// <summary>
			/// Seconds between each call, unaffected by
			/// <see cref="Time.timeScale"/>.
			/// </summary>
			public float DeltaTimeUnscaled
			{
				get;
				internal set;
			}
			/// <summary>
			/// Seconds that has passed since start of game,
			/// affected by <see cref="Time.timeScale"/>.
			/// </summary>
			public float TimeSinceStartScaled
			{
				get;
				internal set;
			}
			/// <summary>
			/// Seconds that has passed since start of game,
			/// unaffected by <see cref="Time.timeScale"/>.
			/// </summary>
			public float TimeSinceStartUnscaled
			{
				get;
				internal set;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="scaledDeltaTime"></param>
		/// <param name="unscaledDeltaTime"></param>
		public delegate void EachFrame(TimeArgs args);
		/// <summary>
		/// Triggers each frame.
		/// </summary>
		public static event EachFrame OnUpdate;
		/// <summary>
		/// Triggers each frame, after
		/// <seealso cref="OnUpdate"/> is finished calling.
		/// </summary>
		public static event EachFrame OnLateUpdate;
		/// <summary>
		/// Triggered every physics update.
		/// </summary>
		public static event EachFrame OnFixedUpdate;

		/// <summary>
		/// Starts <seealso cref="OnUpdate"/>, <seealso cref="OnLateUpdate"/>,
		/// and <seealso cref="OnFixedUpdate"/> if they weren't running already.
		/// </summary>
		public static void StartEvents() => GetInstance();

		/// <summary>
		/// Starts a coroutine. Useful runtime function if calling from a non-MonoBehavior,
		/// or a deactivated one.
		/// </summary>
		/// <param name="coroutine">
		/// The enumerator to generate a coroutine from.
		/// </param>
		/// <returns>
		/// The started coroutine.
		/// </returns>
		/// <seealso cref="StopCoroutine(Coroutine)"/>
		public static Coroutine StartCoroutine(IEnumerator coroutine) => GetInstance().StartCoroutine(coroutine);

		/// <summary>
		/// Stops a coroutine. Useful runtime function if calling from a non-MonoBehavior,
		/// or a deactivated one.
		/// </summary>
		/// <param name="coroutine">
		/// Coroutine to stop.
		/// </param>
		/// <seealso cref="StartCoroutine(IEnumerator)"/>
		public static void StopCoroutine(Coroutine coroutine) => GetInstance().StopCoroutine(coroutine);

		/// <summary>
		/// Stops all coroutines running on Manager.
		/// </summary>
		public static void StopAllCoroutine() => GetInstance().StopAllCoroutines();

		static ManagerImpl GetInstance() => ComponentSingleton<ManagerImpl>.Get(true);

		/// <summary>
		/// The implementation to trigger Unity events.
		/// </summary>
		class ManagerImpl : MonoBehaviour
		{
			// readonly TimeArgs updateArgs = new(),
			// 	lateUpdateArgs = new(),
			// 	fixedUpdateArgs = new();

			// void Update()
			// {
			// 	CallEvent(in OnUpdate, in updateArgs);
			// }

			// void LateUpdate()
			// {
			// 	CallEvent(in OnLateUpdate, in lateUpdateArgs);
			// }

			// void FixedUpdate()
			// {
			// 	CallEvent(in OnFixedUpdate, in fixedUpdateArgs);
			// }

			static void CallEvent(in EachFrame theEvent, in TimeArgs args)
			{
				if (theEvent != null)
				{
					// Update args
					args.DeltaTimeScaled = Time.deltaTime;
					args.DeltaTimeUnscaled = Time.unscaledDeltaTime;
					args.TimeSinceStartScaled = Time.time;
					args.TimeSinceStartUnscaled = Time.unscaledTime;

					// Call event
					theEvent(args);
				}
			}
		}
	}
}
