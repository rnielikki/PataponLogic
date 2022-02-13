﻿using PataRoad.Core.Character.Patapons;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class DefenceStaff : StatUpdatingStaff
    {
        private readonly List<Animator> _activationPrefabs = new List<Animator>();

        protected override void InitEach(Patapon patapon)
        {
            var prefab = Instantiate(_prefabForActivation, patapon.RootTransform.Find(SmallCharacter.CharacterBodyName));
            _activationPrefabs.Add(prefab.GetComponent<Animator>());
            prefab.gameObject.SetActive(false);
        }
        protected override void PerformActionEach(Patapon patapon)
        {
            patapon.Stat.DefenceMin += 1.5f;
            patapon.Stat.DefenceMax += 2f;
        }
        protected override void PerformAnimation()
        {
            foreach (var anim in _activationPrefabs)
            {
                if (anim == null) continue;
                anim.gameObject.SetActive(true);
                anim.Play("Flash");
            }
        }
    }
}