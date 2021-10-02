﻿using Core.Rhythm.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Rhythm.Command
{
    /// <summary>
    /// Checks if input is miracle, and invokes Miracle event when Miracle is detected
    /// </summary>
    internal class MiracleListener : MonoBehaviour
    {
        [Header("When miracle input (Don*5) in fever")]
        [SerializeField]
        public UnityEvent OnMiracle;
        [SerializeField]
        private RhythmInputMiracle _miracleDrumInput;
        public int MiracleDrumCount => _miracleDrumInput.MiracleDrumCount;
        internal bool HasMiracleChance(RhythmInputModel input)
        {
            if (_miracleDrumInput.EnteredMiracleHit && input.Drum != DrumType.Don)
            {
                Reset();
            }
            else if (!_miracleDrumInput.EnteredMiracleHit && input.Drum == DrumType.Don)
            {

                _miracleDrumInput.StartCounter();
            }
            return _miracleDrumInput.EnteredMiracleHit;
        }
        internal void Reset()
        {
            _miracleDrumInput.ResetCounter();
        }
    }
}
