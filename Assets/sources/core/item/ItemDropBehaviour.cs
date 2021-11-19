
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

        public void Drop() => DropItem(false);
        public void DropRandom() => DropItem(true);
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
