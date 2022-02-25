using PataRoad.Core.Character;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level14 : MonoBehaviour, IHavingLevel
    {
        [SerializeField]
        Transform _carriage;
        Structure _carriageStructure;
        [SerializeField]
        float _carriageSpeed;
        float _maxOffset;
        private bool _moving;
        Transform _pataponsManagerTransform;
        Animator _animator;
        [SerializeField]
        Animator _hatapon;
        [SerializeField]
        GameObject _hataFlag;
        [SerializeField]
        [Tooltip("if the level is already cleared the Hatapon animation will be replaced with this")]
        RuntimeAnimatorController _animatorToReplace;
        [SerializeField]
        AudioClip _damageSound;
        [SerializeField]
        AudioClip _destroySound;
        [SerializeField]
        MissionController _missionController;
        private void Start()
        {
            _carriageStructure = _carriage.GetComponentInChildren<Structure>();
            _pataponsManagerTransform = FindObjectOfType<Character.Patapons.PataponsManager>().transform;
            _animator = _carriageStructure.GetComponent<Animator>();
            if (Global.GlobalData.CurrentSlot.MapInfo.NextMap.Cleared)
            {
                _hataFlag.gameObject.SetActive(false);
                _hatapon.runtimeAnimatorController = _animatorToReplace;
            }
            else
            {
                Global.GlobalData.CurrentSlot.Progress.IsEquipmentMinigameOpen = true;
            }
        }
        public void FailMission()
        {
            GameSound.SpeakManager.Current.Play(_destroySound);
            _hatapon.Play("sad");
            _missionController.Fail(_carriage);
        }
        public void PlayDamaged()
        {
            GameSound.SpeakManager.Current.Play(_damageSound);
        }
        private void Update()
        {
            if (!IsEndStatus()
                && _pataponsManagerTransform.position.x > _carriage.position.x - _maxOffset)
            {
                _carriage.Translate(_carriageSpeed * Time.deltaTime, 0, 0);
                if (!_moving)
                {
                    _moving = true;
                    _animator.SetBool("moving", true);
                    _hatapon.SetBool("walking", true);
                }
            }
            else if (_moving && !IsEndStatus())
            {
                _moving = false;
                _animator.SetBool("moving", false);
                _hatapon.SetBool("walking", false);
            }
        }
        private bool IsEndStatus() => _carriageStructure.IsDead || MissionPoint.IsMissionEnd;

        public void SetLevel(int level, int absoluteMaxLevel)
        {
            var offset = (float)(level - 1) / (absoluteMaxLevel - 1);
            _carriageSpeed *= Mathf.Lerp(1, 5, offset);
            _maxOffset = Mathf.Lerp(0.5f, 10, offset);
        }
    }
}
