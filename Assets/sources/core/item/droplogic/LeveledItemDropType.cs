namespace PataRoad.Core.Items
{
    public enum LeveledItemDropType
    {
        Boss,
        Equipment
    }
    internal static class LeveledItemDrop
    {
        internal static ILeveldItemDropLogic GetLogic
            (LeveledItemDropType type, int minLevel, int maxLevel) => type switch
            {
                LeveledItemDropType.Boss => new BossItemDropLogic(minLevel),
                LeveledItemDropType.Equipment => new EquipmentDropLogic(minLevel, maxLevel),
                _ => throw new System.NotImplementedException()
            };
    }
}