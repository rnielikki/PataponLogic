using Core.Character.Equipment;
using Core.Character.Equipment.Weapon;
using Core.Character.Patapon;
using UnityEngine;

namespace Core.Character
{
    /// <summary>
    /// Represents small character, including Patapon and its enemy (Hazoron).
    /// </summary>
    public abstract class SmallCharacter : MonoBehaviour, ICharacter
    {
        /// <summary>
        /// Helm that the character is currently using.
        /// <note>This will do nothing unless if <see cref="Rarepon"/> is set to normal.</note>
        /// </summary>
        public IEquipment Helm { get; protected set; }
        /// <summary>
        /// Weapon (e.g. spear, swrod...) that the character uses.
        /// </summary>
        public WeaponObject Weapon { get; protected set; }
        /// <summary>
        /// Protector (e.g. horse, shoes, shield, shoulder...) that Patapon uses.
        /// <note>This is useless for specific classes, like Yaripon or Yumipon.</note>
        /// </summary>
        public IEquipment Protector { get; protected set; }
        /// <summary>
        /// Current Stat that a Patapon has. Any effect can modify it.
        /// </summary>
        public Stat Stat { get; protected set; }
        /// <summary>
        /// Default stat that a bit varies for each class.
        /// </summary>
        protected abstract Stat DefaultStat { get; }
        /// <summary>
        /// Current Hit point.
        /// <remarks>It shouldn't be bigger than <see cref="Stat.HitPoint"/> or smaller than 0. If this value is 0, it causes death.</remarks>
        /// </summary>

        public int CurrentHitPoint { get; protected set; }

        /// <summary>
        /// Class (e.g. Yaripon, Tatepon, Yumipon...) of the Patapon.
        /// </summary>
        public ClassType Class { get; protected set; }

        /// <summary>
        /// Simple animator that moves patapons.
        /// </summary>
        public CharacterAnimator CharAnimator { get; protected set; }

        public DistanceCalculator DistanceCalculator { get; protected set; }

        /// <summary>
        /// Attack distance WITHOUT head size. Zero for melee expected. Some range units will add the distance by Tailwind.
        /// </summary>
        public virtual float AttackDistance { get; set; }
        /// <summary>
        /// Character size offest from center. Patapon head size, but if they have vehicle, it's depending on vehicle's head.
        /// </summary>

        public float CharacterSize { get; protected set; }

        public AttackType AttackType { get; protected set; }

        protected AttackMoveController _attackController { get; private set; }
        public IAttackMoveData AttackMoveData { get; protected set; }

        public virtual void Die()
        {
            StopAttacking();
            StartCoroutine(WaitUntilDie());
            System.Collections.IEnumerator WaitUntilDie()
            {
                CharAnimator.Animate("die");
                yield return new WaitForSeconds(1);
                Destroy(gameObject);
            }
        }
        public void WeaponAttack(AttackCommandType type) => Weapon.Attack(type);

        protected virtual void StopAttacking()
        {
            StopWeaponAttacking();
            _attackController.StopAttack();
        }

        /// <summary>
        /// Performs attack animation, applying attack seconds in stat.
        /// </summary>
        /// <param name="animationType">Animation name in animator.</param>
        /// <note>If this doesn't attack when Patapon is too fast, check if *AttackMultiplyer* is applied to the *Animation* in Animator.</note>
        protected void StartAttack(string animationType)
        {
            _attackController.StartAttack(animationType);
        }

        protected virtual void StopWeaponAttacking() => Weapon.StopAttacking();

        protected virtual AttackMoveController SetAttackMoveController()
        {
            _attackController = gameObject.AddComponent<AttackMoveController>();
            return _attackController;
        }
        protected AttackMoveController AddDefaultModelsToAttackMoveController()
        {
            if (_attackController == null) SetAttackMoveController();
            _attackController
                .AddModels(new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack", GetAttackMoveModel("attack") },
                    { "defend", GetAttackMoveModel("defend", AttackMoveType.Defend) },
                });
            return _attackController;
        }
        /// <summary>
        /// Get Attack move model based on Patapon default stats.
        /// </summary>
        /// <param name="animationType">Animation name in animator.</param>
        /// <param name="type">Telling attack movement type, if it's attack, defend or rush.</param>
        /// <param name="movingSpeed">Moving speed MULTIPLIER. It automatically multiplies to <see cref="Stat.MovementSpeed"/>.</param>
        /// <param name="attackSpeedMultiplier">Attack speed multiplier, default is 1. Yumipon fever attack is expected to 3.</param>
        /// <param name="attackDistance">Attack distance. default distance value is <see cref="AttackDistance"/>.</param>
        /// <returns>Attack Move Model for <see cref="AttackMoveController"/>.</returns>
        protected AttackMoveModel GetAttackMoveModel(string animationType, AttackMoveType type = AttackMoveType.Attack, float movingSpeed = 1, float attackSpeedMultiplier = 1, float attackDistance = -1)
        {
            movingSpeed *= Stat.MovementSpeed;
            return new AttackMoveModel(
                this,
                animationType,
                type,
                movingSpeed,
                attackSpeedMultiplier,
                attackDistance
                );
        }

        public abstract int GetAttackDamage();

        public void OnAttackHit(Vector2 point) => AttackMoveData.WasHitLastTime = true;
        public void OnAttackMiss(Vector2 point)
        {
            AttackMoveData.LastHit = point;
            AttackMoveData.WasHitLastTime = false;
        }

        public virtual void TakeDamage(int damage) => CurrentHitPoint -= damage;
    }
}
