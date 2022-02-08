using PataRoad.Core.Character.Equipments.Weapons;
using UnityEngine;

namespace PataRoad.Core.Character
{
    class CannonStructure : Structure, IAttacker
    {
        public float CharacterSize { get; private set; }

        [SerializeField]
        private AttackType _attackType;
        public AttackType AttackType => _attackType;

        [SerializeField]
        private ElementalAttackType _elementalAttackType;
        public ElementalAttackType ElementalAttackType => _elementalAttackType;

        public float GetAttackValueOffset() => 1;

        public void OnAttackHit(Vector2 point, int damage)
        {
            //keep attacking
        }

        public void OnAttackMiss(Vector2 point)
        {
            //adjust angle
        }
        public override void SetLevel(int level, int absoluteMaxLevel)
        {
            base.SetLevel(level, absoluteMaxLevel);
            Stat.AddDamage(level * 2);
        }
    }
}
