using UnityEngine;

namespace PataRoad.SceneLogic.Ending
{
    [System.Serializable]
    public class EndingCreditData
    {
        [Header("Events")]
        [SerializeField]
        UnityEngine.Events.UnityEvent _onDisplay;
        [SerializeField]
        UnityEngine.Events.UnityEvent _onHidden;
        [Header("BigTitle")]
        [SerializeField]
        bool _useBigTitle;
        public bool UseBigTitle => _useBigTitle;
        [SerializeField]
        [TextArea]
        string _bigTitleText;
        public string BigTitleText => _bigTitleText;
        [Header("Credit1")]
        [SerializeField]
        bool _hide1;
        public bool HideCredit1 => _hide1;
        [SerializeField]
        string _title1;
        public string Title1 => _title1;
        [SerializeField]
        [TextArea]
        string _content1;
        public string Content1 => _content1;
        [Header("Credit2")]
        [SerializeField]
        bool _hide2;
        public bool HideCredit2 => _hide2;
        [SerializeField]
        string _title2;
        public string Title2 => _title2;
        [SerializeField]
        [TextArea]
        string _content2;
        public string Content2 => _content2;
        public void CallDisplayEvent() => _onDisplay.Invoke();
        public void CallHidingEvent() => _onHidden.Invoke();
    }
}