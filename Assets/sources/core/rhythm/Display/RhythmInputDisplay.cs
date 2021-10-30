﻿using UnityEngine;

namespace Core.Rhythm.Display
{
    /// <summary>
    /// Displays when the drum is hit. Attached to the graphic part with ANIMATOR.
    /// </summary>
    internal class RhythmInputDisplay : MonoBehaviour
    {
        [SerializeField]
        RhythmInput sender;
        Animator _animator;
        ParticleSystem _particle;
        ParticleSystem.MainModule _mainModule;
        private int _drumAnimationHash;
        private int _missAnimationHash;
        private RectTransform _rect;

        [SerializeField]
        [Tooltip("Random X offset position range from center.")]
        private float _xRange;
        [SerializeField]
        [Tooltip("Random Y offset position range from center.")]
        private float _yRange;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _particle = GetComponentInChildren<ParticleSystem>();
            _mainModule = _particle.main;
            _rect = GetComponent<RectTransform>();
            _drumAnimationHash = Animator.StringToHash("DrumAnimation");
            _missAnimationHash = Animator.StringToHash("MissAnimation");
        }
        private void OnEnable()
        {
            sender.OnDrumHit.AddListener(PlayAnimation);
        }
        private void OnDisable()
        {
            sender.OnDrumHit.RemoveListener(PlayAnimation);
        }

        private void PlayAnimation(RhythmInputModel model)
        {
            _rect.anchoredPosition = new Vector2(Random.Range(-_xRange, _xRange), Random.Range(-_yRange, _yRange));
            if (model.Status == DrumHitStatus.Miss)
            {
                _animator.Play(_missAnimationHash);
            }
            else
            {
                _animator.Play(_drumAnimationHash);
                _mainModule.startSize = GetStartSize(model.Status);
                _particle.Play();
            }
        }
        private float GetStartSize(DrumHitStatus status)
        {
            switch (status)
            {
                case DrumHitStatus.Perfect:
                    return 2;
                case DrumHitStatus.Good:
                    return 0.75f;
                default:
                    return 0;
            }
        }
    }
}
