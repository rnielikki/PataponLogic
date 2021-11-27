using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    public class Robopon : Patapon
    {
        [SerializeField]
        GameObject _shield;
        private void Awake()
        {
            IsMeleeUnit = true;
            Init();
            Class = ClassType.Robopon;
        }
        void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-charge", GetAttackMoveModel("attack-charge", attackDistance: 4.5f) },
                }
                );
        }
        protected override void Attack()
        {
            if (!Charged)
            {
                base.Attack();
            }
            else
            {
                StartAttack("attack-charge");
            }
        }
        protected override void Defend()
        {
            _shield.SetActive(true);
            CharAnimator.Animate("defend");
            DistanceManager.MoveTo(0.75f, Stat.MovementSpeed, true);
        }
        protected override void Charge() => ChargeWithoutMoving();
        public override General.IGeneralEffect GetGeneralEffect() => new General.KonKimponEffect();
        public override void OnAttackHit(Vector2 point, int damage)
        {
            base.OnAttackHit(point, damage);
            if (IsGeneral && Common.Utils.RandomByProbability(0.05f)) StatusEffectManager.TumbleAttack();
        }
    }
}
