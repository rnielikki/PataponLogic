using PataRoad.Core.Rhythm;
using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    class MinigameInputAudio : MonoBehaviour
    {
        [SerializeField]
        MinigameManager _manager;
        [SerializeField]
        RhythmInput _rhythmInput;
        [SerializeField]
        AudioSource _audioSource;
        [SerializeField]
        float _perfectRange;
        int _perfectFrequency;
        [SerializeField]
        AudioClip _perfectSound;
        [SerializeField]
        float _goodRange;
        int _goodFrequency;
        [SerializeField]
        AudioClip _goodSound;
        [SerializeField]
        float _notGoodRange;
        int _notGoodFrequency;
        [SerializeField]
        AudioClip _notGoodSound;
        [SerializeField]
        AudioClip _badSound;

        private void Start()
        {
            _perfectFrequency = (int)_perfectRange * RhythmTimer.HalfFrequency;
            _goodFrequency = (int)_goodRange * RhythmTimer.HalfFrequency;
            _notGoodFrequency = (int)_notGoodRange * RhythmTimer.HalfFrequency;

            _rhythmInput.OnDrumHit.AddListener(PlayAudio);
        }

        public void PlayAudio(RhythmInputModel model)
        {
            if (!_manager.ListeningInput)
            {
                _audioSource.PlayOneShot(_badSound);
                return;
            }

            float timing = model.Timing;
            if (timing <= _perfectFrequency) _audioSource.PlayOneShot(_perfectSound);
            else if (timing <= _goodFrequency) _audioSource.PlayOneShot(_goodSound);
            else if (timing <= _notGoodFrequency) _audioSource.PlayOneShot(_notGoodSound);
            else _audioSource.PlayOneShot(_badSound);
        }
    }
}
