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
        private SpriteRenderer _renderer;
        // Use this for initialization
        internal void Init(bool useWindForce, float particleMass, TriggerParticleEmitter sender)
        {
            _useWindForce = useWindForce;
            _mass = particleMass;
            gameObject.layer = sender.gameObject.layer;
            gameObject.tag = sender.gameObject.tag;

            _sender = sender;
            _renderer = GetComponent<SpriteRenderer>();
        }
        internal void Throw(Vector2 position, Vector2 speed)
        {
            transform.position = position;
            _currentSpeed = speed;
            //hide right before playing
            if (_sender.SizeOverLifetime) transform.localScale = Vector3.zero;
            if (_sender.RotationOverLifeTime) transform.Rotate(new Vector3(0, 0, _sender.MinRotationAngle));

            _enabled = true;
            _time = _sender.Duration;
            gameObject.SetActive(true);
        }
        private void Update()
        {
            if (_enabled)
            {
                //time calculation
                _time -= Time.deltaTime;
                transform.position += _currentSpeed * Time.deltaTime;
                float timeRatio = Mathf.Clamp01(_time / _sender.Duration);
                //size
                if (_sender.SizeOverLifetime)
                {
                    transform.localScale = Vector3.one * Mathf.Lerp(_sender.SizeMin, _sender.SizeMax, 1 - timeRatio);
                    _renderer.color = new Color(1, 1, 1, Mathf.Lerp(_sender.AlphaMin, _sender.AlphaMax, timeRatio));
                }
                if (_sender.RotationOverLifeTime)
                {
                    transform.Rotate(new Vector3(0, 0,
                        Mathf.Lerp(_sender.MinRotationAngle, _sender.MaxRotationAngle, 0.5f)));
                }

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
            else if (!_sender.IgnoreShield && collision.CompareTag("Shield"))
            {
                _enabled = false;
                gameObject.SetActive(false);
            }
            else _sender.SetDamage(collision, collision.ClosestPoint(transform.position));
        }
    }
}