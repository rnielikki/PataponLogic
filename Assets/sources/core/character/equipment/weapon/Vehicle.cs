using PataRoad.Core.Items;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Vehicle : Protector
    {
        private UnityEngine.SpriteRenderer _renderer;
        private void Awake()
        {
            LoadRenderersAndImage();
        }
        protected override void LoadRenderersAndImage()
        {
            if (_renderer != null) return;
            _renderer = transform.Find("Head").GetComponent<UnityEngine.SpriteRenderer>();
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            if (_renderer == null) LoadRenderersAndImage();
            _renderer.sprite = equipmentData.Image;
        }
    }
}
