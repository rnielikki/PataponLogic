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

        public void ShowSlotData()
        {
            _slotDataLoader.gameObject.SetActive(true);
            _parentSelectorOnMain.enabled = false;
        }

        public void Save(CommonSceneLogic.SlotDataItem slotItem)
        {
            Common.GameDisplay.ConfirmDialog.Create("Do you want to save the data?",
                _parentSelectorOnSave,
                () => SaveAndUpdate(slotItem)
                );
        }
        private void SaveAndUpdate(CommonSceneLogic.SlotDataItem slotItem)
        {
            Core.Global.GlobalData.SlotManager.SaveSlot(slotItem.SlotMeta.SlotIndex);
            slotItem.UpdateDisplay(slotItem.SlotMeta);
        }
        public void LoadMainMenu()
        {
            /*
                Common.GameDisplay.ConfirmDialog.Create("Do you want to save the data?",
                    _parentSelectorOnMain,
                    () => UnityEngine.SceneManagement.SceneManager.LoadScene("Main")
                    )
                */
        }
    }
}
