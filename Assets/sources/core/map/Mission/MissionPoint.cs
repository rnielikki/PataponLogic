using UnityEngine;
using UnityEngine.Events;

namespace Core.Map
{
    public class MissionPoint : MonoBehaviour
    {
        Animator _animator;
        private Character.Patapon.PataponsManager _pataponsManager;
        // Start is called before the first frame update
        [SerializeField]
        private UnityEvent OnMissionEnd;
        private AudioSource _soundSource;
        [SerializeField]
        private AudioClip _missionSuccessSound;
        [SerializeField]
        private AudioClip _missionFailedSound;
        [SerializeField]
        [Tooltip("Mission complete condition, by default. Only true Misison Condition will call Mission Complete. Can be changed later in code.")]
        private bool _filledMissionCondition = true;
        /// <summary>
        /// Mission complete, only when this is true.
        /// </summary>
        public bool FilledMissionCondition { get; set; }
        public static MissionPoint Current { get; private set; }

        void Awake()
        {
            Current = this;
            FilledMissionCondition = _filledMissionCondition;
            _pataponsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Character.Patapon.PataponsManager>();
            _animator = GetComponent<Animator>();
            _soundSource = GetComponent<AudioSource>();
        }
        /// <summary>
        /// Ends mission. Mission complete if <see cref="FilledMissionCondition"/> is true, otherwise Mission failed.
        /// </summary>
        public void EndMission()
        {
            if (FilledMissionCondition)
            {
                CompleteMission();
            }
            else
            {
                FailMission();
            }
        }
        private void CompleteMission()
        {
            OnMissionEnd.Invoke();
            if (_animator != null)
            {
                _animator.enabled = true;
                _animator.Play("MissionComplete");
            }
            _pataponsManager.DoMissionCompleteAction();
            _soundSource.PlayOneShot(_missionSuccessSound);
        }
        public void FailMission()
        {
            OnMissionEnd.Invoke();
            _soundSource.PlayOneShot(_missionFailedSound);
        }
    }
}
