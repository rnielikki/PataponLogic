using Core.Rhythm.Command;
using System;
using System.Collections;
using UnityEngine;

namespace Core.Rhythm
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

        int[] _timerIndexes;
        private void Awake()
        {
            int _newGoodFrequency = (_newGoodRange == 0) ? (int)(RhythmTimer.GoodFrequency * 0.75) : (int)(_newGoodRange / Time.fixedDeltaTime);
            if (DrumType != DrumType.Don)
            {
                throw new ArgumentException("Only DON Miracle drum is supported");
            }
            TurnCounter.OnTurn.AddListener(() => { if (!TurnCounter.IsPlayerTurn) ResetCounter(); });
            _timerIndexes = new int[]
            {
                RhythmTimer.HalfFrequency - RhythmTimer.GoodFrequency,
                RhythmTimer.HalfFrequency + _newGoodFrequency,
                RhythmTimer.Frequency - _newGoodFrequency,
                RhythmTimer.Frequency + RhythmTimer.GoodFrequency,
                RhythmTimer.Frequency * 2 - RhythmTimer.GoodFrequency,
                RhythmTimer.Frequency * 2 + _newGoodFrequency,
                RhythmTimer.Frequency * 2 + RhythmTimer.HalfFrequency - _newGoodFrequency,
                RhythmTimer.Frequency * 2 + RhythmTimer.HalfFrequency + RhythmTimer.GoodFrequency
            };
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
        private IEnumerator CountMiracle()
        {
            if (MiracleDrumCount == 0)
            {
                throw new InvalidOperationException("Miracle drum count cannot be zero when CountMiracle() is called!");
            }
            Disabled = true;
            while (EnteredMiracleHit && MiracleDrumCount <= 5)
            {
                var index = MiracleDrumCount * 2 + (!Disabled ? -2 : -1);
                yield return new WaitForRhythmTime(_timerIndexes[index]);
                Disabled = !Disabled;
            }
        }

        /// <summary>
        /// Start miracle counting.
        /// </summary>
        internal void StartCounter()
        {
            if (EnteredMiracleHit) return;
            RhythmTimer.OnHalfTime.RemoveListener(SetEnable);
            EnteredMiracleHit = true;
            StartCoroutine(CountMiracle());
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
    }
}
