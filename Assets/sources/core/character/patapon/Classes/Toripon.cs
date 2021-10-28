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
            var birdHead = transform.Find("Root/Protector/Bird-Head");
            _pataponDistance.InitDistance(
                0.25f,
                (birdHead.transform.position - transform.position).x + birdHead.GetComponent<UnityEngine.CapsuleCollider2D>().size.x + 1
                );
            Class = ClassType.Toripon;
        }

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
