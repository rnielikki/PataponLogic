namespace PataRoad.Core.Character.Bosses
{
    interface IAbsorbableBossBehaviour
    {
        public void SetAbsorbHit();
        public void Heal(int amount);
        void StopAbsorbing();
    }
}