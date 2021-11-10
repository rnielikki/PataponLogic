using UnityEngine;

//Let me think how to create various staffs...
namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Meden herself doesn't attack, but boosts attack (damage)/defence (defence)/dodge (movement speed) command etc.
    /// </summary>
    public class MedenStaff : Staff
    {
        private void Start()
        {
            Init();
        }
        public override void NormalAttack()
        {
        }
        public override void ChargeAttack()
        {
        }

        public override void Defend()
        {
        }

    }
}
