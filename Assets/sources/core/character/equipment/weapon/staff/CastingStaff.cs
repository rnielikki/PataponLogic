using UnityEngine;

//Let me think how to create various staffs...
namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Represents staff that casting magic, e.g. throwing fireball.
    /// </summary>
    public class CastingStaff : MonoBehaviour, IStaffActions
    {
        private ParticleDamaging _particles;
        public void Initialize(SmallCharacter holder)
        {
            _particles = GetComponent<ParticleDamaging>();
        }
        public void NormalAttack()
        {
            _particles.Emit(1, 15);
        }
        public void ChargeAttack()
        {
            _particles.Emit(3, 20);
        }

        public void Defend()
        {
        }
    }
}
