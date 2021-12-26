using PataRoad.Core.Rhythm;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    class MinigameInputAudio : MonoBehaviour
    {
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
            _perfectFrequency = (int)_perfectRange * RhythmTimer.Frequency;
            _goodFrequency = (int)_goodRange * RhythmTimer.Frequency;
            _notGoodFrequency = (int)_notGoodRange * RhythmTimer.Frequency;

            _rhythmInput.OnDrumHit.AddListener(PlayAudio);
        }

        public void PlayAudio(RhythmInputModel model)
        {
            float timing = model.Timing;
            if (timing <= _perfectFrequency) _audioSource.PlayOneShot(_perfectSound);
            else if (timing <= _goodFrequency) _audioSource.PlayOneShot(_goodSound);
            else if (timing <= _notGoodFrequency) _audioSource.PlayOneShot(_notGoodSound);
            else _audioSource.PlayOneShot(_badSound);
        }
    }
}
