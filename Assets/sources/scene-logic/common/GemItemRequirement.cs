namespace PataRoad.SceneLogic.CommonSceneLogic
{
    [System.Serializable]
    class GemItemRequirement : Core.Items.ItemRequirement
    {
        [UnityEngine.SerializeField]
        private Core.Items.GemData _item;
        public override Core.Items.IItem Item => _item;
    }
}
