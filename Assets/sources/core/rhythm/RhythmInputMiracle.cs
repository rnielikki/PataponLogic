using PataRoad.Core.Rhythm.Command;
using System;
using UnityEngine;

namespace PataRoad.Core.Rhythm
{
    /// <summary>
    /// Sends one drum signal, like <see cref="RhythmInput"/>, but also extended to manual miracle hearing.
    /// </summary>
    internal class RhythmInputMiracle : RhythmInput
    {
        /// <summary>
        /// Reperesents 'Should send miracle signal'. DOESN'T directly affect <see cref="MiracleDrumCount"/>.
        /// </summary>
        internal bool EnteredMiracleHit { get; private set; }
        internal int MiracleDrumCount { get; private set; }

        int[] _minTimerIndexes;
        int[] _maxTimerIndexes;
        bool _hasResetListener;

        private void Awake()
        {
            int newHalfGoodFrequency = (RhythmEnvironment.MiracleRange == 0) ? (int)(RhythmTimer.GoodFrequency * 0.5) : (int)(RhythmEnvironment.MiracleRange / Time.fixedDeltaTime);
            int newGoodFrequency = newHalfGoodFrequency * 2;
            if (DrumType != DrumType.Don)
            {
                throw new ArgumentException("Only DON Miracle drum is supported");
            }
            TurnCounter.OnTurn.AddListener(() => { if (!TurnCounter.IsPlayerTurn) ResetCounter(); });
            _minTimerIndexes = new int[]
            {
                -newGoodFrequency, //2
                -newHalfGoodFrequency, //3
                -newGoodFrequency, //4
                -newHalfGoodFrequency //5
            };
            _maxTimerIndexes = new int[]
            {
                newHalfGoodFrequency, //2
                newGoodFrequency, //3
                newHalfGoodFrequency, //4
                newGoodFrequency //5
            };
            Init();
        }
        protected override void SetResetTimer()
        {
            if (_hasResetListener) return;
            RhythmTimer.Current.OnHalfTime.AddListener(SetEnable);
            _hasResetListener = true;
        }
        protected override RhythmInputModel GetInputModel()
        {
            if (!EnteredMiracleHit)
            {
                return base.GetInputModel();
            }
            else
            {
                int current;
                switch (MiracleDrumCount)
                {
                    case 1: //this is 2nd
                    case 4: //this is 5th
                        current = RhythmTimer.GetTimingWithDirection(RhythmTimer.GetDrumCount());
                        break;
                    case 2:
                    case 3:
                        current = RhythmTimer.GetTimingWithDirection(
                            (RhythmTimer.GetDrumCount() + RhythmTimer.HalfFrequency) % RhythmTimer.Frequency);
                        break;
                    default:
                        throw new InvalidOperationException($"The {MiracleDrumCount}th miracle drum count isn't valid");
                }
                var minTimer = _minTimerIndexes[MiracleDrumCount - 1];
                var maxTimer = _maxTimerIndexes[MiracleDrumCount - 1];
                if (current >= minTimer && current <= maxTimer)
                {
                    var perfectionRate = Mathf.Abs(Mathf.InverseLerp(minTimer, maxTimer, current) - 0.5f);
                    DrumHitStatus status;
                    if (perfectionRate <= RhythmEnvironment.PerfectRange * 1.4f / RhythmEnvironment.HalfInputInterval)
                    {
                        status = DrumHitStatus.Perfect;
                    }
                    else if (perfectionRate <= RhythmEnvironment.GoodRange * 1.4f / RhythmEnvironment.HalfInputInterval)
                    {
                        status = DrumHitStatus.Good;
                    }
                    else
                    {
                        status = DrumHitStatus.Bad;
                    }
                    return new RhythmInputModel(
                        DrumType.Don,
                        RhythmTimer.Count,
                        status
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
                RhythmTimer.Current.OnHalfTime.RemoveListener(SetEnable);
                _hasResetListener = false;
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
            SetResetTimer();
            EnteredMiracleHit = false;
            StopAllCoroutines();
        }

        //------------- inherit
        private void OnEnable() => Enable();
        private void OnDisable() => Disable();
        private void OnDestroy() => Destroy();
    }
}
