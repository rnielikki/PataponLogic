using Core.Rhythm;
using Core.Rhythm.Command;
using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Gets command and drum status and sends message to Patapons.
    /// </summary>
    class PataponsManager : MonoBehaviour
    {
        private Patapon[] _patapons;
        private bool _isAlreadyIdle;
        private void Awake()
        {
            _patapons = GetComponentsInChildren<Patapon>();
        }
        /// <summary>
        /// Attach to <see cref="RhythmInput.OnDrumHit"/>.
        /// </summary>
        /// <param name="model"></param>
        public void SendDrumInput(RhythmInputModel model)
        {
            if (model.Status == DrumHitStatus.Miss)
            {
                return;
            }
            var drumName = model.Drum.ToString();
            foreach (var pon in _patapons)
            {
                pon.MoveOnDrum(drumName);
            }
        }
        /// <summary>
        /// Attach to <see cref="RhythmCommand.OnCommandInput"/>.
        /// </summary>
        /// <param name="model"></param>
        public void SendAction(RhythmCommandModel model)
        {
            _isAlreadyIdle = false;
            foreach (var pon in _patapons)
            {
                pon.Act(model.Song, model.ComboType == ComboStatus.Fever);
            }
        }
        /// <summary>
        /// Attach to <see cref="RhythmCommand.OnCommandCanceled"/>.
        /// </summary>
        public void ResetAction()
        {
            if (_isAlreadyIdle) return;
            _isAlreadyIdle = true;
            foreach (var pon in _patapons)
            {
                pon.PlayIdle();
            }
        }

    }
}
