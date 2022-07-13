using UnityEngine;

namespace GB.UI
{
    public enum UI_TYPE { Screen = 0, Popup }
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] UI_TYPE _type = UI_TYPE.Popup;

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

        /// <summary>
        /// 다른팝업 또는 화면에 백키를 적용 시키지 않는 경우 true 를 return
        /// </summary>
        virtual public bool Backkey()
        {
            Close();
            return true;
        }

        virtual public void SetExtraValue(int extraValue)
        {

        }



    }
}