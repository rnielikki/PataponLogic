using PataRoad.Core.Character.Equipments.Weapons;
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
        [SerializeField]
        Text _text;
        [SerializeField]
        Image _helmImage;
        [SerializeField]
        Image _bodyImage;
        [SerializeField]
        Image _questionImage;
        RareponData _data;
        bool _isOpen;
        public RareponData RareponData => _isOpen ? _data : null;
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

        public void Init(RareponSelector parent)
        {
            if (_data != null) return;
            var data = Core.Global.GlobalData.CurrentSlot.PataponInfo.RareponInfo.GetFromOpenRarepon(Index);
            _parent = parent;
            if (data != null)
            {
                SetRarepon(data);
                _available = true;
            }
            else
            {
                _data = Core.Global.GlobalData.CurrentSlot.PataponInfo.RareponInfo.LoadResourceWithoutOpen(Index);
                ShowImages(false);
                _available = false;
                _button.enabled = false;
                _totalRequirements = ((ItemRequirement[])_materialRequirements).Concat(_gemRequirements).ToArray();
            }
        }
        public void EnableIfAvailable()
        {
            if (_available && !_button.enabled) _button.enabled = true;
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

        private void SetRarepon(RareponData data)
        {
            _data = data;
            ShowImages(true);
            if (data.Index != 0)
            {
                _helmImage.enabled = false;
                _bodyImage.color = data.Color;
                _image.sprite = data.Image;
                _image.color = data.Color;
            }
            if (_nextEnableTarget.Length > 0)
            {
                foreach (var target in _nextEnableTarget)
                {
                    target._available = true;
                    target.enabled = true;
                }
            }
            _text.text = data.Name;
            _isOpen = true;
        }
        public bool ConfirmToCreateRarepon()
        {
            if (_totalRequirements == null)
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                return false;
            }

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
                var status = string.Join("\n", itemRequirements.Select(req => $"{req.Item.Name} ({Core.Global.GlobalData.CurrentSlot.Inventory.GetAmount(req.Item)}/{req.Amount})"));
                Common.GameDisplay.ConfirmDialog.CreateCancelOnly("The follow items are not enough:\n" + status, targetToResume: _parent);
                return false;
            }
            else
            {
                _parent.enabled = false;
                Common.GameDisplay.ConfirmDialog.Create("Create?", _parent, AddThisRarepon);
            }
            return true;
        }
        private void AddThisRarepon()
        {
            var rarepon = Core.Global.GlobalData.CurrentSlot.PataponInfo.RareponInfo.OpenNewRarepon(Index);
            if (rarepon != null)
            {
                SetRarepon(rarepon);
                Core.Global.GlobalData.Sound.PlayInScene(_parent.NewRareponSound);
                foreach (var item in _totalRequirements)
                {
                    Core.Global.GlobalData.CurrentSlot.Inventory.RemoveItem(item.Item, item.Amount);
                }
                _parent.InventoryRefresher?.Refresh();
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (_data == null) return;
            _parent.UpdateText(_data);
            if (_available && !_isOpen)
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
    }
}
