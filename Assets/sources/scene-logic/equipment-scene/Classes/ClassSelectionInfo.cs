using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Patapons;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class ClassSelectionInfo : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        private ClassType _classType;
        public ClassType ClassType => _classType;
        private GameObject _generalObject;
        public string GeneralName { get; private set; }

        [SerializeField]
        [TextArea]
        private string _classDescription;
        public string ClassDescription => _classDescription;
        [SerializeField]
        [TextArea]
        private string _selfEffectDescription;
        public string SelfEffectDescription => _selfEffectDescription;

        [SerializeField]
        private ClassSelectionUpdater _updater;

        [SerializeField]
        [TextArea]
        private string _groupEffectDescription;

        public string GroupEffectDescription => _groupEffectDescription;
        private void Awake()
        {
            _generalObject = PataponGroupGenerator.GetGeneralObject(_classType);
            GeneralName = _generalObject.GetComponent<Core.Character.Patapons.General.PataponGeneral>().GeneralName;
        }
        public GameObject GetGeneralObject()
        {
            var obj = Instantiate(_generalObject);
            obj.GetComponent<Core.Character.PataponData>().enabled = false;
            foreach (var eq in obj.GetComponentsInChildren<Core.Character.Equipments.Equipment>())
            {
                eq.enabled = false;
            }
            return obj;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _updater.UpdateDescription(this);
        }
    }
}
