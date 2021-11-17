using UnityEngine;

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
            if (!StatusEffectManager.OnStatusEffect || collision.tag == "Attack") return;
            var attackable = collision.GetComponentInParent<IAttackable>();
            if (attackable != null &&
                !attackable.StatusEffectManager.OnStatusEffect &&
                attackable.Stat.FireResistance < 1)
            {
                attackable.StatusEffectManager.SetFire(
                    Equipments.Logic.DamageCalculator.GetFireDuration(Stat, attackable.Stat, 10)
                );
            }
        }
    }
}
