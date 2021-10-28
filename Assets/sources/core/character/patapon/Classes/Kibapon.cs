namespace Core.Character.Patapon
{
    public class Kibapon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 220,
                Defence = 1.2f,
                DamageMin = 3,
                DamageMax = 8,
                AttackSeconds = 2,
                MovementSpeed = 12,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0.1f,
                KnockbackResistance = 0.3f,
                Stagger = 0.3f,
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
            var horseHead = transform.Find("Protector/Horse-Head");
            _pataponDistance.InitDistance(
                    0,
                    (horseHead.transform.position - transform.position).x + horseHead.GetComponent<UnityEngine.CapsuleCollider2D>().size.x + 1
                );
            Class = ClassType.Kibapon;
        }
        protected override void Attack(bool isFever)
        {
            if (!isFever && !_charged)
            {
                base.Attack(false);
            }
            else
            {
                _animator.Animate("attack-fever");
                _pataponDistance.MoveRush(Stat.MovementSpeed * 2);
            }
        }
        protected override void Defend(bool isFever)
        {
            if (!isFever && !_charged)
            {
                _animator.Animate("defend");
            }
            else
            {
                _animator.Animate("defend-fever");
            }
            _pataponDistance.MoveTo(-0.75f, Stat.MovementSpeed);
        }
    }
}
