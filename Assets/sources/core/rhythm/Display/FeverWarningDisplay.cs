using UnityEngine;
using UnityEngine.UI;

namespace Core.Rhythm.Display
{
    public class FeverWarningDisplay : MonoBehaviour
    {
        Animator _animator;
        int _animationHash;
        AudioSource _audioSource;
        Image _image;
        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            _image = GetComponent<Image>();
            _audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            _animationHash = Animator.StringToHash("Fever-Warning");
        }
        public void ShowFeverWarning()
        {
            _image.enabled = true;
            _audioSource.Play();
            _animator.Play(_animationHash);
            RhythmTimer.OnNextHalfTime.AddListener(() => Command.TurnCounter.OnNextTurn.AddListener(() => _image.enabled = false));
        }
    }
}
