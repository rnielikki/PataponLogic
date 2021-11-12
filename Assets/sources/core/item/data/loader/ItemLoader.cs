using PataRoad.Core.Items;
/// <summary>
/// Manages all item informations. Also can pass to the other data loader like <see cref="EquipmentDataLoader"/>.
/// </summary>
namespace PataRoad.Core.Items
{

    public static class ItemLoader
    {
        public static IItem Load(ItemType type, string name, int index)
        {
            switch (type)
            {
                case ItemType.Equipment:
                    return EquipmentDataLoader.GetEquipment(name, index);
                default:
                    throw new System.NotImplementedException("not yet");
            }
        }
    }
}
