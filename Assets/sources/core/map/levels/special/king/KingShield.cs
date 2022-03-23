using PataRoad.Core.Character;
using UnityEngine;

namespace PataRoad.Core.Map.Levels.King
{
    public class KingShield : MonoBehaviour
    {
        [SerializeField]
        bool _enabled;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_enabled && collision.CompareTag("SmallCharacter"))
            {
                var character =
                    collision.GetComponentInParent<SmallCharacter>();
                if (character != null)
                {
                    character.Die();
                }
            }
        }
    }
}