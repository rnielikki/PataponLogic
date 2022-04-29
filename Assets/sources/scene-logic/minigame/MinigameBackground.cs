using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    public class MinigameBackground : MonoBehaviour
    {
        [SerializeField]
        MinigameManager _minigameManager;
        void LoadBackground() => _minigameManager.LoadBackground();
        void LoadGame() => _minigameManager.LoadGame();
    }
}