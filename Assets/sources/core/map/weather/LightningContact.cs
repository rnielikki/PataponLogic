using PataRoad.Core.Character;
using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    public class LightningContact : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var target = collision.GetComponentInParent<IAttackable>();
            if (target == null) return;
            if (!Character.Equipments.Logic.DamageCalculator.CalculateAndSetStatusEffect(target, StatusEffectType.Fire, 0.2f, target.Stat.FireResistance))
            {
                Character.Equipments.Logic.DamageCalculator.CalculateAndSetStagger(target, 0.5f);
            }
        }
    }
}
