using UnityEngine;

namespace GB.UI
{

    /// <remarks>
    /// <copyright file="UIScreen.cs" company="GB">
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
    public enum UI_TYPE { Screen = 0, Popup }
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] UI_TYPE _type = UI_TYPE.Popup;
        public int Weight
        {
            get
            {
                return GetComponent<RectTransform>().GetSiblingIndex();
            }
            set
            {
                GetComponent<RectTransform>().SetSiblingIndex(value);
            }
        }

        public void ShowFirstLayer()
        {
            GetComponent<RectTransform>().SetAsLastSibling();
        }

        void Awake()
        {
            if (_type == UI_TYPE.Popup)
                UIManager.Instance.RegistPopup(this.gameObject.name, this);
            else 
                UIManager.Instance.RegistScreen(this.gameObject.name, this);
        }

        
        
        public void ChangeScene(string sceneName)
        {
            UIManager.Instance.ChangeScene(sceneName);
        }

        virtual public void Close()
        {
            if (_type == UI_TYPE.Popup)
                UIManager.Instance.ClosePopup();
            else
                UIManager.Instance.CloseScene();
        }

        virtual public bool Backkey()
        {
            Close();
            return true;
        }

        virtual public void OnChangeValue(object model)
        {
              
        }



    }
}