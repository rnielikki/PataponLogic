namespace PataRoad.Core.Character.Equipments.Weapons
{
    interface IStaffActions
    {
        public void Initialize(SmallCharacter holder);
        public void NormalAttack();
        public void ChargeAttack();
        public void Defend();
    }
}
