using UnityEngine;

namespace PataRoad.Core.Character
{
    class Fortress : MonoBehaviour
    {
        private void Start()
        {
            Structure structure = GetComponent<Structure>();
            structure.OnDestroy.AddListener(() =>
            {
                foreach (var hazoron in GetComponentsInChildren<Hazorons.Hazoron>())
                {
                    hazoron.Die();
                }
            });
            structure.StatusEffectManager.OnStatusEffect.AddListener((type) =>
            {
                if (type == StatusEffectType.Tumble)
                {
                    foreach (var hazoron in GetComponentsInChildren<Hazorons.Hazoron>())
                    {
                        hazoron.StatusEffectManager.Tumble();
                    }
                }
                else if (type == StatusEffectType.Fire)
                {
                    foreach (var hazoron in GetComponentsInChildren<Hazorons.Hazoron>())
                    {
                        if (Common.Utils.RandomByProbability((1 - hazoron.Stat.FireResistance) * 0.4f))
                            hazoron.StatusEffectManager.SetFire(4);
                    }
                }
            });
        }
    }
}
