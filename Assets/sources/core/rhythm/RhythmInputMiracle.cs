using PataRoad.Core.Rhythm.Command;
using System;
using System.Collections;
using UnityEngine;

namespace PataRoad.Core.Rhythm
{
    /// <summary>
    /// Sends one drum signal, like <see cref="RhythmInput"/>, but also extended to manual miracle hearing.
    /// </summary>
    internal class RhythmInputMiracle : RhythmInput
    {
        [SerializeField]
        [Tooltip("If miracle hit is outside this range (as seconds), it's considered as miss in miracle hit.")]
        private float _newGoodRange;
        /// <summary>
        /// Reperesents 'Should send miracle signal'. DOESN'T directly affect <see cref="MiracleDrumCount"/>.
        /// </summary>
        internal bool EnteredMiracleHit { get; private set; }
        internal int MiracleDrumCount { get; private set; }

        int[] _minTimerIndexes;
        int[] _maxTimerIndexes;
        private void Awake()
        {
            if (_newGoodRange > RhythmEnvironment.InputInterval / 4)
            {
                throw new ArgumentException("The good range cannot be more than " + RhythmEnvironment.InputInterval / 4);
            }
            int newHalfGoodFrequency = (_newGoodRange == 0) ? (int)(RhythmTimer.GoodFrequency * 0.5) : (int)(_newGoodRange / Time.fixedDeltaTime);
            int newGoodFrequency = newHalfGoodFrequency * 2;
            if (DrumType != DrumType.Don)
            {
                throw new ArgumentException("Only DON Miracle drum is supported");
            }
            TurnCounter.OnTurn.AddListener(() => { if (!TurnCounter.IsPlayerTurn) ResetCounter(); });
            _minTimerIndexes = new int[]
            {
                RhythmTimer.HalfFrequency - newGoodFrequency, //2
                RhythmTimer.HalfFrequency - newHalfGoodFrequency, //3
                RhythmTimer.HalfFrequency - newGoodFrequency, //4
                RhythmTimer.HalfFrequency - newHalfGoodFrequency //5
            };
            _maxTimerIndexes = new int[]
            {
                RhythmTimer.HalfFrequency + newHalfGoodFrequency, //2
                RhythmTimer.HalfFrequency + newGoodFrequency, //3
                RhythmTimer.HalfFrequency + newHalfGoodFrequency, //4
                RhythmTimer.HalfFrequency + newGoodFrequency //5
            };
            Init();
        }
        protected override void SetResetTimer()
        {
            RhythmTimer.OnHalfTime.AddListener(SetEnable);
        }
        protected override RhythmInputModel GetInputModel()
        {
            if (!EnteredMiracleHit)
            {
                return base.GetInputModel();
            }
            else
            {
                if (Disabled) return RhythmInputModel.Miss(DrumType.Don);
                int current;
                switch (MiracleDrumCount)
                {
                    case 1: //this is 2nd
                    case 4: //this is 5th
                        current = Mathf.Abs((RhythmTimer.HalfFrequency - RhythmTimer.Count) % RhythmTimer.Frequency);
                        break;
                    case 2:
                    case 3:
                        current = RhythmTimer.Count;
                        break;
                    default:
                        throw new InvalidOperationException($"The {MiracleDrumCount}th miracle drum count isn't valid");
                }
                if (MiracleDrumCount % 2 == 1)
                {
                    RhythmTimer.OnNextQuarterTime.AddListener(() => Disabled = false);
                }
                else
                {
                    RhythmTimer.OnNextHalfTime.AddListener(() => Disabled = false);
                }
                Disabled = true;
                if (current >= _minTimerIndexes[MiracleDrumCount - 1] && current <= _maxTimerIndexes[MiracleDrumCount - 1])
                {
                    //When the drum hit is miracle, it doesn't matter how perfect it is, really
                    return new RhythmInputModel(
                        DrumType.Don,
                        RhythmTimer.Count,
                        DrumHitStatus.Perfect
                    );
                }
                else
                {
                    ResetCounter();
                    return RhythmInputModel.Miss(DrumType.Don);
                }
            }
        }
        /// <summary>
        /// Start miracle counting.
        /// </summary>
        internal void StartCounter(int count)
        {
            if (TurnCounter.IsOn && !EnteredMiracleHit)
            {
                EnteredMiracleHit = true;
                RhythmTimer.OnHalfTime.RemoveListener(SetEnable);
                MiracleDrumCount = count;
            }
            else
            {
                MiracleDrumCount++;
            }
        }
        /// <summary>
        /// Stop(reset) miracle counting.
        /// </summary>
        internal void ResetCounter()
        {
            MiracleDrumCount = 0;
            if (!EnteredMiracleHit) return;
            RhythmTimer.OnHalfTime.AddListener(SetEnable);
            EnteredMiracleHit = false;
            StopAllCoroutines();
        }

        //------------- inherit
        private void OnEnable() => Enable();
        private void OnDisable() => Disable();
        private void OnDestroy() => Destroy();
        private void OnValidate()
        {
            if (_newGoodRange > RhythmEnvironment.InputInterval / 4)
            {
                throw new ArgumentException("The good range cannot be more than " + RhythmEnvironment.InputInterval / 4);
            }
        }
    }
}
