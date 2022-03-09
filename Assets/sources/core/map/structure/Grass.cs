using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Character
{
    public class Grass : MonoBehaviour, IAttackable
    {
        [SerializeReference]
        private Stat _stat = new Stat();
        public Stat Stat => _stat;
        [SerializeField]
        private Sprite _deadImage;
        private SpriteRenderer _renderer;

        [SerializeField]
        private Gradient _colorOverHealth;

        public int CurrentHitPoint { get; private set; }

        public StatusEffectManager StatusEffectManager { get; private set; }
        public Collider2D Collider { get; private set; }

        public bool IsDead { get; private set; }

        public UnityEvent<float> OnDamageTaken => null;

        [SerializeReference]
        private AttackTypeResistance _attackTypeResistance = new AttackTypeResistance();
        public AttackTypeResistance AttackTypeResistance => _attackTypeResistance;

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<Collider2D>();
            CurrentHitPoint = _stat.HitPoint;
            StatusEffectManager = gameObject.AddComponent<GrassStatusEffectManager>();
        }
        public void Die()
        {
            IsDead = true;
            StatusEffectManager.Recover();
            Collider.enabled = false;
            _renderer.sprite = _deadImage;
            _renderer.color = Color.black;
        }

        public void TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            var percent = Mathf.Clamp01((float)CurrentHitPoint / Stat.HitPoint);
            _renderer.color = _colorOverHealth.Evaluate(percent);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!StatusEffectManager.IsOnStatusEffect || collision.CompareTag("Attack")) return;
            var attackable = collision.GetComponentInParent<IAttackable>();
            if (attackable != null &&
                !attackable.StatusEffectManager.IsOnStatusEffect)
            {
                Equipments.Logic.DamageCalculator.CalculateAndSetStatusEffect(
                    attackable, StatusEffectType.Fire, Stat.FireRate, Stat.FireResistance, Map.Weather.WeatherInfo.Current.FireRateMultiplier);
            }
        }

        public float GetDefenceValueOffset() => 1;
    }
}
