using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Logic
{
    internal class DamageDisplay
    {
        private readonly GameObject _pataponDisplayObject;
        private readonly GameObject _nonPataponDisplayObject;
        internal DamageDisplay()
        {
            _pataponDisplayObject = Resources.Load<GameObject>("Common/Display/Damage/Damage.Pon");
            _nonPataponDisplayObject = Resources.Load<GameObject>("Common/Display/Damage/Damage.NonPon");
        }
        internal void DisplayDamage(int damage, Vector2 position, bool isPatapon)
        {
            var txt = Object.Instantiate((isPatapon) ? _pataponDisplayObject : _nonPataponDisplayObject);
            txt.GetComponentInChildren<TMPro.TextMeshPro>().text = damage.ToString();
            txt.transform.position = position;
        }
    }
}
