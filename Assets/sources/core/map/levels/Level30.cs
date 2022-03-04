using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level30 : MonoBehaviour
    {
        [SerializeField]
        Transform _tree;
        private float _pos;
        private Transform _pataponsManager;
        bool _entered;
        private RhythmFever _feverManager;

        private void Start()
        {
            _pos = _tree.position.x;
            _pataponsManager = Character.Patapons.PataponsManager.Current.transform;
            _feverManager = FindObjectOfType<RhythmCommand>().ComboManager.FeverManager;
            FindObjectOfType<RhythmCommand>().OnCommandInput.AddListener(EndIfAttack);
        }
        private void EndIfAttack(RhythmCommandModel model)
        {
            if (model.Song == CommandSong.Ponpon)
            {
                FailMission();
            }
        }
        private void Update()
        {
            if (!_entered && _pataponsManager.position.x >= _pos)
            {
                _entered = true;
                if (RhythmFever.IsFever)
                {
                    FailMission();
                }
                else _feverManager.OnFever.AddListener(FailMission);
            }
        }
        private void FailMission() => MissionPoint.Current.WaitAndFailMission(4);
    }
}
