using PataRoad.Core.Character.Class;
using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class ClassMenu : MonoBehaviour
    {
        private ClassSelectionInfo[] _classSelections;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onOpenClass;
        private ClassType[] _availableClasses;

        private void Awake()
        {
            _availableClasses = Core.GlobalData.Inventory
                .GetKeyItems<Core.Items.ClassMemoryData>("Class")
                .Select(item => item.Class).ToArray();
            _classSelections = GetComponentsInChildren<ClassSelectionInfo>(true);

            foreach (var classSelection in _classSelections)
            {
                if (_availableClasses.Contains(classSelection.ClassType)) classSelection.Init();
            }
        }
        private void OnEnable()
        {
            UpdateStatus();
        }
        public void UpdateStatus()
        {
            foreach (var selection in _classSelections)
            {
                if (Core.GlobalData.PataponInfo.ContainsClass(selection.ClassType) || !_availableClasses.Contains(selection.ClassType))
                {
                    selection.gameObject.SetActive(false);
                }
                else
                {
                    selection.gameObject.SetActive(true);
                }
            }
        }
        public void OpenClass() => _onOpenClass.Invoke();
        private void OnDestroy()
        {
            _onOpenClass.RemoveAllListeners();
        }
    }
}
