using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    internal class MinigameSelectionWindow : MonoBehaviour
    {
        [SerializeField]
        Common.Navigator.ActionEventMap _parentActionEventMap;
        [SerializeField]
        MinigameMaterialWindow _materialWindow;
        [SerializeField]
        Button _firstButton;
        [SerializeField]
        AudioClip _openSound;
        [SerializeField]
        AudioClip _closeSound;
        private MinigameSelectionButton[] _buttons;
        private bool _opening;

        private void Start()
        {
            _buttons = GetComponentsInChildren<MinigameSelectionButton>();
        }
        public void Open()
        {
            _opening = true;
            _parentActionEventMap.enabled = false;
            gameObject.SetActive(true);
            Core.Global.GlobalData.Sound.PlayInScene(_openSound);
            StartCoroutine(
            Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() =>
            {
                foreach (var obj in _buttons)
                {
                    obj.GetComponent<Button>().onClick.AddListener(() => _materialWindow.Open(this, obj));
                }
                _firstButton.Select();
                foreach (var selectHandler in _firstButton.GetComponents<UnityEngine.EventSystems.ISelectHandler>())
                {
                    selectHandler.OnSelect(null);
                }
                _opening = false;
            }));
        }

        public void Close(bool forMinigame = false)
        {
            if (_opening) return;
            if (!forMinigame) Core.Global.GlobalData.Sound.PlayInScene(_closeSound);
            foreach (var obj in _buttons)
            {
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            gameObject.SetActive(false);
            if (!forMinigame) _parentActionEventMap.enabled = true;
        }
    }
}
