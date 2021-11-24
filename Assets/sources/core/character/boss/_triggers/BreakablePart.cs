using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BreakablePart : MonoBehaviour
    {
        [SerializeField]
        Sprite _imageOnBroken;
        [SerializeField]
        float _health;
        [SerializeField]
        float _damageMultiplierOnBroken;
        SpriteRenderer _renderer;
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
        // Start is called before the first frame update
        /// <summary>
        /// Take damage as a part and check if it's already broken.
        /// </summary>
        /// <returns>damage multiplier if ALREADY broken, otherwise just 1.</returns>
        public float TakeDamage(int damage)
        {
            if (_health <= 0) return _damageMultiplierOnBroken;
            else if (_health <= damage)
            {
                _renderer.sprite = _imageOnBroken;
            }
            _health -= damage;
            return 1;
        }
    }
}
