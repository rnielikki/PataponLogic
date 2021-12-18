using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Class
{
    internal class TateClassData : ClassData
    {
        private readonly UnityEngine.GameObject _shield;
        internal TateClassData(SmallCharacter character) : base(character)
        {
            IsMeleeUnit = true;
            _shield = _character.transform.Find(RootName + "shield").gameObject;
        }
        protected override void InitLateForClass(Stat realStat)
        {
            switch (_attackType)
            {
                case 0:
                    SetAttackMoveController()
                        .AddModels(
                        new System.Collections.Generic.Dictionary<AttackCommandType, AttackMoveModel>()
                        {
                            { AttackCommandType.Attack, GetAttackMoveModel("attack") },
                            { AttackCommandType.ChargeAttack, GetAttackMoveModel("attack-charge", AttackMoveType.Rush, movingSpeed: 1.2f) },
                        }
                        );
                    break;
                case 1:
                    SetAttackMoveController();
                    realStat.DefenceMax *= 5;
                    realStat.DefenceMin *= 3;
                    break;
            }
        }
        public override void Attack()
        {
            if (_attackType == 1)
            {
                Defend();
                return;
            }
            if (_character.Charged)
            {
                _attackController.StartAttack("attack-charge");
            }
            else base.Attack();
        }

        public override void Defend()
        {
            if (!_character.OnFever && !_character.Charged)
            {
                _animator.Animate("defend");
            }
            else
            {
                _animator.Animate("defend-fever");
            }
            _character.DistanceManager.MoveTo(0.75f, _character.Stat.MovementSpeed, true);
        }
        public override void OnAction(RhythmCommandModel model)
        {
            _shield.SetActive(model.Song == CommandSong.Chakachaka);
            //"Only works with command"
            if (model.Song == CommandSong.Chakachaka)
            {
                if (model.ComboType == ComboStatus.Fever)
                {
                    _character.Stat.DefenceMin *= 2;
                    _character.Stat.DefenceMax *= 2.5f;
                }
                else
                {
                    _character.Stat.DefenceMin *= 1.5f;
                    _character.Stat.DefenceMax *= 1.8f;
                }
            }
            if (model.Song == CommandSong.Patapata && _character.OnFever)
            {
                _character.Stat.DefenceMin *= 1.5f;
                _character.Stat.DefenceMax *= 2f;
            }
        }
        public override void OnCanceled()
        {
            _shield.SetActive(false);
        }
    }
}
