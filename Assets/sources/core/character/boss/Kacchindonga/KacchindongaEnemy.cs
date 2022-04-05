namespace PataRoad.Core.Character.Bosses
{
    public class KacchindongaEnemy : EnemyBossBehaviour
    {
        protected override void Init()
        {
            _minCombo = 2;
        }
        protected override BossAttackMoveSegment GetNextBehaviour()
        {
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon == null || (firstPon.Type != Class.ClassType.Toripon &&
                firstPon.transform.position.x < _pataponsManager.transform.position.x))
            {
                return new BossAttackMoveSegment("growl", 2);
            }
            if ((firstPon.IsMeleeUnit || firstPon.Type == Class.ClassType.Toripon)
                && Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 20))
            {
                if (_level >= 3 && Common.Utils.RandomByProbability((float)_pataponsManager.PataponCount / 18))
                {
                    return new BossAttackMoveSegment("eat", 0);
                }
                else
                {
                    return new BossAttackMoveSegment("headbutt", 0);
                }
            }
            else
            {
                if (Common.Utils.RandomByProbability(0.5f + _level * 0.01f))
                {
                    return new BossAttackMoveSegment("growl", 5);
                }
                else
                {
                    return new BossAttackMoveSegment("fire", 3);
                }
            }
        }

        protected override string GetNextBehaviourOnIce()
        {
            return Common.Utils.RandomByProbability(0.5f + _level * 0.01f) ? "growl" : "fire";
        }
    }
}