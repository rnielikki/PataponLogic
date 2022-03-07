using PataRoad.Commom;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Logic
{
    /// <summary>
    /// Displays damage *as number*. called from <see cref="DamageCalculator"/>.
    /// </summary>
    internal class DamageDisplay
    {
        private GameObject _pools;

        private const int initialCount = 30;
        private const int maxCount = 100;

        internal void DisplayDamage(int damage, Vector2 position, bool isPatapon, bool isCritical)
        {
            if (_pools == null)
            {
                _pools = GameObject.Find(nameof(GameObjectPool));
            }
            var pool = isPatapon ?
                _pools.GetComponent<GameObjectPool>().GetPool("Characters/Display/Damage/Damage.Pon", initialCount, maxCount) :
                _pools.GetComponent<GameObjectPool>().GetPool("Characters/Display/Damage/Damage.NonPon", initialCount, maxCount);

            var txt = pool.Get();
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
