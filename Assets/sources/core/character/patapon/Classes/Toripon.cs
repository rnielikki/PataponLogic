namespace Core.Character.Patapon
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

        protected override float _attackDistance { get; set; } = 11.5f;
        protected override float _moveVelocity { get; set; } = 9;

        public override void Act(Rhythm.Command.CommandSong song, bool isFever)
        {
            base.Act(song, isFever);
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
        private void Awake()
        {
            Init();
            Class = ClassType.Toripon;
        }
        protected override void Attack(bool isFever)
        {
            if (!isFever && !_charged)
            {
                base.Attack(false);
            }
            else
            {
                AttackInTime("attack-fever");
            }
        }
        private void FlyUp()
        {
            _animator.AnimateFrom("tori-fly-up");
            _isFever = true;
        }
        private void FlyDown()
        {
            _animator.AnimateFrom("tori-fly-down");
            _isFever = false;
        }
    }
}
