﻿namespace PataRoad.Core.Character.Patapons
{
    public class Tatepon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 250,
                Defence = 1.5f,
                DamageMin = 6,
                DamageMax = 8,
                AttackSeconds = 2,
                MovementSpeed = 8,
                Critical = 0.1f,
                CriticalResistance = 0.25f,
                Knockback = 0.1f,
                KnockbackResistance = 0.2f,
                Stagger = 0.05f,
                StaggerResistance = 0.2f,
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
            Class = ClassType.Tatepon;
            AttackType = Equipment.Weapon.AttackType.Slash;
        }
        void Start()
        {
            SetAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack", GetAttackMoveModel("attack") },
                    { "attack-charge", GetAttackMoveModel("attack-charge", AttackMoveType.Rush, movingSpeed: 1.2f) },
                }
                );
        }
        protected override void Attack()
        {
            if (Charged)
            {
                StartAttack("attack-charge");
            }
            else base.Attack();
        }

        protected override void Defend()
        {
            if (!OnFever && !Charged)
            {
                CharAnimator.Animate("defend");
            }
            else
            {
                CharAnimator.Animate("defend-fever");
            }
            DistanceManager.MoveTo(0.75f, Stat.MovementSpeed);
        }
        protected override void Charge()
        {
            base.Charge();
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
    }
}