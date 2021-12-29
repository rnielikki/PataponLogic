using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.SceneLogic.EquipmentScene;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    internal class RareponSelector : MonoBehaviour
    {
        [SerializeField]
        UnityEvent _onOpen;
        [SerializeField]
        UnityEvent<RareponSelection> _onClicked;
        [SerializeField]
        UnityEvent _onClosed;
        [SerializeField]
        AudioClip _newRareponSound;
        public AudioClip NewRareponSound => _newRareponSound;
        private Core.Character.PataponData _targetPatapon;
        private Common.Navigator.SpriteNavigator _lastSelectNavigator;

        [SerializeField]
        private RareponRequirementWindow _rareponRequirementWindow;
        internal RareponRequirementWindow RareponRequirementWindow => _rareponRequirementWindow;
        [SerializeField]
        private EquipmentSetter _equipmentSetter;
        private RareponSelection[] _rareponSelections;
        [SerializeField]
        private Common.Navigator.ActionEventMap _actionEventMap;
        [SerializeField]
        private Patapolis.InventoryRefresher _inventoryRefresher;
        public Patapolis.InventoryRefresher InventoryRefresher => _inventoryRefresher;

        [Header("Description")]
        [SerializeField]
        private Text _descriptionTitle;
        [SerializeField]
        private Text _descriptionContent;

        private void Init()
        {
            _rareponSelections = GetComponentsInChildren<RareponSelection>();
            foreach (var rarepon in _rareponSelections)
            {
                rarepon.Init(this);
            }
            foreach (var rarepon in _rareponSelections)
            {
                rarepon.EnableIfAvailable();
            }
            _rareponRequirementWindow.Init();
        }

        public void Open(Common.Navigator.SpriteNavigator beforeSelect, Core.Character.PataponData pataponData, RareponData data)
        {
            if (_rareponSelections == null) Init();
            _targetPatapon = pataponData;
            _lastSelectNavigator = beforeSelect;
            gameObject.SetActive(true);
            beforeSelect.Freeze();
            Open(data);
        }
        public void Open(RareponData data)
        {
            if (_rareponSelections == null) Init();
            _actionEventMap.enabled = true;
            foreach (var rareponSelection in _rareponSelections)
            {
                rareponSelection.Button
                    .onClick
                    .AddListener(() => _onClicked.Invoke(rareponSelection));
            }
            _onOpen.Invoke();
            gameObject.SetActive(true);
            Select(data);
        }
        public void UpdateText(RareponData data)
        {
            _descriptionTitle.text = data.Name;
            _descriptionContent.text = data.Description;
        }
        public void Select(RareponData data)
        {
            var index = data.Index;
            StartCoroutine(
                Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() => _rareponSelections.SingleOrDefault(s => s.Index == index)?.Select())
                );
        }
        public void Apply(RareponSelection selection)
        {
            if (selection.RareponData == null)
            {
                CreateRarepon(selection);
            }
            else
            {
                _equipmentSetter.SetEquipment(_targetPatapon, selection.RareponData);
                Close();
            }
        }
        public void CreateNewRarepon(RareponSelection selection)
        {
            if (selection.RareponData == null)
            {
                CreateRarepon(selection);
            }
        }
        private void CreateRarepon(RareponSelection selection) => selection.ConfirmToCreateRarepon();

        public void Close()
        {
            _actionEventMap.enabled = false;
            gameObject.SetActive(false);
            Core.Global.GlobalData.Sound.PlaySelected();
            _lastSelectNavigator?.Defrost();
            _onClosed?.Invoke();
        }
    }
}
