namespace PataRoad.SceneLogic.CommonSceneLogic
{
    [System.Serializable]
    class GemItemRequirement : Core.Items.ItemRequirement
    {
        [UnityEngine.SerializeField]
        private Core.Items.GemData _item;
        public override Core.Items.IItem Item => _item;
        private int _defaultAmount;
        public override void Init()
        {
            _defaultAmount = Amount;
        }

        public override void SetRequirementByLevel(int level)
        {
            _amount = _defaultAmount + level - 1;
        }
    }
}
