using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GB.Global;

namespace GB.UI
{
   public class UIManager : MonoBehaviour
    {
        
        const HideFlags Flags = HideFlags.HideInHierarchy | HideFlags.DontSave;
        public static UIManager Instance => Application.isPlaying ? ComponentSingleton<UIManager>.Get(Flags) : null;
        
        private Dictionary<string, UIScreen> _screenList = new Dictionary<string, UIScreen>();

        private Stack<UIScreen> _screenStack = new Stack<UIScreen>();
        private Stack<UIScreen> _popupStack = new Stack<UIScreen>();


        public void Clear()
        {
            _screenList.Clear();
            _screenStack.Clear();
            _popupStack.Clear();
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
            // 띄워주기 전 이전 UI 를 닫음
            _screenStack.Peek().gameObject.SetActive(false);

            if (_screenList.ContainsKey(name))
            {
                _screenList[name].gameObject.SetActive(true);       // UI 활성화
                _screenStack.Push(_screenList[name]);
            }
            else
            {
                LoadFromResources(name, extraValue);
            }
        }

        public void ShowPopup(string name, int extraValue = 0, bool isSwap = true)
        {
            if (_popupStack.Count > 0)
            {
                if (_popupStack.Peek() == null)
                {
                    _popupStack.Clear();
                    ShowPopup(name);
                    return;
                }


                if (_popupStack.Peek().name.Equals(name))
                {
                    _popupStack.Peek().SetExtraValue(extraValue);
                    return;
                }

                if (isSwap)
                    _popupStack.Peek().gameObject.SetActive(false);
            }

            if (_screenList.ContainsKey(name))
            {
                _screenList[name].gameObject.SetActive(true);  
                _screenList[name].SetExtraValue(extraValue);
                _popupStack.Push(_screenList[name]);
                _screenList[name].GetComponent<RectTransform>().SetAsLastSibling();
            }
            else
            {
                LoadFromResources(name, extraValue, true);
            }
        }

        private void LoadFromResources(string name, int extraValue, bool isPopup = false)
        {
            const string SCENE_PATH = "Prefab/Scene";
            const string POPUP_PATH = "Prefab/Popup";

            GameObject screen = null;

            if (isPopup)
                screen = Resources.Load<GameObject>(string.Format("{0}/{1}", POPUP_PATH, name));
            else
                screen = Resources.Load<GameObject>(string.Format("{0}/{1}", SCENE_PATH, name));

            // 로드에 실패한 경우 아무것도 하지 않음
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

            // set extra value
            screen.GetComponent<UIScreen>().SetExtraValue(extraValue);

            _screenList.Add(name, screen.GetComponent<UIScreen>());
            if (isPopup)
                _popupStack.Push(screen.GetComponent<UIScreen>());
            else
                _screenStack.Push(screen.GetComponent<UIScreen>());

        }

        public void RegistScreen(string screenName, UIScreen screen)
        {
            Debug.Log("screenName : " + screenName);


            if (_screenList.ContainsKey(screenName))
                return;

            _screenList.Add(screenName, screen);
            _screenStack.Push(screen);
        }

        public void RegistPopup(string popupName, UIScreen screen)
        {
            Debug.Log("popupName : " + popupName);
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
            UIScreen popup = _popupStack.Pop();
            popup.gameObject.SetActive(false);

            if (_popupStack.Count > 0)
                _popupStack.Peek().gameObject.SetActive(true);
        }

        public void OnBackKey()
        {
            if (_popupStack.Count > 0)
            {
                if (_popupStack.Peek().Backkey())
                    return;
            }

            if (_screenStack.Count > 0)
            {
                _screenStack.Peek().Backkey();
            }
        }

        public bool IsAlreadyPopup()
        {
            return _popupStack.Count > 0;
        }

    }
}