using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class MinigameSelectionButton : MonoBehaviour
    {
        [SerializeField]
        string[] _materialGroups;
        public string[] MaterialGroups => _materialGroups;
        [SerializeField]
        SceneLogic.Minigame.MinigameData _minigameData;
        public SceneLogic.Minigame.MinigameData MinigameData => _minigameData;

        //Will add later different reward depends on material.
        [SerializeField]
        private Core.Items.IItem _reward;
        public Core.Items.IItem Reward => _reward;
        [SerializeField]
        private int _rewardAmount = 1;
        public int RewardAmount => _rewardAmount;
    }
}
