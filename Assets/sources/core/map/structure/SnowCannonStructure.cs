namespace PataRoad.Core.Character
{
    class SnowCannonStructure : CannonStructure
    {
        protected override void Start()
        {
            base.Start();
            Stat.Knockback++;
            Stat.IceRate++;
            _animator.SetBool("return-to-idle", false);
        }
        public override void SetLevel(int level, int absoluteMaxLevel)
        {
            base.SetLevel(level, absoluteMaxLevel);
            Stat.Knockback += 0.04f * level;
            Stat.IceRate += 0.04f * level;
        }
        public override void SetAttack()
        {
            _started = true;
            _animator.Play("attack");
        }
        protected override void Attack()
        {
            ThrowBullet();
        }
        private void LateUpdate()
        {
            //do nothing.
        }
    }
}
