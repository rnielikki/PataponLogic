using PataRoad.Core.Global.Slots;
using UnityEngine;

namespace PataRoad.SceneLogic.Main
{
    class MainMenuSelector : MonoBehaviour
    {
        [SerializeField]
        private MainMenuSelection _newGameButton;
        [SerializeField]
        private MainMenuSelection _continueButton;
        private MainMenuSelection[] _buttons;
        private void Start()
        {
            if (SlotMetaList.HasSave)
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                _continueButton.Button.Select();
                _continueButton.OnSelect(null);
            }
            else
            {
                _continueButton.gameObject.SetActive(false);
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                _newGameButton.Button.Select();
                _newGameButton.OnSelect(null);
            }
            _buttons = GetComponentsInChildren<MainMenuSelection>(false);
            foreach (var button in _buttons)
            {
                button.Button.onClick.AddListener(() => enabled = false);
            }
        }
        public void OnDisable()
        {
            foreach (var button in _buttons)
            {
                button.Button.enabled = false;
            }
        }
        public void OnEnable()
        {
            if (_buttons == null) return;
            foreach (var button in _buttons)
            {
                button.Button.enabled = true;
            }
        }
        public void LoadGameIntro()
        {
            Core.Global.GlobalData.SlotManager.LoadSlot(Slot.CreateNewGame());
            Common.GameDisplay.SceneLoadingAction.Create("GameIntro").ChangeScene();
        }
        public void LoadFromSaved(CommonSceneLogic.SlotDataItem item)
        {
            if (!SlotMetaList.HasDataInIndex(item.Index))
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                return;
            }
            Core.Global.GlobalData.SlotManager.LoadSlot(Slot.LoadSlot(item.Index));
            Common.GameDisplay.SceneLoadingAction.Create("Patapolis").ChangeScene();
        }
        public void Exit()
        {
            Application.Quit();
        }
    }
}
