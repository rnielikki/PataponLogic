using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class MinigameSelectionButton : MonoBehaviour
    {
        [SerializeField]
        string[] _materialGroups;
        public string[] MaterialGroups => _materialGroups;
        [SerializeField]
        string _minigameType;
        public string MinigameType => _minigameType;
    }
}
