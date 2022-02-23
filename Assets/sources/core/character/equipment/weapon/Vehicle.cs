using PataRoad.Core.Items;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Vehicle : Protector
    {
        private UnityEngine.Sprite _defaultSprite;
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
            _defaultSprite = _spriteRenderers[0].sprite;
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            if (_spriteRenderers == null) LoadRenderersAndImage();
            if (equipmentData.Index == 0) _spriteRenderers[0].sprite = _defaultSprite;
            else _spriteRenderers[0].sprite = equipmentData.Image;
        }
    }
}
