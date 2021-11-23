using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class SummonedBoss : Boss
    {
        public override Vector2 MovingDirection { get; } = Vector2.right;

        protected override void Init(BossAttackData data)
        {
            base.Init(data);
            DistanceCalculator = DistanceCalculator.GetBossDistanceCalculator(this);
        }
    }
}
