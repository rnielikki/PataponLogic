using Core.Rhythm;
using Core.Rhythm.Command;
using UnityEngine;

namespace Core.Character.Patapon
{
    class PataponsManager : MonoBehaviour
    {
        private Patapon[] _patapons;
        private void Awake()
        {
            _patapons = GetComponentsInChildren<Patapon>();
        }
        public void SendDrumInput(RhythmInputModel model)
        {
            var drumName = model.Drum.ToString();
            foreach (var pon in _patapons)
            {
                pon.MoveOnDrum(drumName);
            }
        }
        public void SendAction(RhythmCommandModel model)
        {
            foreach (var pon in _patapons)
            {
                pon.Act(model.Song, model.ComboType == ComboStatus.Fever);
            }
        }
        public void ResetAction()
        {
            foreach (var pon in _patapons)
            {
                pon.PlayIdle();
            }
        }

    }
}
