using PataRoad.Core.Character.Equipments;
using PataRoad.Core.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    internal class RareponSelection : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        int _index;
        public int Index => _index;
        [SerializeField]
        Image _image;
        RectTransform _imageRect;
        [SerializeField]
        Image _helmImage;
        [SerializeField]
        Image _bodyImage;
        [SerializeField]
        Image _questionImage;
        RareponData _data;

        bool _isOpen;
        public RareponData RareponData => _isOpen ? _data : null;
        private RareponDataContainer _container;
        [SerializeField]
        private Button _button;
        [SerializeField]
        private RectTransform _rectTransform;
        [SerializeField]
        private Vector2 _requirementWindowPivot;
        public Button Button => _button;
        private RareponSelector _parent;

        [Header("Requirement")]
        [SerializeField]
        private RareponSelection[] _nextEnableTarget;
        [SerializeField]
        public MaterialItemRequirement[] _materialRequirements;
        [SerializeField]
        public GemItemRequirement[] _gemRequirements;
        private ItemRequirement[] _totalRequirements;
        private bool _available;
        public bool CanLevelUp => _container == null || _container.CanLevelUp();
        private int _nextLevel => (_container?.Level ?? 0) + 1;
        private bool _invokingParentAdded;

        public void Init(RareponSelector parent)
        {
            if (_data != null) return;
            _imageRect = _image.GetComponent<RectTransform>();
            _container = Core.Global.GlobalData.CurrentSlot.RareponInfo.GetFromOpenRarepon(Index);
            _parent = parent;
            if (_container != null)
            {
                SetRarepon();
                _available = true;
            }
            else
            {
                ShowImages(false);
                _data = Core.Character.Patapons.Data.RareponInfo.GetRareponData(Index, 1);
                _button.enabled = false;
            }
            _totalRequirements = ((ItemRequirement[])_materialRequirements).Concat(_gemRequirements).ToArray();
            if (_available) UpdateRequirements();
        }
        public void EnableIfAvailable()
        {
            if (!_available) return;
            if (!_button.enabled) _button.enabled = true;
            if (!_invokingParentAdded)
            {
                Button.onClick
                    .AddListener(() => _parent.InvokeOnClicked(this));
                _invokingParentAdded = true;
            }
        }
        private void OnEnable()
        {
            if (_available) _button.enabled = true;
        }
        private void OnDisable()
        {
            _button.enabled = false;
        }
        public void Select() => _button.Select();

        private void SetRarepon()
        {
            _data = _container.Data;
            ShowImages(true);
            if (_data.Index != 0)
            {
                _helmImage.enabled = false;
                _bodyImage.color = _data.Color;
                SetPivotAndUpdateImage(_data.Image);
                _image.color = _data.Color;
            }
            foreach (var target in _nextEnableTarget)
            {
                target._available = true;
                target.enabled = true;
                target.EnableIfAvailable();
            }
            _isOpen = true;
        }
        public bool ConfirmToUpgradeRarepon()
        {
            if (_container != null && !_container.CanLevelUp())
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                return false;
            }
            bool isLevelingUp = _container != null;
            //check condition example. And...
            bool itemExists = true;
            List<ItemRequirement> itemRequirements = new List<ItemRequirement>();
            foreach (var itemPair in _totalRequirements)
            {
                if (!Core.Global.GlobalData.CurrentSlot.Inventory.HasAmountOfItem(itemPair.Item, itemPair.Amount))
                {
                    itemExists = false;
                    itemRequirements.Add(itemPair);
                }
            }
            if (!itemExists)
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                var status = string.Join(
                    "\n", itemRequirements.Select(
                        req => $"{req.Item.Name} ({Core.Global.GlobalData.CurrentSlot.Inventory.GetAmount(req.Item)}/{req.Amount})"));

                Common.GameDisplay.ConfirmDialog.Create("The follow items are not enough:\n" + status)
                    .HideOkButton()
                    .SetTargetToResume(_parent)
                    .SelectCancel();
                return false;
            }
            else
            {
                _parent.enabled = false;
                Common.GameDisplay.ConfirmDialog.Create((isLevelingUp) ? $"Level up to {_nextLevel}?" : "Create?")
                    .SetTargetToResume(_parent)
                    .SetOkAction(() => AddThisRarepon(isLevelingUp))
                    .SelectOk();
            }
            return true;
        }
        private void AddThisRarepon(bool levelingUp)
        {
            bool updated = false;
            if (!levelingUp)
            {
                _container = Core.Global.GlobalData.CurrentSlot.RareponInfo.OpenNewRarepon(Index);
                _data = _container.Data;
                updated = _container != null;
            }
            else if (_container.CanLevelUp())
            {
                _container.LevelUp();
                updated = true;
            }
            if (updated)
            {
                SetRarepon();
                Core.Global.GlobalData.Sound.PlayInScene((levelingUp) ? _parent.LevelUpSound : _parent.NewRareponSound);
                foreach (var item in _totalRequirements)
                {
                    Core.Global.GlobalData.CurrentSlot.Inventory.RemoveItem(item.Item, item.Amount);
                }
                UpdateRequirements();
                if (_parent.InventoryRefresher != null) _parent.InventoryRefresher.Refresh();
            }
        }
        internal void LevelUp()
        {
            if (!_container.CanLevelUp()) return;
            _container.LevelUp();
            SetRarepon();
        }
        private void UpdateRequirements()
        {
            foreach (var requirement in _totalRequirements)
            {
                requirement.SetRequirementByLevel(_nextLevel);
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (!_available) return;
            _parent.UpdateText(_data);
            if (!_parent.ShowRequirements) return;
            if (CanLevelUp)
            {
                _parent.RareponRequirementWindow.ShowRequirements(_totalRequirements, _rectTransform, _requirementWindowPivot);
            }
            else if (_parent.RareponRequirementWindow.gameObject.activeSelf)
            {
                _parent.RareponRequirementWindow.HideRequirements();
            }
        }
        private void ShowImages(bool show)
        {
            _helmImage.enabled = show;
            _bodyImage.enabled = show;
            _image.enabled = show;
            _questionImage.enabled = !show;
        }
        private void SetPivotAndUpdateImage(Sprite sprite)
        {
            _image.sprite = sprite;
            _imageRect.sizeDelta = sprite.rect.size / 2;
            _imageRect.anchoredPosition = new Vector2(
                (sprite.rect.width / 2 - sprite.pivot.x) / 2,
                -sprite.pivot.y + 8
                );
        }
    }
}
