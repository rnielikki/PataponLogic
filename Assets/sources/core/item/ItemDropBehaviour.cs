
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
        [Header("Item Info")]
        [SerializeField]
        private bool _dropRandomItem;
        [SerializeReference]
        ItemType _itemType;
        [SerializeReference]
        string _itemGroup;
        [SerializeReference]
        int _itemIndex;
        [SerializeReference]
        [Tooltip("This is only meaningful when item drop is random.")]
        int _maxItemIndex;

        // ----------- by image

        [SerializeField]
        private bool _useImageInsteadOfItem;

        [Header("Image Info")]
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
            _item = (_dropRandomItem) ? ItemLoader.GetRandomItem(_itemType, _itemIndex, _maxItemIndex) : ItemLoader.GetItem(_itemType, _itemGroup, _itemIndex);
        }

        public void Drop()
        {
            if (!_useImageInsteadOfItem) ItemDrop.DropItem(_item, transform.position, _timeToExist, _action, _sound);
            else ItemDrop.DropItem(_image, transform.position, _timeToExist, _action, _sound);
        }
        /// <summary>
        /// Drops random item with *predefined item index*. Especially works with equipment.
        /// </summary>
        /// <param name="rangeToAdd">Additional value that determines range additional to <see cref="_itemIndex"/>. 0 will return item only in <see cref="_itemIndex"/>.</param>
        public void DropRandomItem(int rangeToAdd)
        {
            if (_useImageInsteadOfItem) return;
            ;
        }
        public void DropRandom()
        {
            if (Random.Range(0, 1) < Mathf.Clamp01(_chanceToDrop))
            {
                if (!_useImageInsteadOfItem) ItemDrop.DropItem(_item, transform.position, _timeToExist, _action, _sound);
                else ItemDrop.DropItem(_image, transform.position, _timeToExist, _action, _sound);
            }
        }
    }
}
