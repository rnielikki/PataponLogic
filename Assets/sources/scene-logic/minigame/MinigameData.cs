using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    [CreateAssetMenu(fileName = "new-minigame", menuName = "Minigame Data")]
    public class MinigameData : ScriptableObject
    {
        [SerializeField]
        bool _useDonChakaGameSound;
        public bool UseDonChakaGameSound => _useDonChakaGameSound;

        public MinigameTurn[] MinigameTurns;
    }
}
