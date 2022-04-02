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
        private MainMenuSelection _lastSelected;
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
                button.Button.onClick.AddListener(() => PauseNavigation(button));
            }
        }
        public void PauseNavigation(MainMenuSelection lastSelected)
        {
            _lastSelected = lastSelected;
            foreach (var button in _buttons)
            {
                button.Button.enabled = false;
                button.enabled = false;
            }
        }
        public void ResumeNavigation()
        {
            enabled = true;
            foreach (var button in _buttons)
            {
                button.Button.enabled = true;
                button.enabled = true;
            }
            _lastSelected.Button.Select();
            _lastSelected.OnSelect(null);
        }
        public void LoadGameIntro()
        {
            Core.Global.GlobalData.SlotManager.LoadSlot(Slot.CreateNewGame());
            Common.GameDisplay.SceneLoadingAction.ChangeScene("GameIntro");
        }
        public void LoadFromSaved(CommonSceneLogic.SlotDataItem item)
        {
            if (!SlotMetaList.HasDataInIndex(item.Index))
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                return;
            }
            item.MarkAsProcessing();
            Core.Global.GlobalData.SlotManager.LoadSlot(Slot.LoadSlot(item.Index));
            Common.GameDisplay.SceneLoadingAction.ChangeScene("Patapolis");
            item.MarkAsDone();
        }
        public void Exit()
        {
            Application.Quit();
        }
    }
}
