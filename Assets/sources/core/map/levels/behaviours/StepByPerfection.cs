using PataRoad.Core.Character;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class StepByPerfection : MonoBehaviour
    {
        [SerializeField]
        private float _minStepMultiplier;
        [SerializeField]
        private float _maxStepMultiplier;
        private void Start()
        {
            var pataponsManager = FindObjectOfType<Character.Patapons.PataponsManager>();
            pataponsManager.SetMinMaxStepRatio(_minStepMultiplier, _maxStepMultiplier);
            foreach (var patapon in pataponsManager.GetComponentsInChildren<Character.Patapons.Patapon>())
            {
                patapon.StatOperator.Add(new MovementSpeedChanger(patapon, _minStepMultiplier, _maxStepMultiplier));
            }
        }
    }
    class MovementSpeedChanger : IStatOperation
    {
        private readonly Character.Patapons.Patapon _patapon;
        private float _min;
        private float _max;
        internal MovementSpeedChanger(Character.Patapons.Patapon patapon, float min, float max)
        {
            _patapon = patapon;
            _min = min;
            _max = max;
        }
        public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
        {
            input.MovementSpeed *= Mathf.Lerp(_min, _max, _patapon.LastPerfectionRate);
            return input;
        }
    }
}
