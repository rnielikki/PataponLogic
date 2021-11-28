using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Items
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "healing-item", menuName = "ItemDrop/Heal")]
    public class HealingItemDropData : EventItemDropData
    {
        [SerializeField]
        private int _healAmount;
        public int HealAmount => _healAmount;
        public override UnityEvent Events
        {
            get
            {
                var ev = new UnityEvent();
                ev.AddListener(Heal);
                return ev;
            }
        }
        private void Heal() => FindObjectOfType<Character.Patapons.PataponsManager>().HealAll(_healAmount);
    }
}
