using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    /// <summary>
    /// Defines strong and weak pont of the boss. Returns multiplier, but part itself doesn't have any HP.
    /// </summary>
    public class BossPartMultiplier : MonoBehaviour, IBossPart
    {
        [SerializeField]
        float _multiplier;
        public float TakeDamage(int damage) => _multiplier;
    }
}