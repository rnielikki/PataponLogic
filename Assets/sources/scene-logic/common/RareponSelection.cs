using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class RareponSelection : MonoBehaviour
    {
        [SerializeField]
        int _index;
        public int Index => _index;
        [SerializeField]
        Image _image;
        [SerializeField]
        Text _text;
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
        }
        private void OnEnable()
        {
            if (_button != null) _button.enabled = true;
        }
        public void Select() => GetComponent<Button>().Select();
        private void SetRarepon(RareponData data)
        {
            _data = data;
            _image.sprite = data.Image;
            _text.text = data.Name;
            _image.color = data.Color;
        }
        public void ConfirmToCreateRarepon()
        {
            //check condition. And...
            _parent.enabled = false;
            Common.GameDisplay.ConfirmDialog.Create("Create?", _parent, AddThisRarepon);
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
    }
}
