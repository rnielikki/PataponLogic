using UnityEngine;

namespace Core.Character.Equipment.Logic
{
    internal class DamageDisplay
    {
        private readonly GameObject _displayObject;
        internal DamageDisplay()
        {
            _displayObject = Resources.Load<GameObject>("Common/Display/Damage/Damage");
        }
        internal void DisplayDamage(int damage, Vector2 position)
        {
            var txt = Object.Instantiate(_displayObject);
            txt.GetComponentInChildren<TMPro.TextMeshPro>().text = damage.ToString();
            txt.transform.position = position;
        }
    }
}
