using PataRoad.Core.Items;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class GemMinigameSelectionButton : MinigameSelectionButton
    {
        [UnityEngine.SerializeField]
        private EquipmentData _reward;
        private readonly int[] _sums = new int[] { 1, 2, 4, 7, 10 };

        //--- Note: this minigame will require only 1 material ------------
        //--- if array out of index exception it's your fault ---
        public override (IItem item, int amount) GetReward(int levelSum, int minMaxDifference)
        {
            return (ItemLoader.LoadByReference(_reward), _sums[levelSum]);
        }
    }
}
