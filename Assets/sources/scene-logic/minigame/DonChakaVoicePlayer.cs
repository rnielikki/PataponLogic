using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    [System.Serializable]
    public class DonChakaVoicePlayer
    {
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip[] _pata;
        [SerializeField]
        private AudioClip[] _pon;
        [SerializeField]
        private AudioClip[] _don;
        [SerializeField]
        private AudioClip[] _chaka;

        private System.Collections.Generic.Dictionary<MinigameDrumType, AudioClip[]> _soundMap;

        private int _soundIndex;

        public void Init(AudioSource audioSource)
        {
            _audioSource = audioSource;
            _soundMap = new System.Collections.Generic.Dictionary<MinigameDrumType, AudioClip[]>()
            {
                { MinigameDrumType.Pata, _pata},
                { MinigameDrumType.Pon, _pon},
                { MinigameDrumType.Don, _don},
                { MinigameDrumType.Chaka, _chaka},
            };
        }

        public void Play(MinigameDrumType drumType)
        {
            if (drumType != MinigameDrumType.Empty)
            {
                _audioSource.PlayOneShot(_soundMap[drumType][_soundIndex]);
                if (_soundIndex < 4) _soundIndex++;
            }
        }
        public void ClearTurn()
        {
            _soundIndex = 0;
        }
    }
}
