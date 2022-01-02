using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class EyeStaffDamaging : MonoBehaviour
    {
        private ParticleSystem _particles;
        private SmallCharacter _holder;
        private Stat _stat;
        private Collider2D _collider;

        private void Initialize(SmallCharacter holder)
        {
            _particles = GetComponent<ParticleSystem>();
            _collider = GetComponent<Collider2D>();
            var renderer = GetComponent<SpriteRenderer>();
            _holder = holder;

            _stat = _holder.Stat;
            _collider.enabled = true;
            renderer.enabled = true;

            StartCoroutine(DisappearAfterTime());

            System.Collections.IEnumerator DisappearAfterTime()
            {
                yield return new WaitForSeconds(Mathf.Min(_stat.AttackSeconds, Rhythm.RhythmEnvironment.TurnSeconds));
                Destroy(gameObject);
            }
        }

        public void Copy(SmallCharacter holder)
        {
            var pos = holder.DistanceCalculator.GetClosestForAttack();
            if (pos == null) return;
            var position = pos.Value;

            var obj = Instantiate(gameObject);
            position.x += Random.Range(-0.5f, 4.5f);
            obj.transform.position = position;
            obj.SetActive(true);
            var comp = obj.GetComponent<EyeStaffDamaging>();
            comp.Initialize(holder);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_collider == null || !_collider.enabled) return;
            var point = collision.ClosestPoint(transform.position);

            _particles.Play();

            Logic.DamageCalculator.DealDamage(_holder, _stat, collision.gameObject, point);
        }
    }
}
