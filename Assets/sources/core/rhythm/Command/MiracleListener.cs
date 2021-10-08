using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Rhythm.Command
{
    /// <summary>
    /// Checks if input is miracle
    /// </summary>
    internal class MiracleListener : MonoBehaviour
    {
        [Header("When miracle input (Don*5) in fever")]
        [SerializeField]
        public UnityEvent OnMiracle;
        [SerializeField]
        private RhythmInputMiracle _miracleDrumInput;
        public int MiracleDrumCount => _miracleDrumInput.MiracleDrumCount;
        internal bool HasMiracleChance(IEnumerable<DrumType> currentDrums, RhythmInputModel input)
        {
            var drum = input.Drum;
            if (_miracleDrumInput.EnteredMiracleHit && !IsMiracleDrum(drum, currentDrums))
            {
                ResetMiracle();
            }
            else if (!_miracleDrumInput.EnteredMiracleHit && IsMiracleDrum(drum, currentDrums))
            {
                _miracleDrumInput.StartCounter();
            }
            return _miracleDrumInput.EnteredMiracleHit;
        }
        internal void ResetMiracle() => _miracleDrumInput.ResetCounter();
        private bool IsMiracleDrum(DrumType inputDrum, IEnumerable<DrumType> currentDrums)
        {
            if (inputDrum != DrumType.Don) return false;
            foreach (var drum in currentDrums)
            {
                if (drum != DrumType.Don) return false;
            }
            return true;
        }
        private void OnDestroy()
        {
            OnMiracle.RemoveAllListeners();
        }
    }
}
