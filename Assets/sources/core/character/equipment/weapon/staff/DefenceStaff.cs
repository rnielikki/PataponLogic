using PataRoad.Core.Character.Patapons;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class DefenceStaff : StatUpdatingStaff
    {
        private bool _performing;

        protected override void InitEach(Patapon patapon)
        {
        }
        protected override void PerformActionEach(Patapon patapon)
        {
            patapon.Stat.DefenceMin += 1.5f;
            patapon.Stat.DefenceMax += 2f;
        }
        protected override void OnPerformEnd()
        {
        }
    }
}
