using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class ClassMenu : MonoBehaviour
    {
        private ClassSelectionInfo[] _classSelections;

        private void Awake()
        {
            _classSelections = GetComponentsInChildren<ClassSelectionInfo>(true);
        }
        public void UpdateStatus()
        {
            foreach (var selection in _classSelections)
            {
                if (Core.GlobalData.PataponInfo.ContainsClass(selection.ClassType))
                {
                    selection.gameObject.SetActive(false);
                }
                else
                {
                    selection.gameObject.SetActive(true);
                }
            }
        }
    }
}
