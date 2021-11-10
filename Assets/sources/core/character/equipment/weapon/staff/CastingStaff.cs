using UnityEngine;

//Let me think how to create various staffs...
namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Represents staff that casting magic, e.g. throwing fireball.
    /// </summary>
    public class CastingStaff : Staff
    {
        private ParticleDamaging _particles;
        private void Start()
        {
            Init();
            //Replace 0 to Data.Index for future implementation
            var instance = Instantiate(GetWeaponInstance("Staffs/0"), transform);
            instance.layer = gameObject.layer;
            _particles = instance.GetComponent<ParticleDamaging>();
        }
        public override void NormalAttack()
        {
            _particles.Emit(1, 15);
        }
        public override void ChargeAttack()
        {
            _particles.Emit(3, 20);
        }

        public override void Defend()
        {
        }

    }
}
