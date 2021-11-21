using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Rhythm.Command
{
    /// <summary>
    /// Checks if input is miracle
    /// </summary>
    public class MiracleListener : MonoBehaviour
    {
        [Header("When miracle input (Don*5) in fever")]
        [SerializeField]
        public UnityEvent OnMiracle;
        [SerializeField]
        private RhythmInputMiracle _miracleDrumInput;
        internal RhythmInputMiracle MiracleDrumInput => _miracleDrumInput;
        public int MiracleDrumCount => _miracleDrumInput.MiracleDrumCount;
        internal virtual bool HasMiracleChance(IEnumerable<DrumType> currentDrums, RhythmInputModel input)
        {
            if (!RhythmFever.IsFever) return false;
            return CheckMiracleChance(currentDrums, input);
        }
        protected bool CheckMiracleChance(IEnumerable<DrumType> currentDrums, RhythmInputModel input)
        {
            var drum = input.Drum;
            if (_miracleDrumInput.EnteredMiracleHit && !IsMiracleDrum(drum, currentDrums))
            {
                ResetMiracle();
            }
            else if (IsMiracleDrum(drum, currentDrums))
            {
                _miracleDrumInput.StartCounter(currentDrums.Count());
            }
            return _miracleDrumInput.EnteredMiracleHit;
        }
        //Without this just many things must be public from internal and I don't want to change all
        internal void SetCurrentMiracleDrumTo(MiracleListener other) => other._miracleDrumInput = _miracleDrumInput;
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
