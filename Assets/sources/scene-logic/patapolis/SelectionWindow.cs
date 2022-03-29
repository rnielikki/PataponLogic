using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Patapolis
{
    public abstract class SelectionWindow : MonoBehaviour
    {
        [SerializeField]
        PatapolisSelector _parentSelector;

        [SerializeField]
        AudioClip _openSound;
        [SerializeField]
        AudioClip _closeSound;
        [SerializeField]
        Button _firstButton;
        private bool _opening;

        public void Open()
        {
            _opening = true;
            _parentSelector.enabled = false;
            gameObject.SetActive(true);
            Core.Global.GlobalData.Sound.PlayInScene(_openSound);
            StartCoroutine(
            Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() =>
            {
                InitButtons();
                _firstButton.Select();
                foreach (var selectHandler in _firstButton.GetComponents<UnityEngine.EventSystems.ISelectHandler>())
                {
                    selectHandler.OnSelect(null);
                }
                _opening = false;
            }));
        }
        protected abstract void InitButtons();
        protected abstract void ResetButtons();

        public void Close(bool forNextWindow = false)
        {
            if (_opening) return;
            if (!forNextWindow) Core.Global.GlobalData.Sound.PlayInScene(_closeSound);
            ResetButtons();
            gameObject.SetActive(false);
            if (!forNextWindow) _parentSelector.enabled = true;
        }
    }
}