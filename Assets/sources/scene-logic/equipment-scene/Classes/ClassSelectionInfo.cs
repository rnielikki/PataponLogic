using PataRoad.Core.Character;
using PataRoad.Core.Character.Class;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class ClassSelectionInfo : MonoBehaviour, ISelectHandler
    {

        [SerializeField]
        CharacterGroupSaver _groupDataSaver;
        public GameObject GroupObject { get; private set; }

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

        public Stat StatAverage { get; private set; }
        public float MassAverage { get; private set; }

        void Start()
        {
            GroupObject = _groupDataSaver.GetGroup(_classType);

            var general = GroupObject.GetComponentInChildren<Core.Character.Patapons.General.PataponGeneral>(true);
            GeneralObject = Instantiate(general.gameObject);
            var generalData = GeneralObject.GetComponent<PataponData>();
            generalData.DisableAllEquipments();
            generalData.enabled = false;

            GeneralName = general.GeneralName;
            GeneralObject.SetActive(false);
            CalculateData();
        }
        private void CalculateData()
        {
            var data = GroupObject.GetComponentsInChildren<PataponData>();
            StatAverage = Stat.GetMidValue(data.Select(p => p.Stat));
            MassAverage = data.Average(p => p.Rigidbody.mass);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _updater.UpdateDescription(this);
        }
    }
}
