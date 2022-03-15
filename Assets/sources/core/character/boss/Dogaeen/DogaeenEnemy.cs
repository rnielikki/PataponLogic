namespace PataRoad.Core.Character.Bosses
{
    class DogaeenEnemy : EnemyBossBehaviour
    {
        private bool _usedLaser;
        private float __prob = -1;
        private float _probabilityFromLevel
        {
            get
            {
                if (__prob < 0) __prob = UnityEngine.Mathf.Log10(_level) * 0.5f;
                return __prob;
            }
        }
        protected override void Init()
        {
            Boss.UseWalkingBackAnimation();
        }

        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            var firstPon = _pataponsManager.FirstPatapon;
            var closest = Boss.DistanceCalculator.GetClosest();

            if (firstPon != null && firstPon.Type != Class.ClassType.Toripon)
            {
                //oh this is nightmare...
                if (closest != null && (
                    (transform.position.x - closest.Value.x <= 1 && Common.Utils.RandomByProbability(0.7f))
                    || (_pataponsManager.transform.position.x - closest.Value.x <= 2 && Common.Utils.RandomByProbability(0.6f))
                    ))
                {
                    _usedLaser = false;
                    return PartBroken ?
                        new BossAttackMoveSegment("repel", 0) :
                        new BossAttackMoveSegment("bodyslam", 0);
                }
                else
                {
                    if (closest == null)
                    {
                        if (!_usedLaser && Common.Utils.RandomByProbability(1 - __prob))
                        {
                            _usedLaser = true;
                            return new BossAttackMoveSegment("laser", 3);
                        }
                        else return SlamOrSlam(0.9f);
                    }
                    var distance = transform.position.x - closest.Value.x;
                    if (distance > 10)
                    {
                        if (!_usedLaser && Common.Utils.RandomByProbability(1 - __prob))
                        {
                            _usedLaser = true;
                            return new BossAttackMoveSegment("laser", 2);
                        }
                        else
                        {
                            return SlamOrSlam(0.8f);
                        }
                    }
                    return SlamOrSlam(0.1f);
                }
            }
            else
            {
                return SlamOrSlam(0.4f);
            }
        }
        private BossAttackMoveSegment SlamOrSlam(float probability)
        {
            _usedLaser = false;
            if (PartBroken) return new BossAttackMoveSegment("bodyslam", 0);
            var pro = UnityEngine.Mathf.Max(_probabilityFromLevel, probability);
            if (Common.Utils.RandomByProbability(pro))
            {
                return new BossAttackMoveSegment("slam", 0);
            }
            else return new BossAttackMoveSegment("bodyslam", 0);
        }

        protected override string GetNextBehaviourOnIce()
        {
            _usedLaser = true;
            return "laser";
        }
    }
}
