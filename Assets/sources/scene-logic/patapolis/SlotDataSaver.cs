using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    class SlotDataSaver : MonoBehaviour
    {
        [SerializeField]
        MonoBehaviour _parentSelectorOnSave;
        [SerializeField]
        MonoBehaviour _parentSelectorOnMain;

        [SerializeField]
        CommonSceneLogic.SlotDataList _slotDataLoader;
        [SerializeField]
        AudioClip _savedSound;

        private bool _saved;

        public void ShowSlotData()
        {
            _slotDataLoader.gameObject.SetActive(true);
            _parentSelectorOnMain.enabled = false;
        }

        public void Save(CommonSceneLogic.SlotDataItem slotItem)
        {
            var dialog = Common.GameDisplay.ConfirmDialog.Create("Do you want to save the data?")
                    .SetTargetToResume(_parentSelectorOnSave)
                    .SetOkAction(() => SaveAndUpdate(slotItem));

            if (Core.Global.Slots.SlotMetaList.HasDataInIndex(slotItem.Index))
            {
                dialog.AppendText("[!] Data exists and you're about to overwrite!");
                dialog.SelectCancel();
            }
            else
            {
                dialog.SelectOk();
            }
            slotItem.ShowHighlight();
        }
        private void SaveAndUpdate(CommonSceneLogic.SlotDataItem slotItem)
        {
            slotItem.MarkAsProcessing();
            var saved = Core.Global.GlobalData.SlotManager.SaveSlot(slotItem.Index);
            slotItem.UpdateDisplay(saved);
            _saved = true;
            Core.Global.GlobalData.Sound.PlayInScene(_savedSound);
            slotItem.MarkAsDone();
        }
        public void MarkAsUnsaved() => _saved = false;
        public void LoadMainMenu()
        {
            if (!_saved)
            {
                Common.GameDisplay.ConfirmDialog.Create("Are you sure to go to the main menu?\nThe play data won't saved.")
                .SetTargetToResume(_parentSelectorOnMain)
                .SetOkAction(() => Common.GameDisplay.SceneLoadingAction.Create("Main").ChangeScene())
                .SelectCancel();
            }
            else
            {
                Common.GameDisplay.SceneLoadingAction.Create("Main").ChangeScene();
            }
        }
    }
}
