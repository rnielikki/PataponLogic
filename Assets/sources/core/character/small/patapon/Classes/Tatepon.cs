using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    public class Tatepon : Patapon
    {
        [SerializeField]
        GameObject _shield;
        private void Awake()
        {
            IsMeleeUnit = true;
            Init();
            Class = ClassType.Tatepon;
        }
        void Start()
        {
            SetAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack", GetAttackMoveModel("attack") },
                    { "attack-charge", GetAttackMoveModel("attack-charge", AttackMoveType.Rush, movingSpeed: 1.2f) },
                }
                );
        }
        protected override void Attack()
        {
            if (Charged)
            {
                StartAttack("attack-charge");
            }
            else base.Attack();
        }

        protected override void Defend()
        {
            if (!OnFever && !Charged)
            {
                CharAnimator.Animate("defend");
                Stat.DefenceMin *= 1.5f;
                Stat.DefenceMax *= 1.8f;
            }
            else
            {
                CharAnimator.Animate("defend-fever");
                Stat.DefenceMin *= 2;
                Stat.DefenceMax *= 2.5f;
            }
            _shield.SetActive(true);
            DistanceManager.MoveTo(0.75f, Stat.MovementSpeed, true);
        }
        public override void StopAttacking()
        {
            base.StopAttacking();
            _shield.SetActive(false);
        }
        protected override void Charge()
        {
            base.Charge();
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
        public override General.IGeneralEffect GetGeneralEffect() => new General.RahGashaponEffect();

        public override void OnAttackHit(Vector2 point, int damage)
        {
            base.OnAttackHit(point, damage);
            //General group effect
            if (IsGeneral && LastSong == Rhythm.Command.CommandSong.Ponpon && Charged)
            {
                Group.HealAllInGroup((int)(damage * 0.1f));
            }
        }
    }
}
