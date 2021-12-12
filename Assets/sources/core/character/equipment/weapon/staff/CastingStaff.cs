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
        private SmallCharacter _holder;
        [SerializeField]
        Color _fireColor;
        [SerializeField]
        Color _iceColor;
        [SerializeField]
        Color _thunderColor;
        public void Initialize(SmallCharacter holder)
        {
            _particles = GetComponent<ParticleDamaging>();
            _holder = holder;
        }
        private void Start()
        {
            if (_holder != null) ChangeElementalType(_holder.ElementalAttackType);
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

        private void ChangeElementalType(ElementalAttackType attackType)
        {
            var particleSystem = GetComponent<ParticleSystem>();
            var main = particleSystem.main;

            switch (attackType)
            {
                case ElementalAttackType.Fire:
                    main.startColor = new ParticleSystem.MinMaxGradient(_fireColor);
                    break;
                case ElementalAttackType.Ice:
                    main.startColor = new ParticleSystem.MinMaxGradient(_iceColor);
                    break;
                case ElementalAttackType.Thunder:
                    main.startColor = new ParticleSystem.MinMaxGradient(_thunderColor);
                    break;
                default:
                    main.startColor = Color.white;
                    break;
            }
        }
    }
}
