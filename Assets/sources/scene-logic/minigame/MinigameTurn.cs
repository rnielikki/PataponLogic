namespace PataRoad.SceneLogic.Minigame
{
    [System.Serializable]
    public class MinigameTurn
    {
        [UnityEngine.SerializeField]
        private MinigameDrumType[] _drums;
        public MinigameDrumType[] Drums => _drums;

        [UnityEngine.SerializeField]
        [UnityEngine.Tooltip("If you use donchaka sound, you can skip this")]
        private UnityEngine.AudioClip _sound;
        public UnityEngine.AudioClip Sound => _sound;
    }
}
