using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    internal class DodongaSummon : SummonedBoss
    {
        private DodongaAttack _attack;

        protected override void Chakachaka()
        {
            _attack.AnimateFire();
        }

        protected override void ChargedChakachaka()
        {
            _attack.AnimateFire();
        }

        protected override void ChargedPonpon()
        {
            _attack.AnimateHeadbutt();
        }

        protected override void Ponpon()
        {
            _attack.AnimateHeadbutt();
        }

        private void Awake()
        {
            _attack = GetComponent<DodongaAttack>();
            Init(_attack);
            _offsetFromManager = 5;
        }
    }
}
