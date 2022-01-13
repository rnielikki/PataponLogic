using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level0 : MonoBehaviour
    {
        [SerializeField]
        Transform _targetToMove;
        bool _following;
        Transform _followingTarget;

        private void Start()
        {
            var dropBehaviour = GetComponentInParent<Items.ItemDropBehaviour>();
            //Prevents retiring
            Global.GlobalData.Input.actions.FindAction("UI/Start").Disable();
            Character.Patapons.PataponsManager.AllowedToGoForward = false;
            (dropBehaviour.DropData[0] as Items.SongItemDropData).OnSongComplete
                .AddListener(() => Character.Patapons.PataponsManager.AllowedToGoForward = true);

            dropBehaviour.Drop();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "SmallCharacter")
            {
                MissionPoint.Current.FilledMissionCondition = true;
                _following = true;
                _followingTarget = GameObject.FindGameObjectWithTag("Player").transform;
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
