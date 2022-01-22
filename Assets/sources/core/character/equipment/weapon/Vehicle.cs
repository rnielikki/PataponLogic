using PataRoad.Core.Items;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Vehicle : Protector
    {
        private void Awake()
        {
            LoadRenderersAndImage();
        }
        protected override void LoadRenderersAndImage()
        {
            if (_spriteRenderers != null) return;
            _spriteRenderers = new UnityEngine.SpriteRenderer[]
            {
                transform.Find("Head").GetComponent<UnityEngine.SpriteRenderer>()
            };
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            if (_spriteRenderers == null) LoadRenderersAndImage();
            _spriteRenderers[0].sprite = equipmentData.Image;
        }
    }
}
