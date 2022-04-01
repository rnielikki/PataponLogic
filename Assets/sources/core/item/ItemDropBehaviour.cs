
using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Drops item, when attached game object is destroyed.
    /// </summary>
    public class ItemDropBehaviour : MonoBehaviour
    {
        protected const float _gap = 0.75f;
        [SerializeField]
        protected ItemDropData[] _dropData;
        public virtual ItemDropData[] DropData => _dropData;

        public virtual void Drop() => DropItem(false);
        public void DropRandom() => DropItem(true);
        /// <summary>
        /// If has one, it drops another. If have all, it doesn't drop at all.
        /// </summary>
        public void DropWithAlternativeChoice()
        {
            foreach (var data in _dropData)
            {
                if (data is ObtainableItemDropData obtainable
                    && !Global.GlobalData.CurrentSlot.Inventory.HasItem(obtainable.Item))
                {
                    ItemDrop.DropItem(data, transform.position, data.DoNotDestroy);
                    return;
                }
            }
        }
        private void DropItem(bool random)
        {
            var startPoint = transform.position;
            startPoint.x -= _dropData.Length * _gap * 0.5f;
            if (random)
            {
                foreach (var data in _dropData)
                {
                    if (Common.Utils.RandomByProbability(data.ChanceToDrop))
                    {
                        ItemDrop.DropItem(data, startPoint, data.DoNotDestroy);
                    }
                    startPoint.x += _gap;
                }
            }
            else
            {
                foreach (var data in _dropData)
                {
                    ItemDrop.DropItem(data, startPoint, data.DoNotDestroy);
                    startPoint.x += _gap;
                }
            }
        }
    }
}
