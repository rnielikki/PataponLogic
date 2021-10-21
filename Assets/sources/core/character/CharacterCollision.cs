using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Any character collision, only for sending message to <see cref="ICharacter.TakeCollision(UnityEngine.Collision2D)"/>. Attach same gameObject as where <see cref="Collider2D"/> is attached.
    /// </summary>
    internal class CharacterCollision : MonoBehaviour
    {
        private ICharacter _target;
        private void Awake()
        {
            _target = GetComponentInParent<ICharacter>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            _target.TakeCollision(collision);
        }
    }
}
