using UnityEngine;

namespace PataRoad.Core.Map.Levels.King
{
    public class HazoronAttacker : MonoBehaviour
    {
        private Animator _animator;
        bool _playingBeforeStatus;
        [SerializeField]
        private ParticleSystem _effect;
        [SerializeField]
        private AudioClip _generalModeSound;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }
        internal void PlayBeforeDefend()
        {
            if (_playingBeforeStatus) return;
            _animator.Play("defend-before");
            _effect.Play();
            GameSound.SpeakManager.Current.Play(_generalModeSound);
            _playingBeforeStatus = true;
        }
        internal void PlayDefend()
        {
            _playingBeforeStatus = false;
            _animator.Play("defend");
        }
    }
}