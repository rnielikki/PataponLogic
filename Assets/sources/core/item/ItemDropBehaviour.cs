
using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Drops item, when attached game object is destroyed.
    /// </summary>
    public class ItemDropBehaviour : MonoBehaviour
    {

        [SerializeField]
        int _timeToExist;
        [SerializeField]
        [Tooltip("0-1 probability value. only works with DropRandom()")]
        float _chanceToDrop;

        // ------- by item
        [SerializeReference]
        ItemMetadata _itemInfo = new ItemMetadata();

        // ----------- by image

        [SerializeField]
        private bool _useImageInsteadOfItem;

        [SerializeField]
        private Sprite _image;

        // -------- action
        [SerializeField]
        UnityEngine.Events.UnityEvent _events;

        [SerializeField]
        AudioClip _sound;

        public void Drop()
        {
            if (!_useImageInsteadOfItem) ItemDrop.DropItem(_itemInfo, transform.position, _timeToExist, GetActionTarget(), _sound);
            else ItemDrop.DropItem(_image, transform.position, _timeToExist, GetActionTarget(), _sound);
        }
        public void DropRandom()
        {
            if (!_useImageInsteadOfItem) ItemDrop.DropItemOnRandom(_itemInfo, transform.position, _timeToExist, _chanceToDrop, GetActionTarget(), _sound);
            else ItemDrop.DropItemOnRandom(_image, transform.position, _timeToExist, _chanceToDrop, GetActionTarget(), _sound);
        }
        private UnityEngine.Events.UnityAction GetActionTarget()
        {
            if (_events.GetPersistentEventCount() == 0) return null;
            else return _events.Invoke;
        }
    }
}
