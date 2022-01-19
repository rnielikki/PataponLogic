
using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Drops item, when attached game object is destroyed.
    /// </summary>
    public class ItemDropBehaviour : MonoBehaviour
    {
        private const float _gap = 0.75f;
        [SerializeField]
        ItemDropData[] _dropData;
        public ItemDropData[] DropData => _dropData;

        public void Drop() => DropItem(false);
        public void DropRandom() => DropItem(true);
        public void DropFromChances()
        {
            var startPoint = transform.position;
            startPoint.x -= _dropData.Length * _gap * 0.5f;
            foreach (var data in _dropData)
            {
                if (!(_dropData[0] is ObtainableItemDropData obtainable)) continue;
                float random = Random.Range(0, 1f);
                float sum = 0;
                foreach (var chance in obtainable.ItemDropChances)
                {
                    sum += chance.Chance;
                    if (chance.Chance > 1) throw new System.ArgumentException("Chance sum cannot be more than 1");
                    if (sum < random)
                    {
                        ItemDrop.DropItem(data, startPoint, data.DoNotDestroy, chance.Item);
                        startPoint.x += _gap;
                        break;
                    }
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
