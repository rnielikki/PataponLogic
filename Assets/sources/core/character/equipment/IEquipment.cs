namespace Core.Character.Equipment
{
    public interface IEquipment
    {
        public EquipmentType EquipmentType { get; }

        public string WeaponName { get; }
        public Stat Stat { get; }
        public UnityEngine.Sprite Image { get; }
    }
}
