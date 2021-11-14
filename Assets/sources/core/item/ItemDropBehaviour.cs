
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
        ItemType _itemType;
        [SerializeReference]
        string _itemGroup;
        [SerializeReference]
        int _itemIndex;

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

        private UnityEngine.Events.UnityAction _action;
        private IItem _item;

        private void Awake()
        {
            if (_events.GetPersistentEventCount() == 0) _action = null;
            else _action = _events.Invoke;
            _item = ItemLoader.GetItem(_itemType, _itemGroup, _itemIndex);
        }

        public void Drop()
        {
            if (!_useImageInsteadOfItem) ItemDrop.DropItem(_item, transform.position, _timeToExist, _action, _sound);
            else ItemDrop.DropItem(_image, transform.position, _timeToExist, _action, _sound);
        }
        public void DropRandom()
        {
            if (!_useImageInsteadOfItem) ItemDrop.DropItemOnRandom(_item, transform.position, _timeToExist, _chanceToDrop, _action, _sound);
            else ItemDrop.DropItemOnRandom(_image, transform.position, _timeToExist, _chanceToDrop, _action, _sound);
        }
    }
}
