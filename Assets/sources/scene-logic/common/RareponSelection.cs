using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
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
        public RareponData RareponData => _data;
        private Button _button;
        private RareponSelector _parent;

        private void Start()
        {
            var data = Core.Global.GlobalData.PataponInfo.RareponInfo.GetRarepon(_index);
            _parent = GetComponentInParent<RareponSelector>();
            _button = GetComponent<Button>();
            if (data != null)
            {
                SetRarepon(data);
            }
            else
            {
                ShowImages(false);
            }
        }
        private void OnEnable()
        {
            if (_button != null) _button.enabled = true;
        }
        private void OnDisable()
        {
            if (_button != null) _button.enabled = false;
        }
        public void Select() => GetComponent<Button>().Select();
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
            _text.text = data.Name;
        }
        public bool ConfirmToCreateRarepon()
        {
            //check condition example. And...
            var material = Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Key, "Drum", 0);
            if (!Core.Global.GlobalData.Inventory.HasItem(material))
            {
                Core.Global.GlobalData.Sound.PlayBeep();
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
            var rarepon = Core.Global.GlobalData.PataponInfo.RareponInfo.OpenNewRarepon(_index);
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
