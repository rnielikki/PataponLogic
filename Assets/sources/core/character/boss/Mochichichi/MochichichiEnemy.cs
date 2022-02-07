namespace PataRoad.Core.Character.Bosses
{
    class MochichichiEnemy : EnemyBossBehaviour
    {
        protected override void Init()
        {
            _boss.StatusEffectManager.OnStatusEffect.AddListener(effect =>
            {
                if (effect == StatusEffectType.Stagger)
                {
                    _boss.BossTurnManager.DefineNextAction("fart");
                }
            });
            _boss.UseWalkingBackAnimation();
            CharacterSize = 4.3f;
        }
        //Example
        protected override (string action, float distance) GetNextBehaviour()
        {
            //Tada. depends on level, it does nothing!
            if (Common.Utils.RandomByProbability(1f / (_level + 5)))
            {
                return ("nothing", 1);
            }
            if (Common.Utils.RandomByProbability(1f / (_level + 8)))
            {
                return ("Idle", 1);
            }

            //Toripon. Any other attacks just don't work. Don't even troll the birb with birb.
            var firstPon = _pataponsManager.FirstPatapon;
            if (firstPon?.Type == Class.ClassType.Toripon) return ("tornado", 1);

            bool isMeleeUnit = firstPon?.IsMeleeUnit ?? false;
            if (_level >= 5 &&
                _pataponsManager.transform.position.x > transform.position.x)
            {
                if (Common.Utils.RandomByProbability(isMeleeUnit ? 0.6f : 0.3f))
                {
                    return ("tornado", 0);
                }
                else
                {
                    return ("slam", 0);
                }
            }
            var slamProbability = _pataponsManager.FirstPatapon.IsMeleeUnit ? 0.6f : 0.4f;
            if (Common.Utils.RandomByProbability(slamProbability))
            {
                return ("slam", 0);
            }
            else
            {
                return ("peck", 1);
            }
        }
    }
}
