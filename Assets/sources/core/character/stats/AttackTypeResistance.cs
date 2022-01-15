using PataRoad.Core.Character.Equipments.Weapons;
using System.Collections.Generic;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Unlike stat, weapon WON'T change this and this is own property of a character.
    /// </summary>
    [System.Serializable]
    public class AttackTypeResistance
    {
        [UnityEngine.SerializeField]
        private float _crushMultiplier = 1;
        public float CrushMultipler => _crushMultiplier;

        [UnityEngine.SerializeField]
        private float _slashMultiplier = 1;
        public float SlashMultipler => _slashMultiplier;

        [UnityEngine.SerializeField]
        private float _stabMultiplier = 1;
        public float StabMultipler => _stabMultiplier;

        [UnityEngine.SerializeField]
        private float _soundMultiplier = 1;
        public float SoundMultipler => _soundMultiplier;

        [UnityEngine.SerializeField]
        private float _magicMultiplier = 1;
        public float MagicMultipler => _magicMultiplier;

        [UnityEngine.SerializeField]
        private float _fireMultiplier = 1;
        public float FireMultiplier => _fireMultiplier;

        [UnityEngine.SerializeField]
        private float _iceMultiplier = 1;
        public float IceMultiplier => _iceMultiplier;

        [UnityEngine.SerializeField]
        private float _thunderMultiplier = 1;
        public float ThunderMultiplier => _thunderMultiplier;

        private Dictionary<AttackType, float> _map;
        private Dictionary<ElementalAttackType, float> _elementalMap;

        public AttackTypeResistance()
        {
            _map = new Dictionary<AttackType, float>()
            {
                { AttackType.Neutral, 1},
                { AttackType.Crush, _crushMultiplier},
                { AttackType.Slash, _slashMultiplier},
                { AttackType.Stab, _stabMultiplier},
                { AttackType.Sound, _soundMultiplier},
                { AttackType.Magic, _magicMultiplier},
            };
            _elementalMap = new Dictionary<ElementalAttackType, float>()
            {
                { ElementalAttackType.Neutral, 1},
                { ElementalAttackType.Fire, _crushMultiplier},
                { ElementalAttackType.Ice, _slashMultiplier},
                { ElementalAttackType.Thunder, _stabMultiplier},
            };
        }
        public float GetMultiplier(AttackType attackType) => _map[attackType];
        public float GetMultiplier(ElementalAttackType attackType) => _elementalMap[attackType];
        /// <summary>
        /// Returns applied default resistance data to Rarepons data. It multiplies each value and returns new instance.
        /// </summary>
        /// <param name="resistances">The data to apply.</param>
        /// <returns>Multiplied data, as new instance.</returns>
        /// <note>This will return same value if parameter and calling instance is exchanged.</note>
        public AttackTypeResistance Apply(AttackTypeResistance resistances)
        {
            return new AttackTypeResistance()
            {
                _crushMultiplier = _crushMultiplier * resistances.CrushMultipler,
                _slashMultiplier = _slashMultiplier * resistances.SlashMultipler,
                _stabMultiplier = _stabMultiplier * resistances.StabMultipler,
                _soundMultiplier = _soundMultiplier * resistances.SoundMultipler,
                _magicMultiplier = _magicMultiplier * resistances.MagicMultipler,
                _fireMultiplier = _fireMultiplier * resistances.FireMultiplier,
                _iceMultiplier = _iceMultiplier * resistances.IceMultiplier,
                _thunderMultiplier = _thunderMultiplier * resistances.ThunderMultiplier
            };
        }
    }
}
