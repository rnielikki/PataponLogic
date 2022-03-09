using PataRoad.Core.Character.Patapons;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class HealingStaff : StatUpdatingStaff
    {
        private readonly List<ParticleSystem> _activationPrefabs = new List<ParticleSystem>();

        protected override void InitEach(Patapon patapon)
        {
            var prefab = Instantiate(_prefabForActivation, patapon.RootTransform.Find(SmallCharacter.CharacterBodyName));
            _activationPrefabs.Add(prefab.GetComponent<ParticleSystem>());
        }
        public override void NormalAttack()
        {
        }
        public override void ChargeAttack()
        {
            if (CanPerform()) _manager.HealAll(GetAmount());
        }
        public override void Defend()
        {
            if (CanPerform())
            {
                if (_holder.Charged) _manager.HealAll(GetAmount() * 3);
                else _manager.HealAll(GetAmount() * 2);
            }
        }
        private int GetAmount() => Mathf.RoundToInt(Mathf.Lerp(_holder.Stat.DamageMin, _holder.Stat.DamageMax, _holder.LastPerfectionRate));

        protected override void PerformAnimation()
        {
            foreach (var anim in _activationPrefabs)
            {
                if (anim == null) continue;
                anim.Play();
            }
        }
        protected override void PerformActionEach(Patapon patapon)
        {
            //sorry you don't need
        }
    }
}
