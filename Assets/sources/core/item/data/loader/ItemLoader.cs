/// <summary>
/// Manages all item informations. Also can pass to the other data loader like <see cref="EquipmentDataLoader"/>.
/// </summary>
namespace PataRoad.Core.Items
{

    public static class ItemLoader
    {
        public static IItem Load(ItemMetadata data)
        {
            switch (data.Type)
            {
                case ItemType.Equipment:
                    return EquipmentDataLoader.GetEquipment(data);
                default:
                    throw new System.NotImplementedException("not yet");
            }
        }
    }
}
