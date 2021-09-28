using Core.Rhythm.Model;
using UnityEngine;

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
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            sender.OnDrumHit.AddListener(PlayAnimation);
        }

        private void PlayAnimation(RhythmInputModel model)
        {
            _animator.Play(0);
        }
    }
}
