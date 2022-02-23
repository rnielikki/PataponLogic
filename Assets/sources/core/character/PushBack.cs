using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Pushing back targetss on body contact if priority is higher than the target.
    /// </summary>
    public class PushBack : MonoBehaviour
    {
        private const float _offset = 0.2f;
        private float _size;
        protected virtual void Start()
        {
            _size = GetComponent<Collider2D>().bounds.size.x + _offset;
        }
        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag != "SmallCharacter") return;
            var character = collision.gameObject.GetComponentInParent<SmallCharacter>();
            if (character != null)
            {
                character.transform.position = Vector2.MoveTowards(
                character.transform.position,
                transform.position - (_size + character.CharacterSize) * (Vector3)character.MovingDirection,
                Time.deltaTime);
            }
        }
    }
}
