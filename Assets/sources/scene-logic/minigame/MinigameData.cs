using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    [CreateAssetMenu(fileName = "new-minigame", menuName = "Minigame Data")]
    public class MinigameData : ScriptableObject
    {
        [SerializeField]
        bool _useDonChakaGameSound;
        public bool UseDonChakaGameSound => _useDonChakaGameSound;
        [SerializeField]
        bool _useRandomDrums;
        public bool UseRandomDrums => _useRandomDrums;
        [SerializeField]
        private AudioClip _music;
        public AudioClip Music => _music;
        [SerializeField]
        private MinigameTurn[] MinigameTurns; //for preserving data it's pascal case
        public MinigameTurn[] Turns => MinigameTurns;
    }
}
