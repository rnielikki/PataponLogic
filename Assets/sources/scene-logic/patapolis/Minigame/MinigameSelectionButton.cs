using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    abstract class MinigameSelectionButton : MonoBehaviour, IMaterialSelectionButton
    {
        [SerializeField]
        string[] _materialGroups;
        public string[] MaterialGroups => _materialGroups;
        [SerializeField]
        SceneLogic.Minigame.MinigameData _minigameData;
        public SceneLogic.Minigame.MinigameData MinigameData => _minigameData;
        [SerializeField]
        private UnityEngine.UI.Button _button;
        public UnityEngine.UI.Button Button => _button;

        public abstract (Core.Items.IItem item, int amount) GetReward(int levelSum, int minMaxDifference);
    }
}
