using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GB.Global;


namespace GB.UI
{

    /// <remarks>
    /// <copyright file="UIManager.cs" company="GB">
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

    public class UIManager : MonoBehaviour
    {
        
        const HideFlags Flags = HideFlags.HideInHierarchy | HideFlags.DontSave;
        public static UIManager Instance => Application.isPlaying ? ComponentSingleton<UIManager>.Get(Flags) : null;
        
        
        private Dictionary<string, UIScreen> _screenList = new Dictionary<string, UIScreen>();
        private Stack<UIScreen> _screenStack = new Stack<UIScreen>();
        private List<UIScreen> _popupList = new List<UIScreen>();

        public void Clear()
        {
            _screenList.Clear();
            _screenStack.Clear();
            _popupList.Clear();
            Presenter.Clear();
        }

        public void ChangeScene(string sceneName)
        {
            Clear();
            SceneManager.LoadScene(sceneName);
        }

        public UIScreen FindScreen(string name)
        {
            if (_screenList.ContainsKey(name))
                return _screenList[name];
            
            return null;
        }

        public void ShowScreen(string name, int extraValue = 0)
        {
            _screenStack.Peek().gameObject.SetActive(false);

            if (_screenList.ContainsKey(name))
            {
                _screenList[name].gameObject.SetActive(true);       
                _screenStack.Push(_screenList[name]);
            }
            else
            {
                LoadFromResources(name);
            }
        }

        public void ShowPopup(string name, int extraValue = 0)
        {
            SortingPopup();

            if (_popupList.Count > 0)
            {
               
                if (_popupList[0] == null)
                {
                    _popupList.RemoveAt(0);
                    ShowPopup(name);
                    return;
                }
            }

            if (_screenList.ContainsKey(name))
            {
                _screenList[name].gameObject.SetActive(true);  
           
                if(!_popupList.Contains(_screenList[name]))
                    _popupList.Add(_screenList[name]);

                _screenList[name].GetComponent<RectTransform>().SetAsLastSibling();
                SortingPopup();
            }
            else
            {
                LoadFromResources(name,true);
            }
        }

        private void LoadFromResources(string name, bool isPopup = false)
        {
            const string SCENE_PATH = "Prefab/Scene";
            const string POPUP_PATH = "Prefab/Popup";

            GameObject screen = null;

            if (isPopup)
                screen = Resources.Load<GameObject>(string.Format("{0}/{1}", POPUP_PATH, name));
            else
                screen = Resources.Load<GameObject>(string.Format("{0}/{1}", SCENE_PATH, name));

            if (screen == null)
            {
                Debug.LogError(string.Format("can not load UI '{0}'", name));
                return;
            }

            screen = Instantiate(screen);
            screen.name = name;

            if (isPopup)
                screen.transform.SetParent(GameObject.Find("UIPopup").transform);
            else
                screen.transform.SetParent(GameObject.Find("UIScreen").transform);

            // reset transform info
            screen.GetComponent<RectTransform>().localScale = Vector3.one;
            screen.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            screen.GetComponent<RectTransform>().offsetMin = Vector2.zero;

            _screenList.Add(name, screen.GetComponent<UIScreen>());
            if (isPopup)
                _popupList.Add(screen.GetComponent<UIScreen>());
            else
                _screenStack.Push(screen.GetComponent<UIScreen>());

            SortingPopup();
        }

        public void RegistScreen(string screenName, UIScreen screen)
        {
            if (_screenList.ContainsKey(screenName))
                return;

            _screenList.Add(screenName, screen);
            _screenStack.Push(screen);
        }

        public void RegistPopup(string popupName, UIScreen screen)
        {
            _screenList.Add(popupName, screen);
            screen.gameObject.SetActive(false);
        }

        public void CloseScene()
        {
            UIScreen screen = _screenStack.Pop();
            screen.gameObject.SetActive(false);

            if (_screenStack.Count > 0)
                _screenStack.Peek().gameObject.SetActive(true);
        }

        public void ClosePopup()
        {
            if(_popupList.Count > 0)
            {
                UIScreen popup = _popupList[0];
                popup.gameObject.SetActive(false);
                _popupList.RemoveAt(0);
            }

            if (_popupList.Count > 0)
                _popupList[0].gameObject.SetActive(true);
        }

        public void ClosePopup(UIScreen screen)
        {
            screen.gameObject.SetActive(false);
            _popupList.Remove(screen);
        }


        public void OnBackKey()
        {
            if (_popupList.Count > 0)
            {
                if (_popupList[0].Backkey())
                    return;
            }

            if (_screenStack.Count > 0)
            {
                _screenStack.Peek().Backkey();
            }
        }

        public bool IsAlreadyPopup()
        {
            return _popupList.Count > 0;
        }

        private void SortingPopup()
        {
            _popupList.Sort((tx, ty) => tx.Weight.CompareTo(ty.Weight));
            _popupList.Reverse();
        }

    }
}