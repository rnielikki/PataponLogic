using UnityEngine;
using UnityEngine.Events;

namespace Core.Map
{
    public class MissionPoint : MonoBehaviour
    {
        Animator _animator;
        // Start is called before the first frame update
        [SerializeField]
        [Tooltip("True if mission complete, otherwise False.")]
        private UnityEvent<bool> OnMissionEnd = new UnityEvent<bool>();
        private AudioSource _soundSource;
        [SerializeField]
        private AudioClip _missionSuccessSound;
        [SerializeField]
        private AudioClip _missionFailedSound;
        [SerializeField]
        [Tooltip("Mission complete condition, by default. Only true Misison Condition will call Mission Complete. Can be changed later in code.")]
        private bool _filledMissionCondition = true;
        public static bool IsMissionEnd { get; private set; }

        private const string _screenPath = "Map/Mission/Instruction/";
        /// <summary>
        /// Mission complete, only when this is true.
        /// </summary>
        public bool FilledMissionCondition { get; set; }
        public static MissionPoint Current { get; private set; }

        void Awake()
        {
            IsMissionEnd = false;
            Current = this;
            FilledMissionCondition = _filledMissionCondition;
            _animator = GetComponent<Animator>();
            _soundSource = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
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

        public void FailMission()
        {
            IsMissionEnd = true;
            OnMissionEnd.Invoke(false);
            AttachToScreen("MissionFailed");
            _soundSource.PlayOneShot(_missionFailedSound);
        }
        private void CompleteMission()
        {
            IsMissionEnd = true;
            OnMissionEnd.Invoke(true);
            if (_animator != null)
            {
                _animator.enabled = true;
                _animator.Play("MissionComplete");
            }
            AttachToScreen("MissionComplete");
            _soundSource.PlayOneShot(_missionSuccessSound);
        }
        private void AttachToScreen(string prefabName) =>
            Instantiate(
                Resources.Load(_screenPath + prefabName),
                GameObject.FindGameObjectWithTag("Screen").transform
                );
    }
}
