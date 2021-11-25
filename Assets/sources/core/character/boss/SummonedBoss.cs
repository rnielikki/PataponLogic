using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class SummonedBoss : Boss
    {
        public override Vector2 MovingDirection { get; } = Vector2.right;
        private Transform _pataponManagerTransform;
        public override float DefaultWorldPosition => _pataponManagerTransform.position.x;

        protected override void Init(BossAttackData data)
        {
            base.Init(data);
            _pataponManagerTransform = FindObjectOfType<Patapons.PataponsManager>().transform;
            DistanceCalculator = DistanceCalculator.GetBossDistanceCalculator(this);
        }
    }
}
