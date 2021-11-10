namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Vehicle : Equipment
    {
        private UnityEngine.SpriteRenderer _renderer;
        private void Awake()
        {
            LoadRenderersAndImage();
        }
        protected override void LoadRenderersAndImage()
        {
            _renderer = transform.Find("Head").GetComponent<UnityEngine.SpriteRenderer>();
            _type = EquipmentType.Protector;
        }
        protected override void ReplaceImage(EquipmentData equipmentData)
        {
            _renderer.sprite = equipmentData.Image;
        }
    }
}
