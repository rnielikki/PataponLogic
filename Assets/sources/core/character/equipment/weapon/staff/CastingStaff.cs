using UnityEngine;

//Let me think how to create various staffs...
namespace Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Represents staff that casting magic, e.g. throwing fireball.
    /// </summary>
    public class CastingStaff : Staff
    {
        private ParticleSystem _particles;
        private void Awake()
        {
            Init();
            //Replace 0 to Data.Index for future implementation
            var instance = Instantiate(GetWeaponInstance("Staffs/0"), transform);
            _particles = instance.GetComponent<ParticleSystem>();
        }
        public override void NormalAttack()
        {
            _particles.Emit(1);
        }
        public override void ChargeAttack()
        {
            _particles.Emit(3);
        }

        public override void Defend()
        {
        }

    }
}
