using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Manages hit point from sprite library and updates phase as image.
    /// </summary>
    internal class StructureImages : MonoBehaviour
    {
        [SerializeField]
        private StructurePhaseCollection _phaseImageReference;
        private SpriteRenderer _renderer;
        public StructurePhaseCollection Reference => _phaseImageReference;

        float[] _phases;
        int _currentPhase;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            var phaseLength = _phaseImageReference.Length;
            _phases = new float[phaseLength];

            for (int i = 0; i < phaseLength; i++)
            {
                _phases[phaseLength - i - 1] = (float)i / phaseLength;
            }
            _renderer.sprite = _phaseImageReference.Resolve(0);
        }
        internal void Evaluate(float percent)
        {
            var phase = GetPhase(percent);
            if (_currentPhase != phase)
            {
                _currentPhase = phase;
                _renderer.sprite = _phaseImageReference.Resolve(phase);
            }
        }
        private int GetPhase(float percent)
        {
            int phase = 0;
            foreach (var phaseRatio in _phases)
            {
                if (phaseRatio <= percent)
                {
                    return phase;
                }
                phase++;
            }
            return phase;
        }
    }
}