using PataRoad.Core.Character.Equipments;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentSummaryElement : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        Color _selectedColor;
        Color _notSelectedColor;

        Image _bg;
        Text _text;
        [SerializeField]
        private bool _isGeneralMode;
        public bool IsGeneralMode => _isGeneralMode;
        [SerializeField]
        private EquipmentType _equipmentType;
        public EquipmentType EquipmentType => _equipmentType;

        void Awake()
        {
            _bg = GetComponent<Image>();
            _text = GetComponentInChildren<Text>();
            _notSelectedColor = _bg.color;
        }
        public void SetText(string text) => _text.text = text;
        public void MarkAsDeselected()
        {
            _bg.color = _notSelectedColor;
        }
        public void MarkAsSelected()
        {
            _bg.color = _selectedColor;
        }
    }
}
