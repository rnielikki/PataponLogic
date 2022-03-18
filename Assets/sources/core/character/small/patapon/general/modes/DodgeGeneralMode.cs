using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.General
{
    //prevents pushback. also adds knockback chance to the wall.
    class DodgeGeneralMode : GeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Ponpata;
        [SerializeField]
        GameObject _wallTemplate;
        GameObject _wall;
        bool _isOn;
        public override void Init()
        {
            _wall = Instantiate(_wallTemplate, PataponsManager.Current.transform);
            _wall.transform.localPosition = Vector3.zero;
            _wall.gameObject.SetActive(false);
        }

        public override void Activate(PataponGroup group)
        {
            if (_isOn) return;
            _isOn = true;
            _wall.gameObject.SetActive(true);
            TurnCounter.OnTurn.AddListener(StopGeneralMode);
        }
        private void StopGeneralMode()
        {
            if (TurnCounter.IsPlayerTurn) _isOn = false;
            else if (!_isOn) CancelGeneralMode();
        }
        public override void CancelGeneralMode()
        {
            _isOn = false;
            TurnCounter.OnTurn.RemoveListener(StopGeneralMode);
            _wall.gameObject.SetActive(false);
        }
    }
}
