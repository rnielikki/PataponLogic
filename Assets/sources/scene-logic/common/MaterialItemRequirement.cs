namespace PataRoad.SceneLogic.CommonSceneLogic
{
    enum RequirementIncrementalType
    {
        One, //n, n+1, n+2
        Even, //1, 2, 4
        Odd //1, 3, 5
    }
    /// <summary>
    /// Rarepon material item requirement.
    /// </summary>
    [System.Serializable]
    class MaterialItemRequirement : Core.Items.ItemRequirement
    {
        [UnityEngine.SerializeField]
        private Core.Items.MaterialData _item;
        [UnityEngine.SerializeField]
        RequirementIncrementalType _incrementalType;
        public override Core.Items.IItem Item => _item;
        private int _defaultIndex;
        public override void Init()
        {
            _defaultIndex = Item.Index;
        }
        public override void SetRequirementByLevel(int level)
        {
            base.SetRequirementByLevel(level);
            if (_item.Index >= 4) return;
            _item = Core.Items.ItemLoader.GetItem<Core.Items.MaterialData>(
                Core.Items.ItemType.Material, _item.Group, GetItemIndex(level));
        }
        private int GetItemIndex(int level)
        {
            switch (_incrementalType)
            {
                case RequirementIncrementalType.One:
                    return UnityEngine.Mathf.Clamp(0, _defaultIndex + level - 1, 4);
                case RequirementIncrementalType.Even:
                    return (level < 2) ? 0 : ((level - 1) * 2 - 1);
                case RequirementIncrementalType.Odd:
                    return (level - 1) * 2;
                default:
                    throw new System.InvalidOperationException("???");
            }
        }
    }
}
