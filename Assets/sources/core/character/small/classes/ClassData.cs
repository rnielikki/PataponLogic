﻿using UnityEngine;

namespace PataRoad.Core.Character.Class
{
    /// <summary>
    /// defines move and attack by class. Applies to both Patapons and Hazorons.
    /// </summary>
    public abstract class ClassData
    {
        protected readonly SmallCharacter _character;
        public bool IsMeleeUnit { get; protected set; }
        protected AttackMoveController _attackController { get; private set; }
        public IAttackMoveData AttackMoveData { get; protected set; }
        public void StopAttack(bool pause) => _attackController.StopAttack(pause);

        protected CharacterAnimator _animator;

        /// <summary>
        /// Root, which is parent of Patapon body. Default is empty, but Toripon has different root. Must add slash to end if it's not empty.
        /// </summary>
        public string RootName { get; protected set; } = "";
        public bool IsFlyingUnit { get; protected set; }
        public bool ChargeWithoutMove { get; protected set; }

        protected ClassData(SmallCharacter character)
        {
            _character = character;
        }
        public virtual void InitLate()
        {
            _animator = _character.CharAnimator;
            if (_character is Patapons.Patapon patapon) AttackMoveData = new PataponAttackMoveData(patapon);
            else if (_character is Hazorons.Hazoron hazoron) AttackMoveData = new HazoronAttackMoveData(hazoron);
            InitLateForClass();
        }
        protected abstract void InitLateForClass();
        public virtual void Attack()
        {
            _attackController.StartAttack("attack");
        }
        public virtual void Defend()
        {
            _attackController.StartAttack("defend");
        }
        public static ClassData GetClassData(SmallCharacter character, ClassType type)
        {
            switch (type)
            {
                case ClassType.Tatepon:
                    return new TateClassData(character);
                case ClassType.Dekapon:
                    return new DekaClassData(character);
                case ClassType.Robopon:
                    return new RoboClassData(character);
                case ClassType.Kibapon:
                    return new KibaClassData(character);
                case ClassType.Yaripon:
                    return new YariClassData(character);
                case ClassType.Megapon:
                    return new MegaClassData(character);
                case ClassType.Toripon:
                    return new ToriClassData(character);
                case ClassType.Yumipon:
                    return new YumiClassData(character);
                case ClassType.Mahopon:
                    return new MahoClassData(character);
                default:
                    throw new System.ArgumentException("class type data not valid");
            }
        }
        protected virtual AttackMoveController SetAttackMoveController()
        {
            _attackController = _character.gameObject.AddComponent<AttackMoveController>();
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
            movingSpeed *= _character.Stat.MovementSpeed;
            return new AttackMoveModel(
                _character,
                animationType,
                type,
                movingSpeed,
                attackSpeedMultiplier,
                attackDistance
                );
        }
        public virtual void OnAttackHit(Vector2 point, int damage) => AttackMoveData.WasHitLastTime = true;
        public void OnAttackMiss(Vector2 point)
        {
            AttackMoveData.LastHit = point;
            AttackMoveData.WasHitLastTime = false;
        }
        public virtual void OnAction(Rhythm.Command.RhythmCommandModel model)
        {
        }
        public virtual void OnCanceled()
        {
        }
    }
}