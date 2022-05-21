using UnityEngine;

namespace PataRoad.Core.Character
{
    public class TriggerParticle : MonoBehaviour
    {
        private bool _useWindForce;
        private Vector3 _currentSpeed;
        private bool _enabled;
        private float _time;
        private float _mass;
        private TriggerParticleEmitter _sender;
        // Use this for initialization
        internal void Init(bool useWindForce, float particleMass, TriggerParticleEmitter sender)
        {
            _useWindForce = useWindForce;
            _mass = particleMass;
            gameObject.layer = sender.gameObject.layer;
            gameObject.tag = sender.gameObject.tag;

            _sender = sender;
        }
        internal void Throw(Vector2 position, Vector2 speed, float time)
        {
            transform.position = position;
            _currentSpeed = speed;
            _enabled = true;
            _time = time;
            gameObject.SetActive(true);
        }
        private void Update()
        {
            if (_enabled)
            {
                _time -= Time.deltaTime;
                transform.position += _currentSpeed * Time.deltaTime;
                _enabled = _time > 0;
                if (!_enabled) gameObject.SetActive(false);
            }
        }
        private void FixedUpdate()
        {
            if (_enabled && _useWindForce)
            {
                transform.position +=
                    Map.Weather.WeatherInfo.Current.Wind.Magnitude * 0.5f
                    * Time.fixedDeltaTime * Time.fixedDeltaTime * Vector3.right / _mass;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_enabled) return;
            if (collision.CompareTag("Ground"))
            {
                _currentSpeed.y = 0;
            }
            else if (collision.CompareTag("Shield"))
            {
                _enabled = false;
                gameObject.SetActive(false);
            }
            else _sender.SetDamage(collision, collision.ClosestPoint(transform.position));
        }
    }
}