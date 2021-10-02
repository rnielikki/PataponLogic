using Core.Rhythm.Command;
using Core.Rhythm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Rhythm
{
    internal class RhythmInputMiracle : RhythmInput
    {
        [SerializeField]
        [Tooltip("If miracle hit is outside this range (as seconds), it's considered as miss in miracle hit.")]
        private float _newGoodRange;
        private int _newGoodFrequency;
        /// <summary>
        /// Reperesents 'Should send miracle signal'. DOESN'T directly affect <see cref="MiracleDrumCount"/>.
        /// </summary>
        internal bool EnteredMiracleHit { get; private set; }
        internal int MiracleDrumCount { get; private set; }
        private int _miracleCounter;

        private void Awake()
        {
            _newGoodFrequency = (_newGoodRange == 0) ? (int)(RhythmTimer.GoodFrequency * 0.75) : (int)(_newGoodRange / Time.fixedDeltaTime);
            if (DrumType != DrumType.Don)
            {
                throw new ArgumentException("Only DON Miracle drum is supported");
            }
            TurnCounter.OnTurn.AddListener(() => { if (!TurnCounter.IsPlayerTurn) ResetCounter(); });
            Init();
        }
        protected override void SetResetTimer()
        {
            RhythmTimer.OnHalfTime.AddListener(SetEnable);
        }
        protected override RhythmInputModel GetInputModel()
        {
            if (RhythmFever.IsFever && TurnCounter.IsPlayerTurn)
            {
                MiracleDrumCount++;
            }
            if (!EnteredMiracleHit)
            {
                return base.GetInputModel();
            }
            else
            {
                //When the drum hti is miracle, it doesn't matter how perfect it is, really
                return new RhythmInputModel(
                    DrumType.Don,
                    RhythmTimer.Count,
                    DrumHitStatus.Perfect
                );
            }
        }

        private void FixedUpdate()
        {
            if (RhythmFever.IsFever && TurnCounter.IsPlayerTurn)
            {
                _miracleCounter++;
            }
            if (EnteredMiracleHit)
            {
                int min, max;
                switch (MiracleDrumCount)
                {
                    case 1:
                        min = RhythmTimer.HalfFrequency - RhythmTimer.GoodFrequency;
                        max = RhythmTimer.HalfFrequency + _newGoodFrequency;
                        break;
                    case 2:
                        min = RhythmTimer.Frequency - _newGoodFrequency;
                        max = RhythmTimer.Frequency + RhythmTimer.GoodFrequency;
                        break;
                    case 3:
                        min = RhythmTimer.Frequency * 2 - RhythmTimer.GoodFrequency;
                        max = RhythmTimer.Frequency * 2 + _newGoodFrequency;
                        break;
                    case 4:
                        min = RhythmTimer.Frequency * 2 + RhythmTimer.HalfFrequency - _newGoodFrequency;
                        max = RhythmTimer.Frequency * 2 + RhythmTimer.HalfFrequency + RhythmTimer.GoodFrequency;
                        break;
                    default:
                        throw new InvalidOperationException("Miracle Drum count isn't valid");
                }
                if (_miracleCounter == min)
                {
                    Disabled = false;
                }
                else if (_miracleCounter == max)
                {
                    Disabled = true;
                }
            }
        }
        /// <summary>
        /// Start miracle counting.
        /// </summary>
        internal void StartCounter()
        {
            RhythmTimer.OnHalfTime.RemoveListener(SetEnable);
            EnteredMiracleHit = true;
        }
        /// <summary>
        /// Stop(reset) miracle counting.
        /// </summary>
        internal void ResetCounter()
        {
            RhythmTimer.OnHalfTime.AddListener(SetEnable);
            EnteredMiracleHit = false;
            _miracleCounter = 0;
            MiracleDrumCount = 0;
        }

        //------------- inherit
        private void OnEnable() => Enable();
        private void OnDisable() => Disable();
        private void OnDestroy() => Destroy();
    }
}
