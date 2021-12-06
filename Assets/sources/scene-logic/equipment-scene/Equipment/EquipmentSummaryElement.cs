using PataRoad.Core.Character.Equipments;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentSummaryElement : SummaryElement
    {
        [SerializeField]
        private bool _isGeneralMode;
        public bool IsGeneralMode => _isGeneralMode;
        [SerializeField]
        private EquipmentType _equipmentType;
        public EquipmentType EquipmentType => _equipmentType;

        void Awake()
        {
            Init();
        }
        public void SetText(string text) => _text.text = text;
    }
}
