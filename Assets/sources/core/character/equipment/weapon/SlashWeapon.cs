using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    /// <summary>
    /// Slash: Each time it enters collision, it deals damage. Most simple type.
    /// </summary>
    public class SlashWeapon : MeleeWeapon
    {
        private void Start()
        {
            Init();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            DealDamage(collision);
        }
    }
}
