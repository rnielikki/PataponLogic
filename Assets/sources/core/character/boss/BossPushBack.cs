using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossPushBack : MonoBehaviour
    {
        private const float _offset = 0.02f;
        private float _size;
        private void Awake()
        {
            _size = GetComponent<BoxCollider2D>().size.x + _offset;
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            var character = collision.gameObject.GetComponentInParent<SmallCharacter>();
            if (character != null)
            {
                character.transform.position = transform.position - (_size + character.CharacterSize) * (Vector3)character.MovingDirection;
            }
        }
    }
}
