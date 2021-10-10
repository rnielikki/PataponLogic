using UnityEngine;
using UnityEngine.UI;


namespace Core.Rhythm.Display
{
    /// <summary>
    /// Shows if it's perfect or fever warning. The worm color change logic (how much perfect it is) is on <see cref="ComboStatusDisplay"/> because it controls all of LineRenderer logics.
    /// </summary>
    public class ScoreStatusDisplay : MonoBehaviour
    {
        [SerializeField]
        AudioClip _warningSound;
        AudioSource _audioSource;

        //------------ Perfects
        ParticleSystem _perfectParticles;

        //------------ Warnings
        Animator _animator;
        int _warningAnimationHash;
        Image _warningImage;


        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;

            _perfectParticles = transform.Find("Perfect").GetComponent<ParticleSystem>();

            var warning = transform.Find("Warning");
            _animator = warning.GetComponent<Animator>();
            _warningImage = warning.GetComponent<Image>();
            _warningAnimationHash = Animator.StringToHash("Fever-Warning");

        }
        public void ShowFeverWarning()
        {
            _warningImage.enabled = true;
            _audioSource.PlayOneShot(_warningSound);
            _animator.Play(_warningAnimationHash);
            RhythmTimer.OnNextHalfTime.AddListener(() => Command.TurnCounter.OnNextTurn.AddListener(() => _warningImage.enabled = false));
        }
        public void ShowPerfect(Command.RhythmCommandModel model)
        {
            if (model.PerfectCount == 4 && gameObject.activeSelf)
            {
                _perfectParticles.Play();
                //Sound play moved to combo status display, because it should be played right after the last command
            }
        }
    }
}
