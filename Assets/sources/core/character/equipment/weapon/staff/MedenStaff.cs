using UnityEngine;

//Let me think how to create various staffs...
namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Meden herself doesn't attack, but boosts attack (damage)/defence (defence)/dodge (movement speed) command etc.
    /// </summary>
    public class MedenStaff : MonoBehaviour, IStaffActions
    {
        public void Initialize(SmallCharacter holder)
        {
            //----------------------
        }
        public void NormalAttack()
        {
            //My job here is done
        }
        public void ChargeAttack()
        {
            //But you didn't do anything
        }
        public void Defend()
        {
            //(going away)
        }
        public void SetElementalColor(Color color)
        {
            //-------------------
        }
    }
}
