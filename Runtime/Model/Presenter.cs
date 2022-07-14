using System.Collections.Generic;
using UnityEngine;

using GB.Global;
using GB.UI;

namespace GB
{
    /// <remarks>
    /// <copyright file="Presenter.cs" company="GB">
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

    public class Presenter : MonoBehaviour
    {
        const HideFlags Flags = HideFlags.HideInHierarchy | HideFlags.DontSave;
        public static Presenter Instance => Application.isPlaying ? ComponentSingleton<Presenter>.Get(Flags) : null;
        Dictionary<string, List<UIScreen>> _dicUIScreen = new Dictionary<string, List<UIScreen>>();

        public static void Clear()
        {
            Instance._dicUIScreen.Clear();
        }

        public static void Regist(string key, UIScreen screen)
        {
            if (Instance._dicUIScreen.ContainsKey(key))
            {
                Instance._dicUIScreen[key].Add(screen);
            }
            else
            {
                List<UIScreen> screenList = new List<UIScreen>();
                screenList.Add(screen);
                Instance._dicUIScreen.Add(key, screenList);
            }
        }

        public static void Send(string key, object model)
        {
            if (Instance._dicUIScreen.ContainsKey(key))
            {
                List<UIScreen> screenList = new List<UIScreen>();

                for (int i = 0; i < screenList.Count; ++i)
                {
                    if (screenList[i] == null) continue;
                    screenList[i].OnChangeValue(model);
                }
            }
        }

        public static void Send(object model)
        {
             
            if (Instance._dicUIScreen.ContainsKey(model.GetType().Name))
            {
                List<UIScreen> screenList = Instance._dicUIScreen[model.GetType().Name];

                for (int i = 0; i < screenList.Count; ++i)
                {
                    if (screenList[i] == null) continue;
                    screenList[i].OnChangeValue(model);
                }
            }
        }
    }
}