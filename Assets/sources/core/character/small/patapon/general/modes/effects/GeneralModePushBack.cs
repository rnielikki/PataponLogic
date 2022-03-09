using UnityEngine;

namespace PataRoad.Core.Character.Patapons.General
{
    class GeneralModePushBack : MonoBehaviour
    {
        private const float _offset = 0.2f;
        private float _size;
        protected virtual void Awake()
        {
            _size = GetComponent<Collider2D>().bounds.size.x + _offset;
        }

        private void OnDisable()
        {
            transform.localPosition = Vector2.zero;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("SmallCharacter")) return;
            var character = collision.gameObject.GetComponentInParent<SmallCharacter>();
            if (character != null && character.Stat.KnockbackResistance != Mathf.Infinity)
            {
                character.StatusEffectManager?.SetKnockback();
            }
        }
        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            //unlike pushback it works also for non-small characters like bosses.
            var character = collision.gameObject.GetComponentInParent<ICharacter>();
            var behaviour = character as MonoBehaviour;
            if (character != null && behaviour != null)
            {
                behaviour.transform.position = Vector2.MoveTowards(
                behaviour.transform.position,
                transform.position - ((_size + character.CharacterSize) * (Vector3)character.MovingDirection),
                Time.deltaTime);
            }
        }

    }
}
