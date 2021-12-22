using PataRoad.Core.Items;
using PataRoad.SceneLogic.CommonSceneLogic;
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
        ItemDisplay _template;
        Sprite _defaultImage;
        [SerializeField]
        Sprite _selectedImage;
        [SerializeField]
        Color _selectedColor;
        private ItemDisplay _current;
        private ItemDisplay[] _allDisplays;
        public System.Collections.Generic.IEnumerable<ItemDisplay> AllDisplays => _allDisplays;
        public IItem Item => _current.Item;
        private MinigameMaterialWindow _parent;
        public string Group { get; private set; }

        public void Init(string materialGroup, MinigameMaterialWindow parent)
        {
            var allData = Core.Global.GlobalData.Inventory.GetItemsByType(ItemType.Material, materialGroup).OrderBy(d => d.Item.Index);
            _parent = parent;
            Group = materialGroup;

            foreach (var data in allData)
            {
                var obj = Instantiate(_template, _content);
                obj.Init(data.Item, data.Amount);
                (obj.Selectable as Button).onClick.AddListener(() => Use(obj));
                _current = obj;
            }
            _allDisplays = GetComponentsInChildren<ItemDisplay>();
        }
        public void LateInit()
        {
            if (_current != null)
            {
                _defaultImage = _current.GetComponent<Image>().sprite;
                Use(_current, true);
            }
            else _titleField.text = $"{Group} (No item)";
        }

        private void UpdateText(IItem item)
        {
            _titleField.text = $"{item.Name} (Level {item.Index + 1})";
        }
        private void Use(ItemDisplay itemDisplay, bool firstInit = false)
        {
            if (itemDisplay.Amount == 0)
            {
                var index = System.Array.IndexOf(_allDisplays, itemDisplay) - 1;
                while (index >= 0 && itemDisplay.Amount == 0)
                {
                    itemDisplay = _allDisplays[index];
                    index--;
                }
                if (index < 0)
                {
                    //cannot use anything!
                    return;
                }
                Debug.Log(itemDisplay.Item.Index);
            }
            if (!firstInit && _current == itemDisplay) return;
            if (_current != null && !firstInit)
            {
                _current.Background.sprite = _defaultImage;
                _current.Background.color = Color.white;
                _parent.RestoreOne(this, _current);
            }
            UpdateText(itemDisplay.Item);
            itemDisplay.Background.sprite = _selectedImage;
            itemDisplay.Background.color = _selectedColor;
            _parent.RemoveOne(this, itemDisplay);
            _current = itemDisplay;
        }
        internal ItemDisplay GetNextSelectionTarget() => _allDisplays.FirstOrDefault();
        internal ItemDisplay GetNextSelectionTarget(ItemDisplay itemDisplay)
        {
            var index = System.Array.IndexOf(_allDisplays, itemDisplay) - 1;
            if (index < 0) return null;
            else if (index == 0)
            {
                for (int i = 1; i < _allDisplays.Length; i++)
                {
                    if (_allDisplays[i].Amount != 0) return _allDisplays[i];
                }
                return null;
            }
            else
            {
                for (int i = _allDisplays.Length - 1; i >= 0; i--)
                {
                    if (_allDisplays[i].Amount != 0) return _allDisplays[i];
                }
                return null;
            }
        }
    }
}
