namespace PataRoad.SceneLogic.CommonSceneLogic
{
    [System.Serializable]
    class MaterialItemRequirement : Core.Items.ItemRequirement
    {
        [UnityEngine.SerializeField]
        private Core.Items.MaterialData _item;
        public override Core.Items.IItem Item => _item;
        private int _defaultIndex;
        public override void Init()
        {
            _defaultIndex = Item.Index;
        }

        public override void SetRequirementByLevel(int level)
        {
            if (_item.Index >= 4) return;
            _item = Core.Items.ItemLoader.GetItem<Core.Items.MaterialData>(
                Core.Items.ItemType.Material, _item.Group, UnityEngine.Mathf.Clamp(0, _defaultIndex + level - 1, 4));
        }
    }
}
