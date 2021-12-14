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
        public static bool IsMissionEnd { get; private set; }
        public static bool IsMissionSuccess { get; private set; }

        private const string _screenPath = "Map/Mission/Instruction/";

        public static int MissionCompleteTime { get; private set; }
        /// <summary>
        /// Mission complete, only when this is true.
        /// </summary>
        public bool FilledMissionCondition { get; set; }
        public bool UseMissionTower { get; set; }
        public static MissionPoint Current { get; private set; }

        public Vector2 MissionPointPosition { get; private set; }

        void Awake()
        {
            IsMissionEnd = false;
            Current = this;
            _soundSource = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
        }
        private void Start()
        {
            var missionPoint = GameObject.FindGameObjectWithTag("Finish");
            if (missionPoint != null)
            {
                MissionPointPosition = missionPoint.transform.position;
                _animator = missionPoint.GetComponent<Animator>();
            }
        }
        /// <summary>
        /// Ends mission. Mission complete if <see cref="FilledMissionCondition"/> is true, otherwise Mission failed.
        /// </summary>
        public void EndMission()
        {
            if (IsMissionEnd) return;
            IsMissionEnd = true;
            if (FilledMissionCondition)
            {
                CompleteMission();
            }
            else
            {
                WaitAndFailMission(4);
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
            OnMissionEnd.Invoke(false);
            StartCoroutine(WaitAndFail());
            System.Collections.IEnumerator WaitAndFail()
            {
                yield return new WaitForSeconds(seconds);
                FailMission();
            }
        }
        public void FailMissionNow()
        {
            if (IsMissionEnd) return;
            IsMissionEnd = true;
            OnMissionEnd.Invoke(false);
            FailMission();
        }
        private void FailMission()
        {
            AttachToScreen("MissionFailed");
            _soundSource.PlayOneShot(_missionFailedMusic);
            Global.GlobalData.TipIndex = Global.GlobalData.MapInfo.NextMap.MapData.TipIndexOnFail;
            Global.GlobalData.MapInfo.MissionFailed();
            StartCoroutine(WaitForNextScene());

            System.Collections.IEnumerator WaitForNextScene()
            {
                yield return new WaitForSeconds(9);
                Common.SceneLoadingAction.Create("Patapolis", true);
            }
        }
        private void CompleteMission()
        {
            MissionCompleteTime = (int)Time.timeSinceLevelLoad;
            IsMissionSuccess = true;
            OnMissionEnd.Invoke(true);
            Global.GlobalData.MapInfo.MissionSucceeded();
            Global.GlobalData.TipIndex = Global.GlobalData.MapInfo.NextMap.MapData.TipIndexOnSuccess;

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
