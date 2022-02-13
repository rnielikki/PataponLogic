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
        protected SmallCharacter _holder;
        [Header("EmitAmount info")]
        [SerializeField]
        int _emitAmountOnAttack;
        [SerializeField]
        int _emitSpeedOnAttack;
        [SerializeField]
        int _emitAmountOnChargeAttack;
        [SerializeField]
        int _emitSpeedOnChargeAttack;
        public virtual void Initialize(SmallCharacter holder)
        {
            _particles = GetComponent<ParticleDamaging>();
            _particles.Init(holder);
            _holder = holder;

            var collision = GetComponent<ParticleSystem>().collision;
            collision.collidesWith = CharacterTypeDataCollection.GetCharacterDataByType(_holder).AttackTargetLayerMask;
        }
        public virtual void NormalAttack()
        {
            _particles.Emit(_emitAmountOnAttack, _emitSpeedOnAttack);
        }
        public virtual void ChargeAttack()
        {
            _particles.Emit(_emitAmountOnChargeAttack, _emitSpeedOnChargeAttack);
        }

        public virtual void Defend()
        {
            //nothing.
        }

        public void SetElementalColor(Color color)
        {
            var particleSystem = GetComponent<ParticleSystem>();
            var main = particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }
}
