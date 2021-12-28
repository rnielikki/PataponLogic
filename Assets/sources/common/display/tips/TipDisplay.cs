using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Common.GameDisplay
{
    public class TipDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _titleText;
        [SerializeField]
        private UnityEngine.UI.Text _contentText;
        [SerializeField]
        private UnityEngine.UI.Image _image;
        [SerializeField]
        private UnityEngine.UI.Text _loadingStatus;
        [SerializeField]
        private AudioClip _tipSound;
        private static Dictionary<int, TipDisplayData> _allTipsIndex { get; set; }
        private static TipDisplayData[] _allTips { get; set; }
        private InputAction _action;
        private AsyncOperation _op;
        private bool _fadingCreated;

        private bool _loadingCompleteCalled;

        // Start is called before the first frame update
        void Start()
        {
            _action = Core.Global.GlobalData.Input.actions.FindAction("UI/Submit");

            _action.Enable();
            if (_allTipsIndex == null)
            {
                _allTips = Resources.LoadAll<TipDisplayData>("Tips/Content");
                _allTipsIndex = new Dictionary<int, TipDisplayData>();
                foreach (var tip in _allTips)
                {
                    if (int.TryParse(tip.name, out int index))
                    {
                        _allTipsIndex.Add(index, tip);
                    }
                }
            }

            var tipindex = Core.Global.GlobalData.TipIndex;
            if (tipindex > -1 && _allTipsIndex.TryGetValue(tipindex, out TipDisplayData data))
            {
                LoadTip(data);
            }
            else
            {
                LoadTip(_allTips[Random.Range(0, _allTips.Length - 1)]);
            }
            Core.Global.GlobalData.TipIndex = -1;
        }
        private void LoadTip(TipDisplayData data)
        {
            _titleText.text = data.Title;
            _contentText.text = data.Content;
            if (data.Image != null)
            {
                _image.sprite = data.Image;
                _image.enabled = true;
            }
            else
            {
                _image.enabled = false;
            }
        }
        public void LoadNextScene(string sceneName)
        {

            StartCoroutine(WaitForScene());
            System.Collections.IEnumerator WaitForScene()
            {
                yield return null;
                var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
                loading.allowSceneActivation = false;
                _op = loading;
                while (!loading.isDone)
                {
                    if (loading.progress >= 0.9f && !_loadingCompleteCalled)
                    {
                        OnLoadingCompleted();
                        _loadingCompleteCalled = true;
                    }
                    else if (loading.progress < 0.9f)
                    {
                        _loadingStatus.text = _op.progress * 100 + "% loaded...";
                    }
                    yield return null;
                }
            }
        }

        private void OnLoadingCompleted()
        {
            _loadingStatus.text = "Press Enter or submit to continue";
            _action.performed += GoToNextScene;
            _action.Enable();

            Core.Global.GlobalData.Sound.PlayGlobal(_tipSound);
        }
        private void GoToNextScene(InputAction.CallbackContext context)
        {
            _action.performed -= GoToNextScene;
            _action.Disable();
            _action = null;
            _op.allowSceneActivation = true;
            if (!_fadingCreated)
            {
                ScreenFading.Create(true);
                _fadingCreated = true;
            }
        }
        private void OnDestroy()
        {
            if (_action != null)
            {
                _action.performed -= GoToNextScene;
                _action.Disable();
            }
        }
    }
}
