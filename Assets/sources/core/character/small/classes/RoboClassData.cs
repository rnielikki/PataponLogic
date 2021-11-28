using PataRoad.Core.Rhythm.Command;

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
        protected override void InitLateForClass()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-charge", GetAttackMoveModel("attack-charge", attackDistance: 4.5f) },
                }
                );
        }
        public override void Attack()
        {
            if (!_character.Charged)
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
