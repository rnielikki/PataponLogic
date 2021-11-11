namespace PataRoad.Core.Character.Patapons
{
    public class Toripon : Patapon
    {
        /// <summary>
        /// Determines if it needs to fly high.
        /// </summary>
        private bool _isFever;

        private void Awake()
        {
            RootName = "Root/";
            Init();
            Class = ClassType.Toripon;
        }
        private void Start()
        {
            AddDefaultModelsToAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack-fever", GetAttackMoveModel("attack-fever") },
                }
                );
            WeaponLoadTest("Weapons/Bird/1");
            WeaponLoadTest("Rarepons/Gekolo");
        }

        public override void Act(Rhythm.Command.RhythmCommandModel model)
        {
            base.Act(model);
            var isFever = model.ComboType == Rhythm.Command.ComboStatus.Fever;
            if (!_isFever && isFever)
            {
                FlyUp();
            }
            else if (_isFever && !isFever)
            {
                FlyDown();
            }
        }
        public override void PlayIdle()
        {
            base.PlayIdle();
            if (_isFever)
            {
                FlyDown();
            }
        }

        protected override void Attack()
        {
            if (!OnFever && !Charged)
            {
                base.Attack();
            }
            else
            {
                StartAttack("attack-fever");
            }
        }

        private void FlyUp()
        {
            CharAnimator.AnimateFrom("tori-fly-up");
            _isFever = true;
        }
        private void FlyDown()
        {
            CharAnimator.AnimateFrom("tori-fly-down");
            _isFever = false;
        }
        protected override void BeforeDie()
        {
            base.BeforeDie();
            CharAnimator.AnimateFrom("tori-fly-stop");
        }
    }
}
