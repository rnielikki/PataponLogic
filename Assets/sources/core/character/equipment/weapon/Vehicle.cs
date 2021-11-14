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
            _renderer = transform.Find("Head").GetComponent<UnityEngine.SpriteRenderer>();
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            _renderer.sprite = equipmentData.Image;
        }
    }
}
