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
        [SerializeField]
        private int _priority;
        //[SerializeField]
        //private bool _enabled;
        private void Start()
        {
            _size = GetComponent<Collider2D>().bounds.size.x + _offset;
        }
        //public void EnablePush() => _enabled = true;
        //public void DisablePush() => _enabled = false;
        private void OnCollisionStay2D(Collision2D collision)
        {
            var character = collision.gameObject.GetComponent<SmallCharacter>();
            if (character != null) character.transform.position = Vector2.MoveTowards(
                character.transform.position,
                transform.position - (_size + character.CharacterSize) * (Vector3)character.MovingDirection,
                Time.deltaTime);
            /*
            if (!_enabled || collision.gameObject.tag != "SmallCharacter") return;
            var character = collision.gameObject.GetComponent<SmallCharacter>();
            if (character != null)
            {
                var pushBack = character.PushBack;
                if (pushBack == null || !pushBack._enabled || pushBack._priority < _priority)
                {
                    character.transform.position = transform.position - (_size + character.CharacterSize) * (Vector3)character.MovingDirection;
                }
            }
            */
        }
    }
}
