﻿namespace Core.Character.Patapon
{
    public class Toripon : Patapon
    {
        /// <summary>
        /// Determines if it needs to fly high.
        /// </summary>
        private bool _isFever;
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 200,
                Defence = 1.2f,
                DamageMin = 1,
                DamageMax = 11,
                AttackSeconds = 2,
                MovementSpeed = 9,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0.35f,
                KnockbackResistance = 0.2f,
                Stagger = 0.15f,
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
            AttackDistance = 0.5f;
            CharacterSize = transform.Find("Root/Patapon-body/Face").GetComponent<UnityEngine.CircleCollider2D>().radius + AttackDistance;
            AttackType = Equipment.Weapon.AttackType.Stab;
            Class = ClassType.Toripon;

        }
        private void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever") },
                }
                );
        }

        public override void Act(Rhythm.Command.RhythmCommandModel model)
        {
            base.Act(model);
            var isFever = model.ComboType == Rhythm.Command.ComboStatus.Fever;
            if (!_isFever && isFever)
            {
                FlyUp();
            }
            else if (_isFever && !isFever)
            {
                FlyDown();
            }
        }
        public override void PlayIdle()
        {
            base.PlayIdle();
            if (_isFever)
            {
                FlyDown();
            }
        }

        protected override void Attack(bool isFever)
        {
            if (!isFever && !_charged)
            {
                base.Attack(false);
            }
            else
            {
                StartAttack("attack-fever");
            }
        }
        private void FlyUp()
        {
            CharAnimator.AnimateFrom("tori-fly-up");
            _isFever = true;
        }
        private void FlyDown()
        {
            CharAnimator.AnimateFrom("tori-fly-down");
            _isFever = false;
        }
    }
}
