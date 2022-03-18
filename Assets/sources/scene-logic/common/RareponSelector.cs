using PataRoad.Core.Character.Equipments;
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
        AudioClip _levelUpSound;
        public AudioClip LevelUpSound => _levelUpSound;
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
        [SerializeField]
        private bool _showRequirements;
        public bool ShowRequirements => _showRequirements;

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

        internal void InvokeOnClicked(RareponSelection rarepon) => _onClicked.Invoke(rarepon);
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
                Core.Global.GlobalData.GlobalInputActions
                    .WaitForNextInput(() =>
                    {
                        var selection = _rareponSelections.SingleOrDefault(s => s.Index == index);
                        if (selection != null) selection.Select();
                    })
                );
        }
        public void Apply(RareponSelection selection)
        {
            if (selection.RareponData != null)
            {
                _equipmentSetter.SetEquipment(_targetPatapon, selection.RareponData);
                Close();
            }
            else Core.Global.GlobalData.Sound.PlayBeep();
        }
        public void ApplyAll()
        {
            var currentObj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            RareponSelection selection = null;
            if (currentObj != null) selection = currentObj.GetComponent<RareponSelection>();
            if (selection != null && selection.RareponData != null)
            {
                Common.GameDisplay.ConfirmDialog
                    .Create($"Do you want to set all Patapons in current class with {selection.RareponData.Name}?")
                    .SetTargetToResume(this)
                    .CallOkActionLater()
                    .SetOkAction(ApplyAllRarepons)
                    .SelectCancel();
            }
            else Core.Global.GlobalData.Sound.PlayBeep();
            void ApplyAllRarepons()
            {
                _equipmentSetter.ApplySameRareponForClass(_targetPatapon.Type, selection.RareponData);
                Close();
            }
        }
        public void CreateNewRarepon(RareponSelection selection)
        {
            if (selection.CanLevelUp)
            {
                CreateRarepon(selection);
            }
        }
        private void CreateRarepon(RareponSelection selection) => selection.ConfirmToUpgradeRarepon();

        public void Close()
        {
            _actionEventMap.enabled = false;
            gameObject.SetActive(false);
            Core.Global.GlobalData.Sound.PlaySelected();
            if (_lastSelectNavigator != null) _lastSelectNavigator.Defrost();
            _onClosed?.Invoke();
        }
    }
}
