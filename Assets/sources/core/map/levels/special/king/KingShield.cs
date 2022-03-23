using PataRoad.Core.Character;
using UnityEngine;

namespace PataRoad.Core.Map.Levels.King
{
    public class KingShield : MonoBehaviour
    {
        [SerializeField]
        bool _enabled;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_enabled && collision.collider.CompareTag("SmallCharacter"))
            {
                var character =
                    collision.collider.GetComponentInParent<SmallCharacter>();
                if (character != null)
                {
                    character.Die();
                }
            }
        }
    }
}