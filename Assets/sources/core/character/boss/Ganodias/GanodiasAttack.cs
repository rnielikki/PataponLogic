using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class GanodiasAttack : BossAttackData
    {
        private BossParticleCollision[] _particles;
        private GanodiasBomb _bomb;
        [SerializeField]
        private ParticleSystem _cannonInhale;
        [SerializeField]
        private Canvas _attackCanvas;
        [SerializeField]
        private GanodiasCannon _cannon;

        protected override void Init()
        {
            CharacterSize = 8;
            _particles = GetComponentsInChildren<BossParticleCollision>();
            _bomb = GetComponentInChildren<GanodiasBomb>(true);
            _attackCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            _attackCanvas.worldCamera = Camera.main;
            _attackCanvas.sortingLayerName = "background";
            _attackCanvas.sortingOrder = 99;
            base.Init();
        }
        public void BulletAttack()
        {
            foreach (var particle in _particles)
            {
                particle.Attack();
            }
        }
        public void InhaleCannon()
        {
            _cannonInhale.Play();
        }
        public void CannonAttack()
        {
            _cannon.Attack();
        }
        public void BombAttack()
        {
            _bomb.ShowBomb();
        }
        public override void StopAllAttacking()
        {
            base.StopAllAttacking();
            foreach (var particle in _particles)
            {
                particle.StopAttacking();
            }
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + level * 0.2f;
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.StaggerResistance += level * 0.05f;
            _stat.KnockbackResistance += level * 0.05f;
            _stat.FireResistance += level * 0.03f;
            _stat.IceResistance += level * 0.03f;
            _stat.SleepResistance += level * 0.03f;
        }
    }
}
