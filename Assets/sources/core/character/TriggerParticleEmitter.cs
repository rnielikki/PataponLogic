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
        [SerializeField]
        private bool _twoSided;
        public bool TwoSided => _twoSided;
        [SerializeField]
        private bool _ignoreShield;
        public bool IgnoreShield => _ignoreShield;

        [Header("Time and force info")]
        [SerializeField]
        float _duration;
        public float Duration => _duration;
        [SerializeField]
        float _minSpeed;
        public float MinSpeed => _minSpeed;
        [SerializeField]
        float _maxSpeed;
        public float MaxSpeed => _maxSpeed;
        [SerializeField]
        bool _useWind;
        [SerializeField]
        float _particleMass = 0.01f;

        [SerializeField]
        Sprite _sprite;
        [Header("Graphic info")]
        [SerializeField]
        bool _sizeOverLifetime;
        public bool SizeOverLifetime => _sizeOverLifetime;
        [SerializeField]
        float _sizeMin;
        public float SizeMin => _sizeMin;
        [SerializeField]
        float _sizeMax;
        public float SizeMax => _sizeMax;
        [SerializeField]
        float _alphaMin;
        public float AlphaMin => _alphaMin;
        [SerializeField]
        float _alphaMax;
        public float AlphaMax => _alphaMax;
        [SerializeField]
        bool _rotationOverLifeTime;
        public bool RotationOverLifeTime => _rotationOverLifeTime;
        [SerializeField]
        float _minRotationAngle;
        public float MinRotationAngle => _minRotationAngle;
        [SerializeField]
        float _maxRotationAngle;
        public float MaxRotationAngle => _maxRotationAngle;
        void Awake()
        {
            Init();
            _particlePool = new TriggerParticle[_amount];
            for (int i = 0; i < _amount; i++)
            {
                var particleObject = new GameObject();
                particleObject.SetActive(false);
                _particlePool[i] = particleObject.AddComponent<TriggerParticle>();
                var particle = _particlePool[i];
                var spriteRenderer = particleObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = _sprite;
                var collider = particleObject.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
                collider.offset = Vector2.zero;
                collider.size = spriteRenderer.size;

                particle.Init(_useWind, _particleMass, this);
            }
        }
        public void Attack()
        {
            var pos = transform.position;
            var halfLength = _particlePool.Length / 2;
            for (int i = 0; i < _particlePool.Length; i++)
            {
                var particle = _particlePool[i];
                var angle = Random.Range(_angleMin, _angleMax) * Mathf.Deg2Rad;
                var speed = Random.Range(_minSpeed, _maxSpeed);
                if (_twoSided && i > halfLength) speed = -speed;
                particle.Throw(
                    GetRandomVectorRange(pos, _positionOffset),
                    new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed
                    );
            }
        }
        internal void SetDamage(Collider2D collider2D, Vector2 position)
        {
            if (collider2D == null) return;
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