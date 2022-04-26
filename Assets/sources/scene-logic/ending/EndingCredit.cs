using TMPro;
using UnityEngine;

namespace PataRoad.SceneLogic.Ending
{
    public class EndingCredit : MonoBehaviour
    {
        [SerializeField]
        EndingCreditData[] _data;
        [SerializeField]
        TextMeshProUGUI _bigTitle;
        [SerializeField]
        TextMeshProUGUI _creditTitle1;
        [SerializeField]
        TextMeshProUGUI _credit1;
        [SerializeField]
        TextMeshProUGUI _creditTitle2;
        [SerializeField]
        TextMeshProUGUI _credit2;

        [SerializeField]
        float _displaySeconds;
        [SerializeField]
        float _fadingSeconds;
        float _currentSecond;
        bool _started;
        bool _fullyDisplayed;

        readonly System.Collections.Generic.List<TextMeshProUGUI> _fadingTarget
            = new System.Collections.Generic.List<TextMeshProUGUI>();

        private int _currentIndex = -1;
        private EndingCreditData _currentData => _currentIndex < 0 ? null : _data[_currentIndex];
        float _currentShowingSeconds;

        private Story.StoryLoader _storyLoader;

        private void Start()
        {
            _storyLoader = FindObjectOfType<Story.StoryLoader>();

            _creditTitle1.gameObject.SetActive(false);
            _creditTitle2.gameObject.SetActive(false);
            _credit1.gameObject.SetActive(false);
            _credit2.gameObject.SetActive(false);
            _bigTitle.gameObject.SetActive(false);
            DisplayCredit();
        }
        private void DisplayCredit()
        {
            _started = true;
        }
        private void DisplayCurrent()
        {
            _fadingTarget.Clear();
            _currentData.CallDisplayEvent();
            if (_currentData.UseBigTitle)
            {
                SetTextAndAddTarget(_bigTitle, _currentData.BigTitleText);
            }
            else
            {
                if (!_currentData.HideCredit1)
                {
                    SetTextAndAddTarget(_creditTitle1, _currentData.Title1);
                    SetTextAndAddTarget(_credit1, _currentData.Content1);
                }
                if (!_currentData.HideCredit2)
                {
                    SetTextAndAddTarget(_creditTitle2, _currentData.Title2);
                    SetTextAndAddTarget(_credit2, _currentData.Content2);
                }
            }
            Fade(0);

            void SetTextAndAddTarget(TextMeshProUGUI target, string text)
            {
                target.gameObject.SetActive(true);
                target.text = text;
                _fadingTarget.Add(target);
            }
        }
        private void EndDisplaying()
        {
            _started = false;
            //-- Move to the next map
            Common.GameDisplay.SceneLoadingAction.ChangeScene("Patapolis", true);
            //-- Story done, you don't need this anymore!
        }
        // coroutine is enemy of sync.
        void Update()
        {
            if (!_started) return;
            _currentSecond += Time.deltaTime;
            _currentShowingSeconds += Time.deltaTime;
            var index = (int)(_currentSecond / _displaySeconds);
            if (_currentIndex != index)
            {
                Hide();
                _currentShowingSeconds = 0;
                _currentData?.CallHidingEvent();
                if (index < _data.Length)
                {
                    _currentIndex = index;
                    DisplayCurrent();
                }
                else EndDisplaying();
            }
            else if (_currentShowingSeconds < _fadingSeconds)
            {
                _fullyDisplayed = false;
                //fade in
                Fade(_currentShowingSeconds / _fadingSeconds);
            }
            else if (_displaySeconds - _currentShowingSeconds < _fadingSeconds)
            {
                _fullyDisplayed = false;
                //fade out
                Fade((_displaySeconds - _currentShowingSeconds) / _fadingSeconds);
            }
            else if (!_fullyDisplayed)
            {
                _fullyDisplayed = true;
                //keep opacity 1
                Fade(1);
            }
        }
        private void Fade(float opacity)
        {
            foreach (var target in _fadingTarget)
            {
                var color = target.color;
                color.a = opacity;
                target.color = color;
            }
        }
        private void Hide()
        {
            foreach (var target in _fadingTarget)
            {
                target.gameObject.SetActive(false);
            }
        }
        private void OnDestroy()
        {
            if (_storyLoader != null) _storyLoader.ForceDestroy();
        }
    }
}