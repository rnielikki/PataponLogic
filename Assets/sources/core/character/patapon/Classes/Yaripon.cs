using Core.Character.Equipment;
using UnityEngine;

namespace Core.Character.Patapon
{
    public class Yaripon : Patapon
    {
        private void Awake()
        {
            Init();
            Class = ClassType.Yaripon;
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
            }
        }
    }
}
