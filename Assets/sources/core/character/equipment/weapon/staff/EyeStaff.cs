using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class EyeStaff : MonoBehaviour, IStaffData
    {
        private SmallCharacter _holder;
        private EyeStaffDamaging _damaging;
        public void Initialize(SmallCharacter holder)
        {
            _holder = holder;
            _damaging = transform.parent.GetComponentInChildren<EyeStaffDamaging>(true);
        }
        public void ChargeAttack()
        {
            NormalAttack();
        }

        public void Defend()
        {
        }

        public void NormalAttack()
        {
            _damaging.Copy(_holder);
        }
    }
}
