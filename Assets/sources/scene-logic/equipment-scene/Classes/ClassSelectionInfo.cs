using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Patapons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class ClassSelectionInfo : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        private ClassType _classType;
        public ClassType ClassType => _classType;
        public GameObject GeneralObject { get; private set; }
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

        public Core.Character.Stat StatAverage { get; private set; }
        public float MassAverage { get; private set; }
        private IEnumerable<Core.Character.PataponData> _members;

        private void Awake()
        {
            GeneralObject = GetGeneralObject();
            GeneralName = GeneralObject.GetComponent<Core.Character.Patapons.General.PataponGeneral>().GeneralName;
            if (_members == null) _members = PataponGroupGenerator.GetGroupMembers(_classType);
        }
        private GameObject GetGeneralObject()
        {
            var obj = Instantiate(PataponGroupGenerator.GetGeneralObject(_classType));
            var data = obj.GetComponent<Core.Character.PataponData>();
            data.enabled = false;
            foreach (var eq in obj.GetComponentsInChildren<Core.Character.Equipments.Equipment>())
            {
                eq.enabled = false;
            }
            obj.SetActive(false);
            return obj;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _updater.UpdateDescription(this);
        }
    }
}
