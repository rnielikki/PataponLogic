using PataRoad.Core.Items;
using PataRoad.SceneLogic.CommonSceneLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class MaterialLoader : MonoBehaviour
    {
        [SerializeField]
        Text _titleField;
        [SerializeField]
        Transform _content;
        [SerializeField]
        GameObject _template;
        Sprite _defaultImage;
        [SerializeField]
        Sprite _selectedImage;
        [SerializeField]
        Color _selectedColor;
        private ItemDisplay _current;
        private ItemDisplay[] _allDisplays;
        public IEnumerable<ItemDisplay> AllDisplays => _allDisplays;
        private System.Guid[] _itemIds;
        public IItem Item => _current?.Item;
        private MinigameMaterialWindow _parent;
        public string Group { get; private set; }
        private bool _lateInitDone;

        public void Init(string materialGroup, MinigameMaterialWindow parent)
        {
            var allData = Core.Global.GlobalData.CurrentSlot.Inventory.GetItemsByType(ItemType.Material, materialGroup)
                .OrderBy(d => d.Item.Index);
            _parent = parent;
            Group = materialGroup;

            foreach (var data in allData)
            {
                var obj = Instantiate(_template, _content);
                var display = obj.GetComponent<ItemDisplay>();
                display.Init(data.Item, data.Amount);
                (display.Selectable as Button).onClick.AddListener(() => Use(display));
                _current = display;
            }
            _allDisplays = GetComponentsInChildren<ItemDisplay>();
            _itemIds = _allDisplays.Select(disp => disp.Item.Id).ToArray();
        }
        public void LateInit()
        {
            if (_current != null)
            {
                _defaultImage = _current.GetComponent<Image>().sprite;
                Use(_current);
            }
            else _titleField.text = $"{Group} (No item)";
            _lateInitDone = true;
        }

        private void UpdateText(IItem item)
        {
            _titleField.text = $"{item.Name} (Level {item.Index + 1})";
        }
        /// <summary>
        /// Select an item. Also updates all others' material amount status by sending message to parent.
        /// </summary>
        /// <param name="itemDisplay">The display of the item that will use.</param>
        private void Use(ItemDisplay itemDisplay)
        {
            if (_lateInitDone)
            {
                if (itemDisplay == _current) return;
                if (_current != null)
                {
                    _current.Background.sprite = _defaultImage;
                    _current.Background.color = Color.white;

                    _parent.RestoreOne(_current.Item);
                }
            }
            else if (itemDisplay.Amount == 0)
            {
                var target = GetNextSelectionTarget(itemDisplay.Item);
                if (target != null) itemDisplay = target;
                else return;
            }
            UpdateText(itemDisplay.Item);
            itemDisplay.Background.sprite = _selectedImage;
            itemDisplay.Background.color = _selectedColor;
            _parent.RemoveOne(itemDisplay.Item);
            _current = itemDisplay;

            if (_lateInitDone)
            {
                if (_current.Amount == 0)
                {
                    var nextTarget = GetNextSelectionTarget(itemDisplay.Item);
                    if (nextTarget == null) _parent.FindNextSelectionTarget(this);
                    else nextTarget.Selectable.Select();
                }
                _parent.UpdateEstimation();
            }
        }
        internal void UpdateAmount(IItem item, bool remove = true)
        {
            var index = System.Array.IndexOf(_itemIds, item.Id);
            if (index == -1) return;
            var display = _allDisplays[index];
            display.UpdateText(display.Amount + (remove ? -1 : 1));
            if (remove && display.Amount == 0)
            {
                display.MarkAsDisable();
            }
            else if (!remove && display.Amount == 1)
            {
                display.MarkAsEnable();
            }
        }
        private ItemDisplay GetNextSelectionTarget(IItem item)
        {
            var index = System.Array.IndexOf(_itemIds, item.Id);
            if (index < 0) return null;
            else if (index == 0)
            {
                for (int i = 0; i < _allDisplays.Length; i++)
                {
                    if (_allDisplays[i].Amount != 0) return _allDisplays[i];
                }
                return null;
            }
            else
            {
                return GetNextSelectionTarget();
            }
        }
        internal ItemDisplay GetNextSelectionTarget()
        {
            for (int i = _allDisplays.Length - 1; i >= 0; i--)
            {
                if (_allDisplays[i].Amount != 0) return _allDisplays[i];
            }
            return null;
        }
    }
}
