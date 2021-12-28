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
        RareponData _rareponData;
        public int Index => _rareponData.Index;
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
        public RareponData RareponData => _data;
        [SerializeField]
        private Button _button;
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

        public void Init()
        {
            var data = Core.Global.GlobalData.PataponInfo.RareponInfo.GetRarepon(Index);
            _parent = GetComponentInParent<RareponSelector>();
            if (data != null)
            {
                SetRarepon(data);
            }
            else
            {
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
                    Core.Global.GlobalData.PataponInfo.RareponInfo.OpenNewRarepon(target.Index);
                    target._available = true;
                    target.enabled = true;
                }
            }
            _text.text = data.Name;
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
                if (Core.Global.GlobalData.Inventory.HasAmountOfItem(itemPair.Item, itemPair.Amount))
                {
                    itemExists = false;
                    itemRequirements.Add(itemPair);
                }
            }
            if (!itemExists)
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                var status = string.Join("\n", itemRequirements.Select(req => $"{req.Item} ({Core.Global.GlobalData.Inventory.GetAmount(req.Item)}/{req.Amount})"));
                Common.GameDisplay.ConfirmDialog.CreateCancelOnly("The follow items are not enough:\n" + status);
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
            var rarepon = Core.Global.GlobalData.PataponInfo.RareponInfo.OpenNewRarepon(Index);
            if (rarepon != null)
            {
                SetRarepon(rarepon);
                Core.Global.GlobalData.Sound.PlayInScene(_parent.NewRareponSound);
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            _parent.UpdateText(_data);
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
