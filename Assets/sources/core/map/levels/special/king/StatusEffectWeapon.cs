using UnityEngine;

namespace PataRoad.Core.Map.Levels.King
{
    public class StatusEffectWeapon : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Shield"))
            {
                var boss = collision.GetComponentInParent<Character.Bosses.Boss>();
                if (boss != null && Common.Utils.RandomByProbability(0.1f))
                {
                    boss.StatusEffectManager.SetIce(Random.Range(8f, 12f));
                }
            }
        }
    }
}