namespace PataRoad.SceneLogic.CommonSceneLogic
{
    [System.Serializable]
    class MaterialItemRequirement : Core.Items.ItemRequirement
    {
        [UnityEngine.SerializeField]
        private Core.Items.MaterialData _item;
        public override Core.Items.IItem Item => _item;
    }
}
