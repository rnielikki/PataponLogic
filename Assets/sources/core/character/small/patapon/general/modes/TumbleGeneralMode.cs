using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    public class TumbleGeneralMode : GeneralMode
    {
        public override CommandSong ActivationCommand => CommandSong.Dondon;
        private bool _isOn;
        private float _knockbackChance;
        public override void Init()
        {
            //This is no Patapon3 so you don't need almost every time perfect
            switch (Rhythm.RhythmEnvironment.Difficulty)
            {
                case Rhythm.Difficulty.Easy:
                    _knockbackChance = 0.4f;
                    break;
                case Rhythm.Difficulty.Normal:
                    _knockbackChance = 0.6f;
                    break;
                case Rhythm.Difficulty.Hard:
                    _knockbackChance = 0.8f;
                    break;
            }
        }

        public override void Activate(PataponGroup group)
        {
            //activated only once in same turn.
            if (_isOn) return;
            _isOn = true;
            foreach (var target in group.FirstPon.DistanceCalculator.GetAllGroundedTargets())
            {
                switch (target)
                {
                    case SmallCharacter small:
                        small.StatusEffectManager.Tumble();
                        break;
                    case Bosses.Boss boss:
                        Equipments.Logic.DamageCalculator
                            .CalculateAndSetKnockback(boss, _knockbackChance);
                        break;
                }
            }
            TurnCounter.OnNextTurn.AddListener(CancelGeneralMode);
        }
        public override void CancelGeneralMode()
        {
            _isOn = false;
            //A warm and warm boilerplate
        }
    }
}