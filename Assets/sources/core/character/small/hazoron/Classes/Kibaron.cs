﻿using PataRoad.Core.Character.Patapon;
using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Hazoron
{
    public class Kibaron : Hazoron
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 100,
                Defence = 1,
                DamageMin = 10,
                DamageMax = 20,
                AttackSeconds = 2,
                MovementSpeed = 8,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0.5f,
                KnockbackResistance = 0.1f,
                Stagger = 0.1f,
                StaggerResistance = 0.1f,
                FireRate = 0.1f,
                FireResistance = 0.1f,
                IceRate = 0.1f,
                IceResistance = 0.1f,
                SleepRate = 0.1f,
                SleepResistance = 0.1f
            };
        }
        private void Awake()
        {
            Init();
            InitDistanceFromHead(0);
            AttackType = Equipment.Weapon.AttackType.Stab;
            Class = ClassType.Kibapon;
        }
        private void Start()
        {
            SetAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever", AttackMoveType.Rush) },
                }
                );
            StartAttack("attack-fever");
        }
    }
}
