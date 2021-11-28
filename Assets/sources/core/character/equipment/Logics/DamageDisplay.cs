using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Logic
{
    /// <summary>
    /// Displays damage *as number*. called from <see cref="DamageCalculator"/>.
    /// </summary>
    internal class DamageDisplay
    {
        private readonly GameObject _pataponDisplayObject;
        private readonly GameObject _nonPataponDisplayObject;
        internal DamageDisplay()
        {
            _pataponDisplayObject = Resources.Load<GameObject>("Characters/Display/Damage/Damage.Pon");
            _nonPataponDisplayObject = Resources.Load<GameObject>("Characters/Display/Damage/Damage.NonPon");
        }
        internal void DisplayDamage(int damage, Vector2 position, bool isPatapon, bool isCritical)
        {
            var txt = Object.Instantiate((isPatapon) ? _pataponDisplayObject : _nonPataponDisplayObject);
            var txtPro = txt.GetComponentInChildren<TMPro.TextMeshPro>();
            txtPro.text = damage.ToString();
            txt.transform.position = position;
            if (isCritical)
            {
                txtPro.fontWeight = TMPro.FontWeight.Heavy;
            }
        }
    }
}
