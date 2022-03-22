using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class HaveOnContact : MonoBehaviour
    {
        [SerializeField]
        Transform _targetToMove;
        bool _following;
        Transform _followingTarget;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("SmallCharacter"))
            {
                MissionPoint.Current.FilledMissionCondition = true;
                _following = true;
                _followingTarget = Character.Patapons.PataponsManager.Current.transform;
            }
        }
        private void Update()
        {
            if (_following)
            {
                _targetToMove.transform.position = _followingTarget.transform.position;
            }
        }
    }
}