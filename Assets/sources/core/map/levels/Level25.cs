using PataRoad.Core.Character;
using PataRoad.Core.Character.Patapons;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    /// <summary>
    /// !!-- PLACE AFTER <see cref="StepByPerfection"/>. ---!!
    /// </summary>
    class Level25 : MonoBehaviour, IHavingLevel
    {
        [SerializeField]
        Transform _carriage;
        Structure _carriageStructure;
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
        [SerializeField]
        MissionController _missionController;
        bool _keepMoving;
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
            _carriageStructure.OnDestroy.AddListener(() =>
            {
                if (!_cannon.IsDead)
                {
                    _cannon.Die();
                }
                _hataponCharacter.StatusEffectManager.RecoverAndIgnoreEffect();
            });

            //weather-
            Weather.WeatherInfo.Current.OnWeatherChanged.AddListener(ChangeStepsOnWeather);
            PataponsManager.Current.SetMinMaxStepRatio(0.2f, 0.5f);
        }
        public void ChangeStepsOnWeather(Weather.WeatherType weatherType)
        {
            if (weatherType == Weather.WeatherType.Snow)
            {
                PataponsManager.Current.SetMinMaxStepRatio(0.2f, 0.5f);
            }
            else
            {
                PataponsManager.Current.SetMinMaxStepRatio(0.6f, 1);
            }
        }
        public void SuccessMission()
        {
            GameSound.SpeakManager.Current.Play(_destroySound);
            MissionPoint.Current.FilledMissionCondition = true;
            MissionPoint.Current.EndMission();
        }
        public void FailMission() => _missionController.Fail(_carriage);

        private bool IsEndStatus() => _carriageStructure.IsDead || MissionPoint.IsMissionEnd;

        public void SetLevel(int level, int absoluteMaxLevel)
        {
            var offset = (float)(level - 1) / (absoluteMaxLevel - 1);
            _carriageSpeed *= Mathf.Lerp(1, 4, offset);
        }

        private void Update()
        {
            if (!IsEndStatus() || _keepMoving)
            {
                _carriage.Translate(_carriageSpeed * Time.deltaTime, 0, 0);
                if (_carriage.position.x > MissionPoint.Current.MissionPointPosition.x)
                {
                    FailMission();
                    _keepMoving = true;
                }
            }
        }
    }
}
