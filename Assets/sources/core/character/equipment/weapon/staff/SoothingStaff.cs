using PataRoad.Core.Character.Patapons;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class SoothingStaff : StatUpdatingStaff
    {
        private readonly List<ParticleSystem> _activationPrefabs = new List<ParticleSystem>();

        protected override void InitEach(Patapon patapon)
        {
            var prefab = Instantiate(_prefabForActivation, patapon.RootTransform.Find(SmallCharacter.CharacterBodyName));
            _activationPrefabs.Add(prefab.GetComponent<ParticleSystem>());
        }

        protected override void PerformActionEach(Patapon patapon)
        {
            patapon.StatusEffectManager.RecoverAndIgnoreEffect();
        }
        protected override void PerformAnimation()
        {
            foreach (var anim in _activationPrefabs)
            {
                if (anim == null) continue;
                anim.Play();
            }
        }
    }
}
