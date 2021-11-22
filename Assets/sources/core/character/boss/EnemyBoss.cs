using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class EnemyBoss : Boss
    {
        public override Vector2 MovingDirection { get; } = Vector2.left;

        protected override void Init(BossAttackData data)
        {
            base.Init(data);
            DistanceCalculator = DistanceCalculator.GetBossDistanceCalculator(this);
        }
        public override void Die()
        {
            base.Die();
            StartCoroutine(WaitAndCelebrate());
            System.Collections.IEnumerator WaitAndCelebrate()
            {
                yield return new WaitForSeconds(4);
                Map.MissionPoint.Current.FilledMissionCondition = true;
                Map.MissionPoint.Current.EndMission();
            }
        }
    }
}
