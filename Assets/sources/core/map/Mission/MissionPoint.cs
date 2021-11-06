using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Map
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
        private AudioClip _missionSuccessMusic;
        [SerializeField]
        private AudioClip _missionFailedMusic;

        [SerializeField]
        private AudioClip[] _ponsSpeakingWhenMissionComplete;

        [SerializeField]
        [Tooltip("Mission complete condition, by default. Only true Misison Condition will call Mission Complete. Can be changed later in code.")]
        private bool _filledMissionCondition = true;
        public static bool IsMissionEnd { get; private set; }
        public static bool IsMissionSuccess { get; private set; }

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

        /// <summary>
        /// "Mission failed" After certain time.
        /// </summary>
        /// <param name="seconds"></param>
        public void WaitAndFailMission(float seconds)
        {
            if (IsMissionEnd) return;
            IsMissionEnd = true;
            StartCoroutine(WaitAndFail());
            System.Collections.IEnumerator WaitAndFail()
            {
                yield return new WaitForSeconds(seconds);
                FailMission();
            }
        }
        public void FailMission()
        {
            if (IsMissionEnd) return;
            Camera.main.GetComponent<CameraController.CameraMover>().StopMoving();
            IsMissionEnd = true;
            OnMissionEnd.Invoke(false);
            AttachToScreen("MissionFailed");
            _soundSource.PlayOneShot(_missionFailedMusic);
        }
        private void CompleteMission()
        {
            if (IsMissionEnd) return;
            IsMissionEnd = true;
            IsMissionSuccess = true;
            OnMissionEnd.Invoke(true);

            if (_animator != null)
            {
                _animator.enabled = true;
                _animator.Play("MissionComplete");
            }

            StartCoroutine(DoMissionCompleteAction());
            System.Collections.IEnumerator DoMissionCompleteAction()
            {
                _soundSource.PlayOneShot(_missionSuccessSound);
                yield return new WaitForSeconds(2);
                foreach (var sound in _ponsSpeakingWhenMissionComplete)
                {
                    _soundSource.PlayOneShot(sound);
                    yield return new WaitForSeconds(2);
                }
                AttachToScreen("MissionComplete");
                _soundSource.PlayOneShot(_missionSuccessMusic);
                Camera.main.GetComponent<CameraController.CameraMover>().StopMoving();
            }
        }
        private void AttachToScreen(string prefabName) =>
            Instantiate(Resources.Load(_screenPath + prefabName));
    }
}
