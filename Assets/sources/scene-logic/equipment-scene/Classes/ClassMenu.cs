using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class ClassMenu : MonoBehaviour
    {
        private ClassSelectionInfo[] _classSelections;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onOpenClass;

        private void Awake()
        {
            _classSelections = GetComponentsInChildren<ClassSelectionInfo>(true);

            foreach (var classSelection in _classSelections)
            {
                if (CharacterGroupSaver.AvailableClasses.Contains(classSelection.ClassType)) classSelection.Init();
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
                if (Core.Global.GlobalData.PataponInfo.ContainsClass(selection.ClassType) || !CharacterGroupSaver.AvailableClasses.Contains(selection.ClassType))
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
