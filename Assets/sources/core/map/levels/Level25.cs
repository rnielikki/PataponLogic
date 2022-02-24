using PataRoad.Core.Character;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level25 : MonoBehaviour, IHavingLevel
    {
        [SerializeField]
        Transform _carriage;
        Character.Structure _carriageStructure;
        [SerializeField]
        float _carriageSpeed;
        Animator _animator;
        [SerializeField]
        Animator _hatapon;
        IAttackable _hataponCharacter;
        [SerializeField]
        GameObject _hataFlag;
        [SerializeField]
        AudioClip _destroySound;
        [SerializeField]
        CannonStructure _cannon;
        private void Start()
        {
            _carriageStructure = _carriage.GetComponentInChildren<Structure>();
            _animator = _carriageStructure.GetComponent<Animator>();

            _hataponCharacter = _hatapon.GetComponent<IAttackable>();

            _carriageStructure.StatusEffectManager.OnStatusEffect.AddListener((effect) =>
            {
                if (effect == StatusEffectType.Fire)
                {
                    _hataponCharacter.StatusEffectManager.SetFire(4);
                }
            });
            if (Global.GlobalData.CurrentSlot.MapInfo.NextMap.Cleared)
            {
                _hataFlag.gameObject.SetActive(false);
            }
            else
            {
                Global.GlobalData.CurrentSlot.Progress.IsEquipmentMinigameOpen = true;
            }
            _animator.SetBool("moving", true);
            _hatapon.SetBool("walking", true);
            _carriageStructure.OnDestroy.AddListener(() =>
            {
                if (!_cannon.IsDead)
                {
                    _cannon.Die();
                }
                _hataponCharacter.StatusEffectManager.RecoverAndIgnoreEffect();
            });
        }
        public void SuccessMission()
        {
            GameSound.SpeakManager.Current.Play(_destroySound);
        }
        public void FailMission()
        {
            Camera.main.GetComponent<CameraController.SafeCameraZoom>().ZoomIn(_carriage);
            MissionPoint.Current.WaitAndFailMission(4);
        }
        private bool IsEndStatus() => _carriageStructure.IsDead || MissionPoint.IsMissionEnd;

        public void SetLevel(int level, int absoluteMaxLevel)
        {
            var offset = (float)(level - 1) / (absoluteMaxLevel - 1);
            _carriageSpeed *= Mathf.Lerp(1, 4, offset);
        }

        private void Update()
        {
            if (!IsEndStatus())
            {
                _carriage.Translate(_carriageSpeed * Time.deltaTime, 0, 0);
                if (_carriage.position.x > MissionPoint.Current.MissionPointPosition.x)
                {
                    FailMission();
                }
            }
        }
    }
}
