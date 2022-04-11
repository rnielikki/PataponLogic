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
        AudioClip _badSound;
        [SerializeField]
        AudioClip _missSound;
        bool _clearedInput;

        private void Start()
        {
            _perfectFrequency = (int)(_perfectRange * RhythmTimer.HalfFrequency);
            _goodFrequency = (int)(_goodRange * RhythmTimer.HalfFrequency);

            _rhythmInput.OnDrumHit.AddListener(PlayAudio);
            RhythmTimer.Current.OnHalfTime.AddListener(ClearAudio);
        }

        public void PlayAudio(RhythmInputModel model)
        {
            if (!_clearedInput || !_manager.ListeningInput || model.Status == DrumHitStatus.Miss)
            {
                _audioSource.PlayOneShot(_missSound);
                return;
            }

            float timing = model.Timing;
            _clearedInput = false;
            if (timing <= _perfectFrequency) _audioSource.PlayOneShot(_perfectSound);
            else if (timing <= _goodFrequency) _audioSource.PlayOneShot(_goodSound);
            else _audioSource.PlayOneShot(_badSound);
        }
        private void ClearAudio() => _clearedInput = true;
    }
}
