using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace PataRoad.Core.Character.Class
{
    internal class RoboClassData : ClassData
    {
        private readonly GameObject _shield;
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
                        new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                        {
                            { AttackCommandType.ChargeAttack, GetAttackMoveModel("attack-charge") }
                        }
                        );
                    realStat.MultipleDamage(0.6f); //attacks twice.
                    break;
                case 1:
                    SetAttackMoveController()
                        .AddModels(
                        new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                        {
                            { AttackCommandType.Attack, GetAttackMoveModel("attack-charge") },
                            { AttackCommandType.Defend, GetAttackMoveModel("defend") }
                        }
                        );
                    realStat.MultipleDamage(0.85f);
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
                _attackController.StartAttack(AttackCommandType.ChargeAttack);
            }
        }

        public override void Defend()
        {
            _animator.Animate("defend");
            _character.DistanceManager.MoveTo(0.75f, _character.Stat.MovementSpeed, true);
        }
        public override void PerformCommandAction(CommandSong song)
        {
            _shield.SetActive(song == CommandSong.Chakachaka);
            if (_character.Charged && song == CommandSong.Ponpon)
            {
                _character.Stat.MultipleDamage(1.5f);
            }
        }
        public override void OnCanceled()
        {
            _shield.SetActive(false);
        }
    }
}
