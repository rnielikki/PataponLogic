using PataRoad.Core.Character.Bosses;
using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Because Triggering with particle system doesn't work properly, I use this custom emitter.
    /// </summary>
    public class TriggerParticleEmitter : BossAttackComponent
    {
        TriggerParticle[] _particlePool;
        [SerializeField]
        int _amount;
        [SerializeField]
        Vector2 _positionOffset;
        public Vector2 PositionOffset => _positionOffset;
        [SerializeField]
        float _angleMin;
        public float AngleMin => _angleMin;
        [SerializeField]
        float _angleMax;
        public float AngleMax => _angleMax;

        [Header("Time and force info")]
        [SerializeField]
        float _duration;
        [SerializeField]
        float _minSpeed;
        [SerializeField]
        float _maxSpeed;
        [SerializeField]
        bool _useWind;
        [SerializeField]
        float _particleMass = 0.01f;

        [SerializeField]
        Sprite _sprite;
        void Awake()
        {
            _particlePool = new TriggerParticle[_amount];
            for (int i = 0; i < _amount; i++)
            {
                var particleObject = new GameObject();
                _particlePool[i] = particleObject.AddComponent<TriggerParticle>();
                var particle = _particlePool[i];
                particleObject.AddComponent<SpriteRenderer>().sprite = _sprite;

                particle.Init(_useWind, _particleMass, this);
            }
        }
        public void Play()
        {
            var pos = transform.position;
            foreach (var particle in _particlePool)
            {
                var angle = Random.Range(_angleMin, _angleMax) * Mathf.Deg2Rad;
                particle.Throw(
                    GetRandomVectorRange(pos, _positionOffset),
                    new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(_minSpeed, _maxSpeed),
                    _duration
                    );
            }
        }
        internal void SetDamage(Collider2D collider2D, Vector2 position)
        {
            _boss.Attack(this, collider2D.gameObject, position,
                        _attackType, _elementalAttackType, true);
        }
        private Vector2 GetRandomVectorRange(Vector2 start, Vector2 offset)
        {
            return
                new Vector2(
                    Random.Range(start.x - offset.x, start.x + offset.x),
                    Random.Range(start.y - offset.y, start.y + offset.y)
                );
        }
    }
}