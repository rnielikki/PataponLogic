using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace PataRoad.Core.Character.Class
{
    internal class RoboClassData : ClassData
    {
        private readonly UnityEngine.GameObject _shield;
        internal RoboClassData(SmallCharacter character) : base(character)
        {
            IsMeleeUnit = true;
            _shield = _character.transform.Find(RootName + "shield").gameObject;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            switch (_attackType)
            {
                case 0:
                    AddDefaultModelsToAttackMoveController()
                        .AddModels(
                        new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                        {
                            { "attack-charge", GetAttackMoveModel("attack-charge", attackDistance: 4.5f) }
                        }
                        );
                    break;
                case 1:
                    SetAttackMoveController()
                        .AddModels(
                        new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                        {
                            { "attack", GetAttackMoveModel("attack-charge", AttackMoveType.Rush, movingSpeed: 1.2f) },
                            { "defend", GetAttackMoveModel("defend", AttackMoveType.Defend) }
                        }
                        );
                    realStat.DamageMax = Mathf.Max(1, realStat.DamageMax - 20);
                    realStat.DamageMin = Mathf.Max(1, realStat.DamageMax - 20);
                    break;
            }

        }
        public override void Attack()
        {
            if (!_character.Charged || _attackType == 1)
            {
                base.Attack();
            }
            else
            {
                _attackController.StartAttack("attack-charge");
            }
        }

        public override void Defend()
        {
            _animator.Animate("defend");
            _character.DistanceManager.MoveTo(0.75f, _character.Stat.MovementSpeed, true);
        }
        public override void OnAction(RhythmCommandModel model)
        {
            _shield.SetActive(model.Song == CommandSong.Chakachaka);
        }
        public override void OnCanceled()
        {
            _shield.SetActive(false);
        }
    }
}
