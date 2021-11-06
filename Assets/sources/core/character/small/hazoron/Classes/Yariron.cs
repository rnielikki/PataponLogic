namespace PataRoad.Core.Character.Hazoron
{
    public class Yariron : Hazoron
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 200,
                Defence = 1,
                DamageMin = 4,
                DamageMax = 10,
                AttackSeconds = 2,
                MovementSpeed = 8,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0.5f,
                KnockbackResistance = 0.1f,
                Stagger = 0.1f,
                StaggerResistance = 0.1f,
                FireRate = 0.0f,
                FireResistance = 0.1f,
                IceRate = 0.0f,
                IceResistance = 0.1f,
                SleepRate = 0.0f,
                SleepResistance = 0.1f
            };
        }
        private void Awake()
        {
            Init();
            AttackType = Equipment.Weapon.AttackType.Stab;
            InitDistanceFromHead(8.5f);
        }
        private void Start()
        {
            SetAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever") },
                }
                );
            StartAttack("attack-fever");
        }
    }
}
