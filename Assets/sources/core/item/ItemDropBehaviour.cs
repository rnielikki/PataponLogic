
using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Drops item, when attached game object is destroyed.
    /// </summary>
    public class ItemDropBehaviour : MonoBehaviour
    {
        [SerializeReference]
        ItemMetadata _itemInfo = new ItemMetadata();
        [SerializeField]
        int _timeToExist;
        [SerializeField]
        [Tooltip("0-1 probability value. only works with DropRandom()")]
        float _chanceToDrop;

        public void Drop() => ItemDrop.DropItem(_itemInfo, transform.position, _timeToExist);
        public void DropRandom() => ItemDrop.DropItemOnRandom(_itemInfo, transform.position, _timeToExist, _chanceToDrop);
    }
}
