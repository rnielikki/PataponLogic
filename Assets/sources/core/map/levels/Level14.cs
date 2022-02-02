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
        private void Start()
        {
            _carriageStructure = _carriage.GetComponent<Structure>();
            _pataponsManagerTransform = FindObjectOfType<Character.Patapons.PataponsManager>().transform;
            _animator = _carriage.GetComponent<Animator>();
        }
        public void FailMission()
        {
            Camera.main.GetComponent<CameraController.PataponCameraMover>().SetTarget(_carriage);
            MissionPoint.Current.WaitAndFailMission(4);
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
                }
            }
            else if (_moving && !IsEndStatus())
            {
                _moving = false;
                _animator.SetBool("moving", false);
            }
        }
        private bool IsEndStatus() => _carriageStructure.IsDead || MissionPoint.IsMissionEnd;

        public void SetLevel(int level)
        {
            _carriageSpeed *= 1 + (level - 1) * 0.02f;
            _maxOffset = level * 0.5f;
        }
    }
}
