using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// For only dead patapon cap retrieving, which heals *only one* patapon.
    /// </summary>
    public class DeadPataponItemDrop : ItemDrop
    {
        [SerializeField]
        private EventItemDropData _data;
        private bool _wasGeneral;
        public static void Create(Vector2 position, bool wasGeneral)
        {
            var drop = GetItemDropGameObject(ItemManager.Current.DeadPonDropTemplate, position).GetComponent<DeadPataponItemDrop>();
            drop.SetItem(drop._data);
            drop._wasGeneral = wasGeneral;
        }

        protected override void DoAction(Collider2D collision)
        {
            var pon = collision.GetComponentInParent<Character.Patapons.Patapon>();
            if (pon == null) return;
            if (!_wasGeneral) pon.HealAlone(100);
            else pon.Group.HealAllInGroup(100);
        }
    }
}
