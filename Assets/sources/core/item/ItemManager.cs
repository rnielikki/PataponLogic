using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Items
{
    internal class ItemManager : MonoBehaviour
    {
        public static ItemManager Current { get; private set; }
        readonly Dictionary<System.Guid, ItemStatusUpdater> _itemMap = new Dictionary<System.Guid, ItemStatusUpdater>();
        private RectTransform _rectTransform;

        [SerializeField]
        private AudioClip _obtainingSound;
        public AudioClip ObtainingSound => _obtainingSound;

        [SerializeField]
        private GameObject _itemDropTemplate;
        public GameObject ItemDropTemplate => _itemDropTemplate;

        [SerializeField]
        private GameObject _daedPonDropTemplate;
        public GameObject DeadPonDropTemplate => _daedPonDropTemplate;

        [SerializeField]
        private GameObject _itemInScreenTemplate;

        [SerializeField]
        [Tooltip("Parent of every dropped item.")]
        private Transform _itemDropPoint;
        public Transform ItemDropPoint => _itemDropPoint;
        private readonly HashSet<IItem> _currentItems = new HashSet<IItem>();
        private readonly HashSet<int> _dropIds = new HashSet<int>();


        private void Awake()
        {
            Current = this;
            _rectTransform = GetComponent<RectTransform>();
        }
        private void OnDestroy()
        {
            Current = null;
        }

        internal static void AddToScreen(ItemDrop itemDrop)
        {
            if (itemDrop == null || Current._dropIds.Contains(itemDrop.DropId))
            {
                if (itemDrop != null) Destroy(itemDrop.gameObject);
                return;
            }
            IItem item = itemDrop.Item;
            if (!Current._itemMap.TryGetValue(item.Id, out ItemStatusUpdater updater))
            {
                var obj = Instantiate(Current._itemInScreenTemplate, Current.transform);
                var itemStatusUpdater = obj.GetComponent<ItemStatusUpdater>();
                Current._currentItems.Add(item);
                itemStatusUpdater.SetItem(item);
                Current._itemMap.Add(item.Id, itemStatusUpdater);
            }
            else
            {
                //increase count
                updater.IncreaseCount();
            }
        }
        internal static bool HasUniqueItem(IItem item)
        {
            if (!item.IsUniqueItem || Current == null) return false;
            else return Current._currentItems.Contains(item);
        }
        public IEnumerable<ItemStatusUpdater> LoadItemStatus() => _itemMap.Values;
    }
}
