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
            if (Common.Utils.RandomByProbability(target.AttackTypeResistance.ThunderMultiplier * 0.8f)
                && !Character.Equipments.Logic.DamageCalculator.CalculateAndSetStatusEffect(target, StatusEffectType.Fire, 0.1f, target.Stat.FireResistance))
            {
                Character.Equipments.Logic.DamageCalculator.CalculateAndSetStagger(target, 0.5f);
            }
        }
    }
}
