using UnityEngine;

namespace PataRoad.Core.Character
{
    class Fortress : MonoBehaviour
    {
        private Hazorons.Hazoron[] _hazorons;
        private void Start()
        {
            _hazorons = GetComponentsInChildren<Hazorons.Hazoron>();
            Structure structure = GetComponent<Structure>();
            structure.OnDestroy.AddListener(() =>
            {
                foreach (var hazoron in _hazorons)
                {
                    if (hazoron != null && !hazoron.IsDead) hazoron.Die();
                }
            });
            structure.StatusEffectManager.OnStatusEffect.AddListener((type) =>
            {
                if (type == StatusEffectType.Tumble)
                {
                    foreach (var hazoron in _hazorons)
                    {
                        if (hazoron != null && !hazoron.IsDead)
                        {
                            hazoron.StatusEffectManager.Tumble();
                        }
                    }
                }
                else if (type == StatusEffectType.Fire)
                {
                    foreach (var hazoron in _hazorons)
                    {
                        if (hazoron != null && !hazoron.IsDead
                        && Common.Utils.RandomByProbability((1 - hazoron.Stat.FireResistance) * 0.4f))
                        {
                            hazoron.StatusEffectManager.SetFire(4);
                        }
                    }
                }
            });
        }
    }
}
