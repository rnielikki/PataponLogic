namespace PataRoad.Core.Character
{
    class BossStatusEffectManager : CharacterStatusEffectManager
    {
        void Awake() { Init(); }
        public override void SetKnockback()
        {
            _character.CharAnimator.Animate("knockback");
            //status effects for seconds....
        }
    }
}
